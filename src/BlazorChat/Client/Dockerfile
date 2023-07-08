FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build-env
WORKDIR /app


# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "BlazorChat.Client.csproj"
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview
WORKDIR /app

COPY --chmod=755 --from=build-env /app/out .

FROM nginx:alpine AS final
WORKDIR /app

EXPOSE 8080
COPY --chmod=755 /nginx.conf /etc/nginx/nginx.conf
COPY --chmod=755  --from=build-env /app/out/wwwroot /usr/share/nginx/html
 
