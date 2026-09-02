FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish "GameLibrary.csproj" -c Release -o /app
RUN dotnet publish "GameLibraryFrontend.csproj" -c Release -o /app/frontend

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
COPY --from=build /app .
COPY --from=build /app/frontend/wwwroot /app/wwwroot
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "GameLibrary.dll"]