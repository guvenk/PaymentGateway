using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

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
                .HasMaxLength(30);

            builder.Property(a => a.ExpireMonth)
                .IsRequired();

            builder.Property(a => a.ExpireYear)
                .IsRequired();

            builder.Property(a => a.Cvv)
                .IsRequired();

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
                    CardNumber = "3333-4444-5555-6666",
                    FirstName = "John",
                    LastName = "Smith",
                    ExpireYear = 2025,
                    ExpireMonth = 12,
                    Cvv = 333,
                    Payments = new List<Payment>{
                        new Payment
                        {
                            Id = Guid.Parse("0A036FBA-2BBF-4530-A90C-C0D07C3FD23A"),
                            Amount = 1000.000M,
                            CreatedDate = DateTime.UtcNow,
                            Currency = "EUR",
                            IsSuccessful = true,
                            Merchant = new Merchant{
                                Id = 1L,
                                Name = "Amazon",
                            }
                        }
                    }
                }
            };

            builder.HasData(shoppers);
        }
    }
}
