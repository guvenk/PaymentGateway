using Models;
using DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace Business
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankService _bankService;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IBankService bankService, AppDbContext dbContext, ILogger<PaymentService> logger)
        {
            _bankService = bankService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto dto)
        {
            var isValid = CreditCard.Validate(dto.CardNumber);
            if (!isValid)
                return new PurchaseResultDto(Guid.Empty, PaymentStatus.Failed);

            // Validate if product exists
            if (!Enum.IsDefined(typeof(Product), dto.Product))
                return new PurchaseResultDto(Guid.Empty, PaymentStatus.Failed);

            var response = await _bankService.ProcessPaymentAsync(dto);

            _logger.LogInformation($"Bank Payment Status : {response.PaymentStatus}");

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

            int total = await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Number of state entries written to db: {total}");
        }

        public async Task<PaymentResponseDto> GetPaymentAsync(Guid paymentId)
        {
            var result = await _dbContext.Payments
                .Include(x => x.Shopper)
                .Where(x => x.Id == paymentId)
                .Select(x => new PaymentResponseDto(
                CreditCard.GetMasked(x.Shopper.CardNumber),
                x.Shopper.FirstName,
                x.Shopper.LastName,
                x.Shopper.ExpireMonth,
                x.Shopper.ExpireYear,
                x.PaymentStatus))
                .AsNoTracking()
                .SingleOrDefaultAsync();

            _logger.LogInformation($"GetPayment with id: {paymentId} called");

            return result;
        }
    }
}
