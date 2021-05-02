using System.Collections.Generic;

namespace DataAccess
{
    public class Merchant : EntityBase
    {
        public string Name { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}