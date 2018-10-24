FROM microsoft/dotnet:2.0-runtime-nanoserver-1709 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Monefy.Api.IntegrationTests/Monefy.Api.IntegrationTests.csproj Monefy.Api.IntegrationTests/
COPY Monefy.Api.Common/Monefy.Api.Common.csproj Monefy.Api.Common/
COPY Monefy.Data.Access/Monefy.Data.Access.csproj Monefy.Data.Access/
COPY Monefy.Data.Model/Monefy.Data.Model.csproj Monefy.Data.Model/
COPY MonefyApi/MonefyApi.csproj MonefyApi/
COPY Monefy.Queries/Monefy.Queries.csproj Monefy.Queries/
COPY Monify.Security/Monefy.Security.csproj Monify.Security/
COPY Monefy.Api.Models/Monefy.Api.Models.csproj Monefy.Api.Models/
RUN dotnet restore Monefy.Api.IntegrationTests/Monefy.Api.IntegrationTests.csproj
COPY . .
WORKDIR /src/Monefy.Api.IntegrationTests
RUN dotnet build Monefy.Api.IntegrationTests.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Monefy.Api.IntegrationTests.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monefy.Api.IntegrationTests.dll"]
