#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["tutorial-api-2.csproj", "."]
RUN dotnet restore "./tutorial-api-2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "tutorial-api-2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tutorial-api-2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tutorial-api-2.dll"]