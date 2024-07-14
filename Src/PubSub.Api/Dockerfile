#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PublisherSubscriberPattern/Src/PubSub.Api/PubSub.Api.csproj", "PublisherSubscriberPattern/Src/PubSub.Api/"]
COPY ["PublisherSubscriberPattern/Src/PubSub.Application/PubSub.Application.csproj", "PublisherSubscriberPattern/Src/PubSub.Application/"]
COPY ["PublisherSubscriberPattern/Src/PubSub.Domain/PubSub.Domain.csproj", "PublisherSubscriberPattern/Src/PubSub.Domain/"]
RUN dotnet restore "./PublisherSubscriberPattern/Src/PubSub.Api/PubSub.Api.csproj"
COPY . .
WORKDIR "/src/PublisherSubscriberPattern/Src/PubSub.Api"
RUN dotnet build "./PubSub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PubSub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PubSub.Api.dll"]