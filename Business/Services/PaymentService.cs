using Models;
using DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Business
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankService _bankService;
        private readonly AppDbContext _dbContext;

        public PaymentService(IBankService bankService, AppDbContext dbContext)
        {
            _bankService = bankService;
            _dbContext = dbContext;
        }

        public static bool ValidateCreditCard(string creditCardNumber)
        {
            // The Luhn algorithm, also known as the modulus 10 or mod 10 algorithm,
            // is a simple checksum formula used to validate a variety of identification numbers,
            // such as credit card numbers, IMEI numbers, Canadian Social Insurance Numbers.
            if (string.IsNullOrEmpty(creditCardNumber))
                return false;

            int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                                              .Reverse()
                                              .Select((e, i) => (e - 48) * (i % 2 == 0 ? 1 : 2))
                                              .Sum((e) => (e / 10) + (e % 10));

            return sumOfDigits % 10 == 0;
        }

        public async Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto dto)
        {
            // validate card info before sending to bank
            var isValid = ValidateCreditCard(dto.CardNumber);
            if (!isValid)
            {
                return new PurchaseResultDto(Guid.Empty, PaymentStatus.Failed, "Invalid Credit Card.");
            }

            var response = await _bankService.ProcessPaymentAsync(dto);

            await UpdateDb(dto, response);

            return response;
        }

        private async Task UpdateDb(PurchaseRequestDto dto, PurchaseResultDto response)
        {
            var product = Constants.ProductPrices[dto.Product];
            var payment = new Payment
            {
                Id = response.Id,
                Amount = product.Price,
                Currency = product.Currency,
                IsSuccessful = response.PaymentStatus == PaymentStatus.Successful,
                CreatedDate = DateTime.UtcNow
            };

            var shopper = await _dbContext.Shoppers
                .Where(x => x.CardNumber == dto.CardNumber
                && x.Cvv == dto.Cvv
                && x.ExpireYear == dto.ExpireYear
                && x.ExpireMonth == dto.ExpireMonth
                && x.FirstName == dto.FirstName
                && x.LastName == dto.LastName)
                .SingleOrDefaultAsync();

            if (shopper is null)
            {
                var newShopper = new Shopper()
                {
                    CardNumber = dto.CardNumber,
                    Payments = new List<Payment> { payment }
                };
                _dbContext.Shoppers.Add(newShopper);
            }
            else
            {
                shopper.Payments.Add(payment);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PaymentResponseDto>> GetPaymentsAsync(PaymentsRequestDto paymentsRequestDto)
        {
            var query = _dbContext.Payments.AsQueryable();

            if (paymentsRequestDto.ShopperId.HasValue)
                query = query.Where(x => x.ShopperId == paymentsRequestDto.ShopperId);
            if (paymentsRequestDto.MerchantId.HasValue)
                query = query.Where(x => x.MerchantId == paymentsRequestDto.MerchantId);
            if (paymentsRequestDto.PaymentId.HasValue)
                query = query.Where(x => x.Id == paymentsRequestDto.PaymentId);

            var result = await query.Select(x => new PaymentResponseDto(x.Id, x.Amount, x.Currency.ToString(), x.IsSuccessful, x.CreatedDate, x.ShopperId))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
