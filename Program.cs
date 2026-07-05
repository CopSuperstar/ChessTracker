using Microsoft.Data.Sqlite;
using System.Text.Json;
using MyChess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ChessTracker/1.0");
var connectionString = "Data Source=chess.db";

Database.Initialize(connectionString);

app.MapGet("/api/games/fetch", async (string username) =>
{
    var url = $"https://api.chess.com/pub/player/{username}/games/2026/05";
    var json = await httpClient.GetStringAsync(url);
    
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var response = JsonSerializer.Deserialize<ChessComResponse>(json, options);
    if(response == null){return Results.BadRequest("Failed to deserialize, response was empty.");}
    int counter = 0;
    foreach(ChessComGame game in response.Games)
    {
        GameReport report = Converter.ConvertReport(game);
        if(game.Rated){
            counter += Database.InsertGame(report, connection);
            }
    }
    return Results.Ok(counter);
}
);
app.MapGet("/api/games/openings", async  (string username) =>{
    var sorting = await Database.SortingOpenings(connectionString, username);
    return sorting;
});

app.Run();
