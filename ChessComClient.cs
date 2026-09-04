using System.Text.Json;

public class ChessComClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public ChessComClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ChessComGame>> GetAllGamesAsync(string username)
    {
        var archiveUrl = $"https://api.chess.com/pub/player/{username}/games/archives";
        var archiveJson = await _httpClient.GetStringAsync(archiveUrl);
        var archiveList = JsonSerializer.Deserialize<ChessComArchives>(archiveJson, _options);

        var allGames = new List<ChessComGame>();
        foreach (string url in archiveList!.Archives)
        {
            var json = await _httpClient.GetStringAsync(url);
            var response = JsonSerializer.Deserialize<ChessComResponse>(json, _options);
            if (response == null) { throw new InvalidOperationException($"Failed to deserialize chess.com response from {url}."); }
            allGames.AddRange(response.Games);
        }
        return allGames;
    }
}
