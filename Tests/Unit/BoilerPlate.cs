using AutoFixture;
using AutoFixture.AutoMoq;
using Business;
using Microsoft.Extensions.Configuration;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Unit
{
    public class BoilerPlate
    {
        public static IFixture SetupAutoFixture()
        {
            // This is to mitigation of a self loop reference error while building mock objects.
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
        public static IConfiguration SetupConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> { { nameof(TestConstants.EncryptionKey), TestConstants.EncryptionKey } };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return configuration;
        }

        public static IBankService SetupBankService()
        {
            var mockBankService = new Mock<IBankService>();
            mockBankService.SetupSequence(x => x.ProcessPaymentAsync(It.IsAny<PurchaseRequestDto>()))
                .ReturnsAsync(new PurchaseResultDto(Guid.NewGuid(), PaymentStatus.Successful))
                .ReturnsAsync(new PurchaseResultDto(Guid.NewGuid(), PaymentStatus.Successful))
                .ReturnsAsync(new PurchaseResultDto(Guid.NewGuid(), PaymentStatus.Successful));

            return mockBankService.Object;
        }
    }
}
