## Payment Gateway 

A sample project for a payment gateway


## Installation

- Project requires a SQL or SQLEXPRESS instance to be available. Connection string in appsettings.json should be adjusted accordingly.
- Please use Update-Database command on Package Manager Console to create the database.
- .Net Core 5.0 SDK must be installed.
- Please run "dotnet tool install -g swashbuckle.aspnetcore.cli" on Command Prompt for API client generation to work


## Libraries and Frameworks
- .Net Core 5.0
- EF Core 5.0
- Automapper 10.1.1 is utilized to map dto classes to entities and vice versa for user to consume the api.
- Swashbuckle.AspNetCore 5.6.3 to have auto generated documentation of the API
- Xunit 2.4.1 testing library.
- Microsoft.EntityFrameworkCore.InMemory 5.0.5 this is utilized for testing purposes on test layer.
- Moq 4.16.1 this one is installed but not utilized. it would be utilized for upcoming tests for their dependency injected services to be mocked.


## Usage

- Build the solution with Visual Studio or 'dotnet build' command with dotnet CLI.using 
- Run it using either Visual Studio or 'dotnet run' command with dotnet CLI in project's main directory.
- Need to use JWT to be authenticated to be able to access resources in the API.
- You can generate & use a token using [GET]auth/token endpoint.

## API

- Brotli and GZIP Compressions are available for responses. To utilize add Accept-Encoding header with either "gzip" or "br" as values.

- Authentication is used along with JWT generation and validation using key,issuer,audience and lifetime.
- Authorization is utilized with default authorization configuration. Didn't include custom policy implementation for simplicity.


## Encryption & Decryption

Module is the one doing the encryption and the one which needs to decrypt it. Therefore I have utilized "Symmetric Encryption" strategy.

- Credit card number and Cvv fields are used for encryption before saving to db and for retrieving back.



## Testing Project
- Xunit is used as testing library along with it's runner.
- AAA pattern is utilized for testing convenstions
- Naming convension for tests is UnitOfWork_StateUnderTest_ExpectedBehavior