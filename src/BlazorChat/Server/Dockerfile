FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "BlazorChat.Server.csproj"
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BlazorChat.Server.dll"]
EXPOSE 8080
EXPOSE 8081
EXPOSE 5000