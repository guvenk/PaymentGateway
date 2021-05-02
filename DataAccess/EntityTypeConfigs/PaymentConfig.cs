using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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

            builder.Property(x => x.IsSuccessful)
                .IsRequired();

            builder.Property(a => a.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");
        }
    }
}
