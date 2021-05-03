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
            bool isSuccessfull = rnd.NextDouble() >= 0.5;
            var status = isSuccessfull ? PaymentStatus.Successful : PaymentStatus.Failed;

            var result = new PurchaseResultDto(Guid.NewGuid(), status);
            return await Task.FromResult(result);
        }
    }
}
