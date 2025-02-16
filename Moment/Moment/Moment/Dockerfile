# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore as distinct layers
COPY *.sln .
COPY Moment/*.csproj ./Moment/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/Moment
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Use a non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "Moment.dll"]
