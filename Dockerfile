# prepare hosting image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS hosting
WORKDIR /app
EXPOSE 80
EXPOSE 443

# prepare ops tooling
FROM hosting AS hosting-tools
RUN apt update && apt install -y curl && mkdir /tools && \
    curl -L https://aka.ms/dotnet-counters/linux-x64 -o /tools/dotnet-counters && \
    curl -L https://aka.ms/dotnet-dump/linux-x64 -o /tools/dotnet-dump && \
    chmod +x /tools/dotnet-counters /tools/dotnet-dump

# build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY src/. .
RUN dotnet publish -c release -o /app/publish

# final stage/image
FROM hosting-tools AS server
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "tile-server.dll"]