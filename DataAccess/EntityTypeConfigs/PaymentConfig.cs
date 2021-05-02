using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityTypeConfigs
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable(nameof(Payment));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CardNumber)
                .IsRequired()
                .HasMaxLength(22);

            builder.Property(a => a.ExpireMonth)
                .IsRequired();

            builder.Property(a => a.ExpireYear)
                .IsRequired();

            builder.Property(a => a.Amount)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(a => a.Currency)
                .IsRequired();

            builder.Property(a => a.Cvv)
                .IsRequired();

            builder.Property(a => a.CreateDate)
                .IsRequired();
        }
    }
}
