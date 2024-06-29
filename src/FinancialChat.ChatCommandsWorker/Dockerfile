#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinancialChat.ChatCommandsWorker/FinancialChat.ChatCommandsWorker.csproj", "FinancialChat.ChatCommandsWorker/"]
COPY ["FinancialChat.Infra/FinancialChat.Infra.csproj", "FinancialChat.Infra/"]
COPY ["FinancialChat.Application/FinancialChat.Application.csproj", "FinancialChat.Application/"]
RUN dotnet restore "./FinancialChat.ChatCommandsWorker/FinancialChat.ChatCommandsWorker.csproj"
COPY . .
WORKDIR "/src/FinancialChat.ChatCommandsWorker"
RUN dotnet build "./FinancialChat.ChatCommandsWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinancialChat.ChatCommandsWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialChat.ChatCommandsWorker.dll"]