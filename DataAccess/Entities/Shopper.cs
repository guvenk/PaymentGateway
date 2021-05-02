using System.Collections.Generic;

namespace DataAccess
{
    public class Shopper
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CardNumber { get; set; }

        public int ExpireMonth { get; set; }

        public int ExpireYear { get; set; }

        public int Cvv { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
