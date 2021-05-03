using System.Linq;
using System;

namespace Business
{
    public static class CreditCard
    {
        public static bool Validate(string creditCardNumber)
        {
            // The Luhn algorithm, also known as the modulus 10 or mod 10 algorithm,
            // is a simple checksum formula used to validate a variety of identification numbers,
            // such as credit card numbers, IMEI numbers, Canadian Social Insurance Numbers.
            if (string.IsNullOrEmpty(creditCardNumber))
                return false;

            int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                                              .Reverse()
                                              .Select((e, i) => (e - 48) * (i % 2 == 0 ? 1 : 2))
                                              .Sum((e) => (e / 10) + (e % 10));

            return sumOfDigits % 10 == 0;
        }
    }
}
