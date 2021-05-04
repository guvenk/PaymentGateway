using Models;
using System;
using System.Threading.Tasks;

namespace Business
{
    public class BankService : IBankService
    {
        public async Task<PurchaseResultDto> ProcessPaymentAsync(PurchaseRequestDto purchaseRequestDto)
        {
            Random rnd = new();
            bool isSuccessful = rnd.NextDouble() >= 0.5;
            var status = isSuccessful ? PaymentStatus.Successful : PaymentStatus.Failed;

            var result = new PurchaseResultDto(Guid.NewGuid(), status);
            return await Task.FromResult(result);
        }
    }
}
