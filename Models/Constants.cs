using System.Collections.Generic;

namespace Models
{
    public static class Constants
    {
        public const string JwtKey = "Jwt";
        public const string DefaultConnection = "DefaultConnection";

        public static readonly Dictionary<Product, (decimal Price, Currency Currency, long MerchantId)> ProductPrices = new()
        {
            { Product.Book,     (20.0M,  Currency.EUR,  1 )},
            { Product.Table,    (100.0M, Currency.USD,  1 )},
            { Product.Lamp,     (35.0M,  Currency.EUR,  1 )},
            { Product.Monitor,  (350.0M, Currency.GBP,  1 )}
        };
    }
}
