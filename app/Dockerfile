FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "7071Group/7071Group.csproj"
RUN dotnet publish "7071Group/7071Group.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create directory for HTTPS certificates
RUN mkdir -p /https

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "7071Group.dll"]
