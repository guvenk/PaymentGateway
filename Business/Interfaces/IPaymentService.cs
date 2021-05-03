using Models;
using System;
using System.Threading.Tasks;

namespace Business
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> GetPaymentAsync(Guid PaymentId);

        Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto purchaseRequestDto);
    }
}
