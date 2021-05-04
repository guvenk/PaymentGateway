using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public record PaymentResponseDto(string CardNumber, string FirstName, string LastName, int ExpireMonth, int ExpireYear, int Cvv, PaymentStatus PaymentStatus);

    public record PurchaseRequestDto(
        [Required] Product Product,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string CardNumber,
        [Range(1, int.MaxValue)] int ExpireMonth,
        [Range(1, int.MaxValue)] int ExpireYear,
        [Range(100, 999)] int Cvv);

    public record PurchaseResultDto(Guid Id, PaymentStatus PaymentStatus);
}
