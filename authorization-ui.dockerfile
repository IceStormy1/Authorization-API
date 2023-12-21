#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish "Authorization.UI/Authorization.UI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y libgdiplus

ENTRYPOINT ["dotnet", "Authorization.UI.dll"]