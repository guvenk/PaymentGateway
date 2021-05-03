using Models;
using System.Threading.Tasks;

namespace Business
{
    public interface IBankService
    {
        Task<PurchaseResultDto> ProcessPaymentAsync(PurchaseRequestDto purchaseRequestDto);
    }
}
