# todoapp-api

API built to learn how to make APIs in dotnet.

## User Secrets

Create a `secrets.json` file to pass a `connectionString` and parameters for the JWT token. It should follow this
structure:

``` json
{
    "ConnectionStrings": {
        "DefaultConnection": "YourConnectionString"
    }
    "JwtSettings": {
        "SecretKey": "secretKey123",
        "Issuer": "TodoApp.Api",
        "Audience": "TodoApp"
    }
}
```

Then add the following line with the Guid of the created folder with the secret inside to the `.csproj` file in
TodoApp.Api:

``` csproj
 <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
        <UserSecretsId> GUID IN HERE </UserSecretsId>
</PropertyGroup>
```
