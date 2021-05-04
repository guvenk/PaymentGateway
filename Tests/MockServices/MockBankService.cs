using Business;
using Models;
using System.Threading.Tasks;

namespace Tests
{
    public class MockBankService : IBankService
    {
        public async Task<PurchaseResultDto> ProcessPaymentAsync(PurchaseRequestDto purchaseRequestDto)
        {
            var result = new PurchaseResultDto(TestConstants.TestPaymentId, PaymentStatus.Successful);

            return await Task.FromResult(result);
        }
    }
}
