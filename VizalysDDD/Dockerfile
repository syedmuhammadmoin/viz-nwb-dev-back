# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file
COPY *.sln .

# Copy the project files
COPY Application/*.csproj ./Application/
COPY Application.Contracts/*.csproj ./Application.Contracts/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY VizalysDDD/*.csproj ./VizalysDDD/
COPY ApplicationTests/*.csproj ./ApplicationTests/

# Restore dependencies
RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
RUN dotnet publish VizalysDDD -c Release -o out

# Use a lightweight runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /app/out .

# Expose the ports your app runs on
EXPOSE 5000
EXPOSE 5001

# Command to run the application
ENTRYPOINT ["dotnet", "VizalysDDD.dll"]

