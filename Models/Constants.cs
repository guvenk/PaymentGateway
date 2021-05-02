using System;
using System.Collections.Generic;

namespace Models
{
    public static class Constants
    {
        public const string JwtKey = "Jwt";
        public const string DefaultConnection = "DefaultConnection";

        public static readonly Dictionary<Product, decimal> ProductPrices = new()
        {
            { Product.Book, 20.0M },
            { Product.Table, 100.0M },
            { Product.Lamp, 35.0M },
            { Product.Monitor, 350.0M }
        };

    }
}
