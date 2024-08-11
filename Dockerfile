# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./brave_api/brave_api.csproj" --disable-parallel
RUN dotnet publish "./brave_api/brave_api.csproj" -c release -o /app --no-restore

# SERVE
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT [ "dotnet", "brave_api.dll" ]