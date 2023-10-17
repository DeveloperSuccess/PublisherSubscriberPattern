#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PublisherSubscriberPattern/PublisherSubscriberPattern.csproj", "PublisherSubscriberPattern/"]
RUN dotnet restore "PublisherSubscriberPattern/PublisherSubscriberPattern.csproj"
COPY . .
WORKDIR "/src/PublisherSubscriberPattern"
RUN dotnet build "PublisherSubscriberPattern.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublisherSubscriberPattern.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublisherSubscriberPattern.dll"]