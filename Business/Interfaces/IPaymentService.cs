using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public interface IPaymentService
    {
        Task<List<PaymentResponseDto>> GetPaymentsAsync(PaymentsRequestDto paymentsRequestDto);

        Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto purchaseRequestDto);
    }
}
