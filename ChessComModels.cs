using System.Text.Json.Serialization;
class ChessComResponse
{
    public List<ChessComGame> Games { get; set; } = new();
}

// One game
class ChessComGame
{
    public string Url { get; set; } = "";
    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
    [JsonPropertyName("rated")]
    public bool Rated { get; set; }
    public string Uuid { get; set; } = "";
    public string Eco { get; set; } = "";
    public ChessComAccuracies Accuracies { get; set; } = new();
    public ChessComPlayer White { get; set; } = new();
    public ChessComPlayer Black { get; set; } = new();
}
class ChessComAccuracies
{
    public double White { get; set; }
    public double Black { get; set; }
}
class ChessComPlayer
{
    public int Rating { get; set; }
    public string Result { get; set; } = "";

    public string Username {get; set;} = "";
}
class ChessComArchives {
    [JsonPropertyName("archives")]
    public List<string> Archives { get; set; } = new();
}