using Microsoft.Data.Sqlite;
using MyChess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{ app.MapOpenApi(); }

app.UseHttpsRedirection();
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ChessTracker/1.0");
var chessComClient = new ChessComClient(httpClient);
var connectionString = "Data Source=chess.db";

Database.Initialize(connectionString);

app.MapGet("/api/games/fetch", async (string username) =>
{
    int counter = 0;
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    List<ChessComGame> games = await chessComClient.GetAllGamesAsync(username);

    foreach (ChessComGame game in games)
    {
        GameReport report = Converter.ConvertReport(game);
        if (game.Rated)
        {
            counter += Database.InsertGame(report, connection);
        }
    }
    return Results.Ok(counter);
});
app.MapGet("/api/games/openings", async (string username) => { return await Database.SortingOpenings(connectionString, username); });

app.MapGet("/api/games/bycolor", async (string username) => { return await Database.PerformanceForColor(connectionString, username); });

app.MapGet("/api/games/monthly_comparison", async (string username) => { return await Database.MonthlyPerformance(connectionString, username); });

app.MapGet("/api/games/endings", async (string username) => { return await Database.WinRateByEndings(connectionString, username); });

app.Run();
