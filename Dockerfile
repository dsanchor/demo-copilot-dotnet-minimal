# Use the official .NET 7 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory to /app
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET 7 runtime image as the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set the working directory to /app
WORKDIR /app

# Copy the published output from the build environment to the runtime environment
COPY --from=build-env /app/out ./

# Start the application
ENTRYPOINT ["dotnet", "WebApi.dll"]