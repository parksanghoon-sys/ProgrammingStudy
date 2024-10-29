```sh
dotnet new sln -o MinimalApiSample

cd MinimalApiSample

dotnet new web -n MinimalApiSample.Api
dotnet add MinimalApiSample.Api package System.Text.Json
dotnet add MinimalApiSample.Api package Swashbuckle.AspNetCore
dotnet add MinimalApiSample.Api package Microsoft.AspNetCore.OpenApi
dotnet add MinimalApiSample.Api package Microsoft.EntityFrameworkCore.Tools
dotnet add MinimalApiSample.Api package Microsoft.EntityFrameworkCore
dotnet sln add ./MinimalApiSample.Api/MinimalApiSample.Api.csproj

dotnet new nunit -o MinimalApiSample.Tests
dotnet add MinimalApiSample.Tests/ reference MinimalApiSample.Api
dotnet add MinimalApiSample.Tests package Moq
dotnet add MinimalApiSample.Tests package RichardSzalay.MockHttp
dotnet add MinimalApiSample.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet sln add ./MinimalApiSample.Tests/MinimalApiSample.Tests.csproj
```


```sh
dotnet new sln -o MinimalApiSample

cd MinimalApiSample

dotnet new web -n MinimalApiSample.Api
dotnet add MinimalApiSample.Api package System.Text.Json
dotnet add MinimalApiSample.Api package Swashbuckle.AspNetCore
dotnet add MinimalApiSample.Api package Microsoft.AspNetCore.OpenApi
dotnet sln add ./MinimalApiSample.Api/MinimalApiSample.Api.csproj

dotnet new nunit -o MinimalApiSample.Tests
dotnet add MinimalApiSample.Tests/ reference MinimalApiSample.Api
dotnet add MinimalApiSample.Tests package Moq
dotnet add MinimalApiSample.Tests package RichardSzalay.MockHttp
dotnet add MinimalApiSample.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet sln add ./MinimalApiSample.Tests/MinimalApiSample.Tests.csproj
```
