using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> GetPaymentsAsync(Guid PaymentId);

        Task<PurchaseResultDto> PurchaseProductAsync(PurchaseRequestDto purchaseRequestDto);
    }
}
