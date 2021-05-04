using Business;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Models;
using PaymentGateway;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Tests.Integration
{
    public class PaymentControllerTests : IClassFixture<TestFactory<Startup>>
    {
        private readonly TestFactory<Startup> _factory;
        private const string basePath = "payments";

        public PaymentControllerTests(TestFactory<Startup> factory) => _factory = factory;


        [Fact]
        public async void Get_PaymentWithId_Success()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            string paymentId = _factory.DbContext.Payments.Single().Id.ToString();
            Uri uri = new($"{basePath}?PaymentId={paymentId}", UriKind.Relative);

            // Act
            using var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode();
            var payments = response.ParseAs<PaymentResponseDto>();

            string maskedCardNumber = CreditCard.GetMasked("5105-1051-0510-5100");
            Assert.Equal(maskedCardNumber, payments.CardNumber);
            Assert.Equal("333", payments.Cvv);
            Assert.Equal("John", payments.FirstName);
            Assert.Equal("Smith", payments.LastName);
            Assert.Equal(2025, payments.ExpireYear);
            Assert.Equal(12, payments.ExpireMonth);
        }


        [Fact]
        public async void Post_PurchaseProduct_Success()
        {
            // Arrange
            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IBankService, MockBankService>();
                });
            }).CreateClient();

            Uri uri = new(basePath, UriKind.Relative);

            var input = new PurchaseRequestDto(Product.Monitor, "Sarah", "Connor", "4208-0400-5852-3273", 11, 2026, "333");

            var stringContent = new StringContent(
                JsonSerializer.Serialize(input),
                Encoding.UTF8, "application/json");

            // Act
            using var response = await client.PostAsync("payments", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var payment = response.ParseAs<PurchaseResultDto>();

            Assert.Equal(TestConstants.TestPaymentId, payment.Id);
            Assert.Equal(PaymentStatus.Successful, payment.PaymentStatus);
        }
    }
}
