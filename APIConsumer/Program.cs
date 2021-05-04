using PaymentAPI;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIConsumer
{
    class Program
    {
        private static readonly HttpClient _httpclient = new();
        static async Task Main()
        {
            Console.WriteLine("Payment Gateway API Consumption...");

            var paymentClient = new PaymentAPIClient("https://localhost:5001", _httpclient);

            string token = await paymentClient.GetTokenAsync();

            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer", string.Empty));
            var payment = await paymentClient.GetPaymentAsync(Guid.Parse("0A036FBA-2BBF-4530-A90C-C0D07C3FD23A"));

            Console.WriteLine($"Payment Details:");
            Console.WriteLine($"Fullname: {payment.FirstName + payment.LastName}");
            Console.WriteLine($"CardNumber: {payment.CardNumber}");
            Console.WriteLine($"Expiry: {payment.ExpireMonth}/{payment.ExpireYear} Cvv: {payment.Cvv}");
            Console.WriteLine($"Payment Status: {payment.PaymentStatus}");

            var body = new PurchaseRequestDto { CardNumber = "4012-8888-8888-1881", FirstName = "Sarah", LastName = "Connor", Cvv = "678", ExpireMonth = 5, ExpireYear = 2090, Product = Product._3 };
            var purchaseResponse = await paymentClient.PurchaseProductAsync(body);

            Console.WriteLine($"Purchase response: Id: {purchaseResponse.Id} StatusCode: {purchaseResponse.PaymentStatus}");
        }
    }
}
