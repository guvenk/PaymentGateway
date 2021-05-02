using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public record PaymentsRequestDto(long? MerchantId, long? ShopperId, Guid? PaymentId);

    public record PaymentResponseDto(Guid Id, decimal Amount, string Currency, bool IsSuccessful, DateTime CreatedDate, long ShopperId);

    public record PurchaseRequestDto(
        [Required] Product Product,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string CardNumber,
        [Range(1, int.MaxValue)] int ExpireMonth,
        [Range(1, int.MaxValue)] int ExpireYear,
        [Range(100, 999)] int Cvv);

}
