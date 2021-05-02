using System.Collections.Generic;

namespace DataAccess
{
    public class Merchant
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}