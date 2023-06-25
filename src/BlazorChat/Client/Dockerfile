FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY "BlazorChat.Client.csproj" .
RUN dotnet restore "BlazorChat.Client.csproj"
COPY . .
RUN dotnet build BlazorChat.Client.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorChat.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf