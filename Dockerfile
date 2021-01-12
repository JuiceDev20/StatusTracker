#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your ~/assets/Images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app




FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StatusTracker.csproj", ""]
RUN dotnet restore "./StatusTracker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StatusTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatusTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet StatusTracker.dll