#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Yelp.Api.Web/Yelp.Api.Web.csproj", "Yelp.Api.Web/"]
COPY ["Yelp.Api.Domain/Yelp.Api.Domain.csproj", "Yelp.Api.Domain/"]
RUN dotnet restore "Yelp.Api.Web/Yelp.Api.Web.csproj"
COPY . .
WORKDIR "/src/Yelp.Api.Web"
RUN dotnet build "Yelp.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yelp.Api.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yelp.Api.Web.dll"]