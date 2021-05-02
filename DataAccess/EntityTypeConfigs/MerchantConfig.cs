using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityTypeConfigs
{
    public class MerchantConfig : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable(nameof(Merchant));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Merchant)
                .HasForeignKey(x => x.MerchantId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            var merchant = new Merchant
            {
                Id = 1L,
                Name = "Amazon",
            };

            builder.HasData(merchant);
        }
    }
}
