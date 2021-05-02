using Business;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Merchant")]
        public async Task<IActionResult> GetPaymentsAsync(PaymentsRequestDto paymentsDto)
        {
            var payments = await _paymentService.GetPaymentsAsync(paymentsDto);

            if (payments is null) return NotFound();

            return Ok(payments);
        }

        [HttpPost]
        //[Authorize(Policy = "Shopper")]
        public async Task<IActionResult> BuyProductAsync(PurchaseRequestDto buyProductDto)
        {
            var success = await _paymentService.BuyProductAsync(buyProductDto);

            if (success == PaymentStatus.Successful)
                return Ok(success);
            else
                return BadRequest(success);
        }
    }
}
