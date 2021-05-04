using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public record PaymentResponseDto(string CardNumber, string FirstName, string LastName, int ExpireMonth, int ExpireYear, string Cvv, PaymentStatus PaymentStatus);

    public record PurchaseRequestDto(
        [Required] Product Product,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string CardNumber,
        [Range(1, 12)] int ExpireMonth,
        [Range(1, 9999)] int ExpireYear,
        [Required][StringLength(3, MinimumLength = 3)] string Cvv);

    public record PurchaseResultDto(Guid Id, PaymentStatus PaymentStatus);
}
