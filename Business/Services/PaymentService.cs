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

        public async Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto dto)
        {
            var isValid = CreditCard.Validate(dto.CardNumber);
            if (!isValid)
                return new PurchaseResultDto(Guid.Empty, PaymentStatus.Failed);

            var response = await _bankService.ProcessPaymentAsync(dto);

            await UpdateDb(dto, response);

            return response;
        }

        private async Task UpdateDb(PurchaseRequestDto request, PurchaseResultDto response)
        {
            var product = Constants.ProductPrices[request.Product];
            var payment = new Payment
            {
                Id = response.Id,
                Amount = product.Price,
                Currency = product.Currency,
                PaymentStatus = response.PaymentStatus,
                CreatedDate = DateTime.UtcNow,
                MerchantId = product.MerchantId
            };

            var shopper = await _dbContext.Shoppers
                .Include(x => x.Payments)
                .SingleOrDefaultAsync(x => x.CardNumber == request.CardNumber);

            if (shopper is null)
            {
                var newShopper = new Shopper()
                {
                    CardNumber = request.CardNumber,
                    Cvv = request.Cvv,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ExpireMonth = request.ExpireMonth,
                    ExpireYear = request.ExpireYear,
                    Payments = new List<Payment> { payment }
                };
                _dbContext.Shoppers.Add(newShopper);
            }
            else
            {
                payment.ShopperId = shopper.Id;
                _dbContext.Payments.Add(payment);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaymentResponseDto> GetPaymentsAsync(Guid PaymentId)
        {
            var result = await _dbContext.Payments
                .Include(x => x.Shopper)
                .Where(x => x.Id == PaymentId)
                .Select(x => new PaymentResponseDto(
                CreditCard.GetMasked(x.Shopper.CardNumber),
                x.Shopper.FirstName,
                x.Shopper.LastName,
                x.Shopper.ExpireMonth,
                x.Shopper.ExpireYear,
                x.PaymentStatus))
                .AsNoTracking()
                .SingleOrDefaultAsync();

            return result;
        }
    }
}
