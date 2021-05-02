using Models;
using DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<PaymentStatus> BuyProductAsync(PurchaseRequestDto purchaseRequestDto)
        {
            // some validations
            
            var response = await _bankService.ProcessPaymentAsync(purchaseRequestDto);

            return response;
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

            var result = await query.Select(x => new PaymentResponseDto(x.Id, x.Amount, x.Currency, x.IsSuccessful, x.CreatedDate, x.ShopperId))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
