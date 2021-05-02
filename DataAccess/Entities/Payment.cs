using System;

namespace DataAccess
{
    public class Payment : EntityBase
    {
        public string CardNumber { get; set; }

        public int ExpireMonth { get; set; }

        public int ExpireYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int Cvv { get; set; }

        public bool IsSuccessful { get; set; }

        public DateTime CreateDate { get; set; }

        public long MerchantId { get; set; }

        public Merchant Merchant { get; set; }
    }
}
