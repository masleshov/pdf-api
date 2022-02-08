FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Pdf.Api/Pdf.Api.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY src/Pdf.Api .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

# ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Pdf.Api.dll"]