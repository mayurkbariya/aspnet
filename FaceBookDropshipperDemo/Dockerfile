FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
ARG BuildNumber
ENV ASPNETCORE_URLS=http://+:5000
ENV BuildNumber = $BuildNumber

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/FBDropshipper.WebApi/FBDropshipper.WebApi.csproj"
WORKDIR "/src/FBDropshipper.WebApi"
RUN dotnet build "FBDropshipper.WebApi.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/FBDropshipper.WebApi"
RUN dotnet publish "FBDropshipper.WebApi.csproj" -c Release -o /app

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FBDropshipper.WebApi.dll", "--server.urls", "http://0.0.0.0:5000"]