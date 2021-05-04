using System;
using System.Threading.Tasks;

namespace PaymentAPI
{
    public interface IPaymentAPIClient
    {
        Task<PaymentResponseDto> GetPaymentAsync(Guid? paymentId);

        Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto body);

    }
}
