# 1. Derleme Aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyalarını ve bağımlılıkları kopyala
COPY *.sln .
COPY Moment/*.csproj ./Moment/
RUN dotnet restore

# Tüm dosyaları kopyala ve yayınla
COPY . .
RUN dotnet publish -c Release -o out

# 2. Çalışma Zamanı Aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5000

# Port Açıklaması
EXPOSE 5000

# Uygulamayı çalıştır
ENTRYPOINT ["dotnet", "Moment.dll"]