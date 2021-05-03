using Models;
using DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Business
{
    public partial class PaymentService : IPaymentService
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
                IsSuccessful = response.PaymentStatus == PaymentStatus.Successful,
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
