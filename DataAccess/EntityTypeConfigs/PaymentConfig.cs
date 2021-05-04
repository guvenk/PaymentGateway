using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;

namespace DataAccess.EntityTypeConfigs
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable(nameof(Payment));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Amount)
                .IsRequired()
                .HasColumnType("decimal(15,3)");

            builder.Property(a => a.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.PaymentStatus)
                .IsRequired();

            builder.Property(a => a.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            var payments = new List<Payment>{
                new Payment
                {
                    Id = Guid.Parse("0A036FBA-2BBF-4530-A90C-C0D07C3FD23A"),
                    Amount = 100.000M,
                    CreatedDate = DateTime.UtcNow,
                    Currency =  Currency.USD,
                    PaymentStatus = PaymentStatus.Successful,
                    MerchantId = 1L,
                    ShopperId = 1L,
                }
            };

            builder.HasData(payments);

        }
    }
}
