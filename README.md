# tile-server
[![CI](https://github.com/philipp-meier/tile-server/actions/workflows/dotnet.yml/badge.svg)](https://github.com/philipp-meier/tile-server/actions/workflows/dotnet.yml)
[![MIT License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/philipp-meier/tile-server/blob/main/LICENSE)  
Simple tile server for `.mbtiles`-files written in C# with minimal APIs.

## Quickstart
#### Development environment
Run `dotnet run` in the `src` folder to start the application.

#### Standalone (Release)
Download the latest standalone.zip from the [release page](https://github.com/philipp-meier/tile-server/releases), place your `.mbtiles`-file in the "data"-folder, rename it to `map_data.mbtiles` and start the .NET application. The tiles are available at `https://localhost:5001/{z}/{x}/{y}.png`.

## Docker setup
1. Place your .mbtiles file in the `./data` directory (`mkdir data`) and rename it to `map_data.mbtiles`.
2. Generate a (trusted) https certificate and place it in the `https` folder:  
`dotnet dev-certs https -ep ./https/aspnetapp.pfx -p <password>`  
`dotnet dev-certs https --trust`  
3. Enter your chosen https certificate password in the `docker-compose.yml` environment variable.
4. Build the tile server with `docker compose build`.
5. Run the server with `docker compose up`.
6. Open the `./test/index.html` to see a leaflet-demo.
7. _[Optional]_ Enter the container with `docker exec -it tile-server-app-1 bash`.

## Operations
[dotnet-counters](https://learn.microsoft.com/en/dotnet/core/diagnostics/dotnet-counters) and [dotnet-dump](https://learn.microsoft.com/en/dotnet/core/diagnostics/dotnet-dump) are available in the `/tools`-folder of the tile-server. `/tools/dotnet-counters monitor -p 1` can provide a high-level overview of the applications health (CPU usage, GC- and thread pool information,...).

`/tools/dotnet-dump collect -p 1` can be used to create (and also analyze) dumps.

## Stress testing
Install [k6](https://k6.io/docs/get-started/installation/) by Grafana Labs and run the following command: `k6 run test/k6.js`

## Statistics
Statistics from a local [bombardier](https://github.com/codesenberg/bombardier) benchmark:
```
./bombardier https://localhost/5/15/19.png -d 120s --insecure

Bombarding https://localhost:443/5/15/19.png for 2m0s using 125 connection(s)
Done!
Statistics        Avg      Stdev        Max
  Reqs/sec     36734.64    4022.55   47849.41
  Latency        3.40ms     1.08ms   122.90ms
  HTTP codes:
    1xx - 0, 2xx - 4407445, 3xx - 0, 4xx - 0, 5xx - 0
    others - 0
  Throughput:   716.23MB/s
```

## Used mbtiles file
https://ftp.gwdg.de/pub/misc/openstreetmap/openandromaps/world/OAM-World-1-8-min-J80.mbtiles
