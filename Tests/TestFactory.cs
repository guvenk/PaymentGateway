using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Tests
{
    public class TestFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public AppDbContext DbContext { get; set; }

        public IConfiguration Configuration { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's AppDbContext registration.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>((options, context) =>
                {
                    context.UseInMemoryDatabase("InMemoryDb");
                });

                var sp = services.BuildServiceProvider();
                var scopedServices = sp.CreateScope().ServiceProvider;
                Configuration = scopedServices.GetRequiredService<IConfiguration>();
                DbContext = scopedServices.GetRequiredService<AppDbContext>();
                DbContext.Database.EnsureCreated();
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            string token = GenerateJwtToken();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public string GenerateJwtToken()
        {
            var config = Configuration.GetSection(Constants.JwtKey).Get<JwtConfig>();

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(config.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, Constants.TestUser),
                }),
                Issuer = config.Issuer,
                Audience = config.Audience,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            string result = jwtTokenHandler.WriteToken(token);

            return result;
        }
    }
}
