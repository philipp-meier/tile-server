FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/tile-server.csproj", "./"]
RUN dotnet restore "tile-server.csproj"
COPY ./src .
WORKDIR "/src/"
RUN dotnet build "tile-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tile-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tile-server.dll"]