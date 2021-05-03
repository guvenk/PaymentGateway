using Models;
using System;

namespace DataAccess
{
    public class Payment
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public bool IsSuccessful { get; set; }

        public DateTime CreatedDate { get; set; }

        public long MerchantId { get; set; }

        public Merchant Merchant { get; set; }

        public long ShopperId { get; set; }

        public Shopper Shopper { get; set; }
    }
}
