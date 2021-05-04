using Models;
using DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Business
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankService _bankService;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentService> _logger;
        private readonly string _encryptionKey;
        public PaymentService(IBankService bankService, AppDbContext dbContext, ILogger<PaymentService> logger, IConfiguration configuration)
        {
            _bankService = bankService;
            _dbContext = dbContext;
            _logger = logger;
            _encryptionKey = configuration["EncryptionKey"].ToString();
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

            string cardNumber = request.CardNumber.Encrypt(_encryptionKey);
            string cvv = request.Cvv.ToString().Encrypt(_encryptionKey);

            var shopper = await _dbContext.Shoppers
                .Include(x => x.Payments)
                .SingleOrDefaultAsync(x => x.CardNumber == cardNumber && x.Cvv == cvv);

            if (shopper is null)
            {
                var newShopper = new Shopper()
                {
                    CardNumber = cardNumber,
                    Cvv = cvv,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ExpireMonth = request.ExpireMonth,
                    ExpireYear = request.ExpireYear,
                    Payments = new List<Payment> { payment }
                };

                _dbContext.Shoppers.Add(newShopper);
                _logger.LogInformation($"New shopper created: {request.FirstName} {request.LastName}");
            }
            else
            {
                payment.ShopperId = shopper.Id;
                _dbContext.Payments.Add(payment);
            }

            int total = await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Number of entries written to db: {total}");
        }

        public async Task<PaymentResponseDto> GetPaymentAsync(Guid paymentId)
        {
            var result = await _dbContext.Payments
                .Include(x => x.Shopper)
                .Where(x => x.Id == paymentId)
                .Select(x => new PaymentResponseDto(
                CreditCard.GetMasked(x.Shopper.CardNumber.Decrypt(_encryptionKey)),
                x.Shopper.FirstName,
                x.Shopper.LastName,
                x.Shopper.ExpireMonth,
                x.Shopper.ExpireYear,
                x.Shopper.Cvv.Decrypt(_encryptionKey),
                x.PaymentStatus))
                .AsNoTracking()
                .SingleOrDefaultAsync();

            _logger.LogInformation($"GetPayment with id: {paymentId} called");

            return result;
        }
    }
}
