using Microsoft.Extensions.DependencyInjection;
using PaymentAPI;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APIConsumer
{
    public static class Configuration
    {
        public static IServiceCollection RegisterHttpClient(this IServiceCollection services, string baseUrl, string jwtToken)
        {
            // usage
            //services.RegisterHttpClient("https://localhost:5001/", "jwtTokenString");
            // then inject IPaymentAPIClient into your services

            services.AddHttpClient<IPaymentAPIClient, PaymentAPIClient>(client =>
            {
                client.Configure(baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Baerer", jwtToken);
            })
            .ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }

        private static void Configure(this HttpClient client, string baseUrl)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
    }
}
