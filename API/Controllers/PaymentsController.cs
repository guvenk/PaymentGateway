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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Name are used for API Client generation to produce friendly names
        [HttpGet(Name = nameof(GetPaymentAsync))]
        [Authorize]
        public async Task<ActionResult<PaymentResponseDto>> GetPaymentAsync(Guid PaymentId)
        {
            var payments = await _paymentService.GetPaymentAsync(PaymentId);

            if (payments is null) return NotFound();

            return Ok(payments);
        }

        //Sample input
        //{
        //  "product": 2,
        //  "firstName": "guven",
        //  "lastName": "kurt",
        //  "cardNumber": "2222 4000 7000 0005",
        //  "expireMonth": 12,
        //  "expireYear": 2024,
        //  "cvv": "444"
        //}
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
