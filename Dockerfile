FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY TestAppSmartWay.Application/*.csproj TestAppSmartWay.Application/
COPY TestAppSmartWay.Domain/*.csproj TestAppSmartWay.Domain/
COPY TestAppSmartWay.Infrastructure/*.csproj TestAppSmartWay.Infrastructure/
COPY TestAppSmartWay.IntegrationTests/*.csproj TestAppSmartWay.IntegrationTests/
COPY TestAppSmartWay.WebApi/*.csproj TestAppSmartWay.WebApi/
WORKDIR TestAppSmartWay.WebApi
RUN dotnet restore
COPY . .
RUN dotnet publish TestAppSmartWay.WebApi/TestAppSmartWay.WebApi.csproj -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_ENVIRONMENT Docker
EXPOSE 80
ENTRYPOINT ["dotnet", "TestAppSmartWay.WebApi.dll"]