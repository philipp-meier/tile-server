using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

var mbtilesPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "map_data.mbtiles");
if (!File.Exists(mbtilesPath))
    throw new FileNotFoundException(mbtilesPath);

var connStr = new SqliteConnectionStringBuilder {
    Mode = SqliteOpenMode.ReadOnly,
    DataSource = mbtilesPath
};
builder.Services.AddScoped(_ => new SqliteConnection(connStr.ToString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/{z:int}/{x:int}/{y:int}.png", async (HttpContext context, SqliteConnection db, int z, int x, int y) => {
    context.Response.Headers.Add("Cache-Control", "max-age=58362, stale-while-revalidate=604800, stale-if-error=604800");

    await db.OpenAsync();
    using (var command = db.CreateCommand()) {
        command.CommandText =
        @"
            SELECT tile_data
            FROM tiles
            WHERE zoom_level = $z AND tile_column = $x AND tile_row = $y
        ";
        command.Parameters.AddWithValue("$z", z);
        command.Parameters.AddWithValue("$x", x);
        command.Parameters.AddWithValue("$y", y);
        
        using (var reader = await command.ExecuteReaderAsync()) {
            if (await reader.ReadAsync() && reader["tile_data"] is byte[] tileData)
                return Results.File(tileData, contentType: "image/png");
            else 
                return Results.NoContent();
        }
    }
});

app.UseHttpsRedirection();
app.Run();
