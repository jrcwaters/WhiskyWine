#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WhiskyWine.BottleService.API/WhiskyWine.BottleService.API.csproj", "WhiskyWine.BottleService.API/"]
COPY ["WhiskyWine.BottleService.Data/WhiskyWine.BottleService.Data.csproj", "WhiskyWine.BottleService.Data/"]
COPY ["WhiskyWine.BottleService.Domain/WhiskyWine.BottleService.Domain.csproj", "WhiskyWine.BottleService.Domain/"]
RUN dotnet restore "WhiskyWine.BottleService.API/WhiskyWine.BottleService.API.csproj"
COPY . .
WORKDIR "/src/WhiskyWine.BottleService.API"
RUN dotnet build "WhiskyWine.BottleService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WhiskyWine.BottleService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhiskyWine.BottleService.API.dll"]