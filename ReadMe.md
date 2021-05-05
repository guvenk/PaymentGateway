## Payment Gateway 

A sample project for a payment gateway

## Installation

- Project requires a SQL or SQLEXPRESS instance to be available. Connection string in appsettings.json should be adjusted accordingly.
- Please use Update-Database command on Package Manager Console or update db through dotnet cli to create the database.
- .Net Core 5.0 SDK must be installed.
- Please run "dotnet tool install -g swashbuckle.aspnetcore.cli" on Command Prompt for API client generation to work

## General Ideas & Assumptions

- I've hardcoded the products and their prices as a dictionary for simplicity. Didn't create a db table because they could be fetched from merchants in real life scenario.
- Used simplistic auth mechanism without custom policies. Implemented JWT generation and validation using key,issuer,audience and lifetime.
- After building API project, APIClient project needs to be build so that API client is generated automatically on build. Flow:  make a visible change on API endpoints > build API > build APIClient > Eh Voilà! the change will be visible on APIClient codebase
- Azure ci/cd pipelines and k8s-deployment is used for showcasing, they would work when connected to an infrastructure
- Project can be run with Kestrel, IIS and Docker.
- Logging is only to a file in C:\Logs\ directory, could be sent to elastic search or other log storage providers.
- Metrics are collected using Prometheus libarary. I've created a middleware that collects the metrics.

## Usage

- Once the necessary tools are installed project can be built and run.
- Need to use JWT to be authenticated to be able to access resources in the API.
- You can generate & use a token using [GET]auth/token endpoint (Allows anonymous access).

## API

- I have hardcoded the products and their prices as a dictionary for simplicity. They could be fetched from merchants as well.
- Brotli and GZIP response compressions are available. To utilize it, use Accept-Encoding header with either "gzip" or "br" as value.

## Database 

- SQL Server along with EF Core 5.0 is used.
- There are three tables: Merchant, Payment and Shopper. Payment is child entity of other two.
- Relationships: Merchant -- 1-n -- Payment  AND  Shopper -- 1-n -- Payment
- There is a seed data for each table added with a migration.

## Encryption & Decryption

- Module is the one doing the encryption and the one which needs to decrypt it. Therefore I have utilized "Symmetric Encryption" strategy.
- Credit card number and Cvv fields are used for encryption before saving to db and for retrieving back.

## Testing Project
- Xunit is used as testing library along with it's runner.
- AAA pattern is utilized for testing convenstions
- In memory database is used for all tests, for integration tests there is seed data since it builds up an artificial host in the background.
- For unit tests database is initialized as empty.
- Naming convension for tests is UnitOfWork_StateUnderTest_ExpectedBehavior
- Autofixture is used to generate multiple randomized objects.
- Moq is used for mocking some services.
- For integration tests [WebApplicationFactory](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0) is used. It is from Microsoft documentation
- MockBankService is used to mock bank service responses in integration tests.
- Integration tests needs to be authenticated to be able to access endpoints in the API, there fore a token generation step is implemented during creation of TestFactory.