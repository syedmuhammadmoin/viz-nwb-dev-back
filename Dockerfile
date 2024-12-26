# Use the .NET 6 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY Application/*.csproj ./Application/
COPY Application.Contracts/*.csproj ./Application.Contracts/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY VizalysDDD/*.csproj ./VizalysDDD/

# Restore dependencies
RUN dotnet restore

# Copy all source files and publish the app
COPY . .
RUN dotnet publish VizalysDDD/Vizalys.Api.csproj -c Release -o out

# Use the .NET 6 Runtime to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out .

# Expose the ports used by the app
EXPOSE 5000
EXPOSE 5001

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Vizalys.Api.dll"]

