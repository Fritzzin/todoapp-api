# todoapp-api
API built to learn how to make APIs in dotnet.

## User Secrets
Create a `secrets.json` file to pass the `connectionString`. It should have the following structure:
``` json
{
    "ConnectionStrings": {
        "DefaultConnection": "YourConnectionString"
    }
}
```

Then add the following line with the Guid of the created folder with the secret inside to the `.csproj` file in TodoApp.Api:
``` csproj
 <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
        <UserSecretsId> GUID IN HERE </UserSecretsId>
    </PropertyGroup>
```
