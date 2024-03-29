using System.Data;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var mbtilesPath = builder.Configuration["TileServer:MbTilesPath"];
if (!File.Exists(mbtilesPath))
    throw new FileNotFoundException($"Required .mbtiles-file \"{mbtilesPath}\" not found.");

var connStr = $"Data Source={mbtilesPath};Mode=ReadOnly";
builder.Services.AddTransient(_ => new SqliteConnection(connStr));

var app = builder.Build();

const string tileDataQuery = "SELECT tile_data FROM tiles WHERE zoom_level = $z AND tile_column = $x AND tile_row = $y LIMIT 1";
app.MapGet("/{z:int}/{x:int}/{y:int}.png", async (HttpContext context, SqliteConnection db, int z, int x, int y) =>
{
    context.Response.Headers.Add("Cache-Control", "max-age=58362, stale-while-revalidate=604800, stale-if-error=604800");

    await db.OpenAsync();
    await using var command = db.CreateCommand();

    command.CommandText = tileDataQuery;
    command.Parameters.AddWithValue("$z", z);
    command.Parameters.AddWithValue("$x", x);
    command.Parameters.AddWithValue("$y", y);

    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
    if (await reader.ReadAsync())
        return Results.File(await reader.GetFieldValueAsync<byte[]>(0), contentType: "image/png");

    return Results.NoContent();
});

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.Run();
