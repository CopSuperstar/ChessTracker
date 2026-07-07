using Dapper;
using Microsoft.Data.Sqlite;
using MyChess;
public static class Database
{
   public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        connection.Execute(@"CREATE TABLE IF NOT EXISTS Games(
        ChessComGameId  TEXT PRIMARY KEY,
WhiteName TEXT NOT NULL,
BlackName TEXT NOT NULL,
GameTime INTEGER NOT NULL,
GameResult TEXT NOT NULL,
EndingCausedBy TEXT NOT NULL,
WhiteRating INTEGER NOT NULL,
BlackRating INTEGER NOT NULL,
analysisLink TEXT NOT NULL,
ecoCode TEXT NOT NULL,
WhiteAccuracy REAL, 
BlackAccuracy REAL
)");
    }
    public static int InsertGame(GameReport game, SqliteConnection connection)
    {
        int count = connection.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Games WHERE ChessComGameId = @Id",
        new { Id = game.ChessComGameId });
        if (count  == 0) { connection.Execute(@"INSERT INTO Games(ChessComGameId, 
 WhiteName,
 BlackName,
 GameTime, 
 GameResult, 
 EndingCausedBy,
 WhiteRating, 
 BlackRating, 
 analysisLink,
 ecoCode, 
 WhiteAccuracy, 
 BlackAccuracy) 
 VALUES (@ChessComGameId, @WhiteName, @BlackName, @GameTime, @GameResult, @EndingCausedBy, @WhiteRating, @BlackRating, @analysisLink, @ecoCode, @WhiteAccuracy, @BlackAccuracy)", 
 new {
    game.ChessComGameId,
    game.WhiteName,
    game.BlackName,
    game.GameTime,
    GameResult = game.GameResult.ToString(),
    EndingCausedBy = game.EndingCausedBy.ToString(),
    game.WhiteRating,
    game.BlackRating,
    game.analysisLink,
    game.ecoCode,
    game.WhiteAccuracy,
    game.BlackAccuracy
 }); return 1;
        }
        else return 0;
    }
    
    public static async Task<IEnumerable<OpeningStats>>  SortingOpenings(string connectionString , string username )
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var OpeningStats_result = await connection.QueryAsync<OpeningStats>(@"SELECT ecoCode,
        COUNT(ChessComGameId) AS games_played,
        AVG(CASE WHEN WhiteName =  @Username THEN WhiteAccuracy ELSE BlackAccuracy END) AS average_accuracy
        FROM Games
       GROUP BY ecoCode
        ORDER BY average_accuracy DESC",
                new { Username = username }
                );
        return OpeningStats_result;
    }
}
    public class OpeningStats
    {
        public string EcoCode {get;set;}
        public int games_played {get;set;}
        public double average_accuracy {get;set;}
    }