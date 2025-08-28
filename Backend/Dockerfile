# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Backend.Api/Backend.Api.csproj", "Backend.Api/"]
RUN dotnet restore "Backend.Api/Backend.Api.csproj"

COPY ["src/Backend.Api", "Backend.Api/"]
WORKDIR /src/Backend.Api
RUN dotnet build "Backend.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Backend.Api.csproj" -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.dll"]