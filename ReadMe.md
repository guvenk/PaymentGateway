## Payment Gateway 

A sample project for a payment gateway


## Installation

- Project requires a SQL or SQLEXPRESS instance to be available. Connection string in appsettings.json should be adjusted accordingly.
- Please use Update-Database command on Package Manager Console to create the database.
- .Net Core 5.0 SDK must be installed.
- Please run "dotnet tool install -g swashbuckle.aspnetcore.cli" on Command Prompt for API client generation to work


## General Ideas

- I have hardcoded the products and their prices as a dictionary for simplicity. They could be fetched from merchants as well.
- Logging is only to a file in C:\Logs\ directory, could be sent to elastic search or other log storage providers.
- Very simplistic auth mechanism used for showcasing them.
- Azure ci/cd pipelines and k8s-deployment is used for showcasing, they would work when connected to an infrastructure
- Project can be run with Kestrel, IIS and Docker as well.
## Usage

- Build the solution with Visual Studio or 'dotnet build' command with dotnet CLI.using 
- Run it using either Visual Studio or 'dotnet run' command with dotnet CLI in project's main directory.
- Need to use JWT to be authenticated to be able to access resources in the API.
- You can generate & use a token using [GET]auth/token endpoint.

## API

- I have hardcoded the products and their prices as a dictionary for simplicity. They could be fetched from merchants as well.

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