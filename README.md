# tile-server
[![CI](https://github.com/philipp-meier/tile-server/actions/workflows/dotnet.yml/badge.svg)](https://github.com/philipp-meier/tile-server/actions/workflows/dotnet.yml)
[![MIT License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/philipp-meier/tile-server/blob/main/LICENSE)  
Simple tile server for `.mbtiles`-files written in C# with minimal APIs.

## Setup
1. Place your .mbtiles file in the `./data` directory (`mkdir data`) and rename it to `map_data.mbtiles`.
2. Generate a (trusted) https certificate and place it in the `https` folder:  
`dotnet dev-certs https -ep ./https/aspnetapp.pfx -p <password>`  
`dotnet dev-certs https --trust`  
3. Enter your chosen https certificate password in the `docker-compose.yml` environment variable.
4. Build the tile server with `docker compose build`.
5. Run the server with `docker compose up`.
6. Open the `./test/index.html` to see a leaflet-demo.
7. _[Optional]_ Enter the container with `docker exec -it tile-server-app-1 bash`.

## Stress testing
Install [k6](https://k6.io/docs/get-started/installation/) by Grafana Labs and run the following command: `k6 run test/k6.js`

## Operations

[dotnet-counters](https://learn.microsoft.com/en/dotnet/core/diagnostics/dotnet-counters) and [dotnet-dump](https://learn.microsoft.com/en/dotnet/core/diagnostics/dotnet-dump) are available in the `/tools`-folder of the tile-server. `/tools/dotnet-counters monitor -p 1` can provide a high-level overview of the applications health (CPU usage, GC- and thread pool information,...).

`/tools/dotnet-dump collect -p 1` can be used to create (and also analyze) dumps.

## Used mbtiles file
https://ftp.gwdg.de/pub/misc/openstreetmap/openandromaps/world/OAM-World-1-8-min-J80.mbtiles
