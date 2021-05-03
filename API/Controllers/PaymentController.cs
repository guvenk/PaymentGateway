using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsAsync(Guid PaymentId)
        {
            var payments = await _paymentService.GetPaymentsAsync(PaymentId);

            if (payments is null) return NotFound();

            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseProductAsync(PurchaseRequestDto buyProductDto)
        {
            var result = await _paymentService.PurchaseProductAsync(buyProductDto);

            if (result.PaymentStatus == PaymentStatus.Successful)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
