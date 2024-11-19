### 솔루션 만들기

dotnet new sln -o MinimalApiSample

cd MinimalApiSample

## API 서버 만들기

dotnet new web -n MinimalApiSample.Api
dotnet add package System.Text.Json
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore
dotnet add MinimalApiSample.Api package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add MinimalApiSample.Api package NMySql.EntityFrameworkCore

dotnet add package Aspire.Npgsql

dotnet sln add ./MinimalApiSample.Api/MinimalApiSample.Api.csproj

## UnitTest 프로젝트

dotnet new nunit -o MinimalApiSample.Tests
dotnet add MinimalApiSample.Tests/ reference MinimalApiSample.Api
dotnet add MinimalApiSample.Tests package Moq
dotnet add MinimalApiSample.Tests package RichardSzalay.MockHttp
dotnet add MinimalApiSample.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet sln add ./MinimalApiSample.Tests/MinimalApiSample.Tests.csproj

## Aspire 프로젝트

dotnet new aspire-apphost -n "BicycleSharingSystem.AppHost" -o AppHost
dotnet new aspire-servicedefaults -n "BicycleSharingSystem.ServiceDefaults" -o ServiceDefaults

dotnet add BicycleSharingSystem.AppHost reference WebApi/BicycleSharingSystem.WebApi.csproj

dotnet add BicycleSharingSystem.AppHost package Aspire.Hosting.MySql
dotnet add BicycleSharingSystem.AppHost package Aspire.Hosting.PostgreSQL
dotnet add package Aspire.Npgsql.EntityFrameworkCore.PostgreSQL

## 닷넷 최신버젼

<LangVersion>latest</LangVersion>
