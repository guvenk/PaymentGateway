using Models;
using System.Threading.Tasks;

namespace Business
{
    public interface IBankService
    {
        Task<PaymentStatus> ProcessPaymentAsync(PurchaseRequestDto purchaseRequestDto);
    }
}
