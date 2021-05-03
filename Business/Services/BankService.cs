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
            string failureReason = isSuccessfull ? null : "Acquiring bank failed to process the transaction.";

            var result = new PurchaseResultDto(Guid.NewGuid(), status, failureReason);
            return await Task.FromResult(result);
        }
    }
}
