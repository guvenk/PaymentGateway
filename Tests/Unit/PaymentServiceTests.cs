using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DataAccess;
using Business;
using Models;
using Xunit;

namespace Tests.Unit
{
    public class PaymentServiceTests
    {
        private readonly AppDbContext dbContext;
        private readonly PaymentService sut;
        private readonly IFixture _fixture;
        public PaymentServiceTests()
        {
            var configuration = BoilerPlate.SetupConfiguration();
            var bankService = BoilerPlate.SetupBankService();
            dbContext = new AppDbContext(DbContextHelper.GetInMemoryOptions());

            sut = new PaymentService(bankService, dbContext, Mock.Of<ILogger<PaymentService>>(), configuration);

            _fixture = BoilerPlate.SetupAutoFixture();
        }

        [Fact]
        public async Task PurchaseProduct_CreateMultiple_Success()
        {
            // Arrange
            var rnd = new Random();
            int totalPayment = 3;
            var dtos = _fixture.Build<PurchaseRequestDto>()
                .With(x => x.ExpireMonth, rnd.Next(1, 13))
                .With(x => x.ExpireYear, rnd.Next(2022, 2030))
                .With(x => x.Cvv, TestConstants.TestCvv)
                .With(x => x.CardNumber, "5105-1051-0510-5100")
                .CreateMany(totalPayment);

            // Act
            foreach (var dto in dtos)
                await sut.PurchaseProductAsync(dto);

            // Assert
            Assert.Equal(totalPayment, dbContext.Payments.Count());
        }

        [Fact]
        public async Task GetPayment_WithEncryption_Success()
        {
            // Arrange
            var shopper = new Shopper
            {
                Id = 1,
                CardNumber = Encryption.Encrypt(TestConstants.TestCard, TestConstants.EncryptionKey),
                Cvv = Encryption.Encrypt(TestConstants.TestCvv, TestConstants.EncryptionKey),
                FirstName = "TestName",
                LastName = "TestLastName",
                ExpireMonth = 3,
                ExpireYear = 2025
            };

            var paymentDb = new Payment()
            {
                Id = Guid.NewGuid(),
                Amount = 123.5M,
                CreatedDate = DateTime.UtcNow,
                Currency = Currency.EUR,
                PaymentStatus = PaymentStatus.Successful,
                Merchant = new Merchant { Id = 1, Name = "Test Merchant" },
                Shopper = shopper
            };

            dbContext.Payments.Add(paymentDb);
            await dbContext.SaveChangesAsync();

            // Act
            var payment = await sut.GetPaymentAsync(paymentDb.Id);

            // Assert
            Assert.NotNull(payment);
            Assert.Equal(shopper.FirstName, payment.FirstName);
            Assert.Equal(shopper.LastName, payment.LastName);
            Assert.Equal(CreditCard.GetMasked(TestConstants.TestCard), payment.CardNumber);
            Assert.Equal(TestConstants.TestCvv, payment.Cvv);
            Assert.Equal(shopper.ExpireYear, payment.ExpireYear);
            Assert.Equal(shopper.ExpireMonth, payment.ExpireMonth);
            Assert.Equal(PaymentStatus.Successful, payment.PaymentStatus);
        }
    }
}
