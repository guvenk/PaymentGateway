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

        // Name are used for API Client generation to produce friendly names
        [HttpGet(Name = nameof(GetPaymentAsync))]
        //[Authorize]
        public async Task<ActionResult<PaymentResponseDto>> GetPaymentAsync(Guid PaymentId)
        {
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var strEnc = EncryptionUtil.Encrypt("333", "b14ca5898a4e4133bbce2ea2315a1916");
            //var strDecrypted = EncryptionUtil.Decrypt(strEnc, "b14ca5898a4e4133bbce2ea2315a1916");

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
