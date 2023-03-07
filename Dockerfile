FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["src/ShortUrl.Api/ShortUrl.Api.csproj", "ShortUrl.Api/"]
COPY ["src/ShortUrl.Infrastructure/ShortUrl.Infrastructure.csproj", "ShortUrl.Infrastructure/"]
COPY ["src/ShortUrl.Application/ShortUrl.Application.csproj", "ShortUrl.Application/"]
COPY ["src/ShortUrl.Domain/ShortUrl.Domain.csproj", "ShortUrl.Domain/"]
RUN dotnet restore "ShortUrl.Api/ShortUrl.Api.csproj"
COPY . .
WORKDIR "src/ShortUrl.Api"
RUN dotnet build "ShortUrl.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShortUrl.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShortUrl.Api.dll"]
