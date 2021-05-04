using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityTypeConfigs
{
    public class ShopperConfig : IEntityTypeConfiguration<Shopper>
    {
        public void Configure(EntityTypeBuilder<Shopper> builder)
        {
            builder.ToTable(nameof(Shopper));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
               .IsRequired()
               .HasMaxLength(128);

            builder.Property(a => a.LastName)
               .IsRequired()
               .HasMaxLength(128);

            builder.Property(a => a.CardNumber)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(a => a.ExpireMonth)
                .IsRequired();

            builder.Property(a => a.ExpireYear)
                .IsRequired();

            builder.Property(a => a.Cvv)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Shopper)
                .HasForeignKey(x => x.ShopperId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            var shoppers = new[]
            {
                new Shopper
                {
                    Id = 1L,
                    CardNumber = "vHOScgRyhq1RCQCZ2Kb08OaleMrcR8Q42dPae1hX/yc=", // 5105-1051-0510-5100,
                    Cvv = "kFUwtPfKQ3M+LG3idKODhQ==", // 333",
                    FirstName = "John",
                    LastName = "Smith",
                    ExpireYear = 2025,
                    ExpireMonth = 12
                }
            };

            builder.HasData(shoppers);
        }
    }
}
