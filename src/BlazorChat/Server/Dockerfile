#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Server/BlazorChat.Server.csproj", "Server/"]
COPY ["Client/BlazorChat.Client.csproj", "Client/"]
COPY ["Shared/BlazorChat.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/BlazorChat.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "BlazorChat.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorChat.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorChat.Server.dll"]