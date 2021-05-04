using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet(Name = nameof(GetPaymentAsync))]
        [Authorize]
        public async Task<ActionResult<PaymentResponseDto>> GetPaymentAsync(Guid PaymentId)
        {
            var payments = await _paymentService.GetPaymentAsync(PaymentId);

            if (payments is null) return NotFound();

            return Ok(payments);
        }

        [HttpPost(Name = nameof(PurchaseProductAsync))]
        [Authorize]
        public async Task<ActionResult<PurchaseResultDto>> PurchaseProductAsync(PurchaseRequestDto buyProductDto)
        {
            var result = await _paymentService.PurchaseProductAsync(buyProductDto);

            if (result.PaymentStatus == PaymentStatus.Successful)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
