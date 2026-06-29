using MyChess;
static class Converter
{
static (Result gameResult, EndingCausedBy endingCausedBy) ConvertResult(ChessComGame game)
{
    bool isDraw = game.White.Result == "agreed" || game.White.Result == "stalemate";
    
    if (isDraw)
    {
        EndingCausedBy ending = game.White.Result == "agreed" 
            ? EndingCausedBy.Agreement 
            : EndingCausedBy.Stalemate;
        return (Result.Draw, ending);
    }
    
    ChessComPlayer loser = game.White.Result == "win" ? game.Black : game.White;
    Result gameResult = loser == game.Black ? Result.White : Result.Black;
    EndingCausedBy endingCausedBy = loser.Result switch
    {
        "checkmated" => EndingCausedBy.Checkmate,
        "timeout" => EndingCausedBy.Time,
        "resigned" => EndingCausedBy.Resignation,
        "abandoned" => EndingCausedBy.Abandonment,
        _ => throw new Exception($"Unknown result: {loser.Result}")
    };
    
    return (gameResult: gameResult, endingCause:endingCausedBy);
}

    static public GameReport ConvertReport(ChessComGame game)
    {
        GameReport MappedReport = new()
        {
            WhiteName = game.White.Username,
            BlackName = game.Black.Username,
            ChessComGameId = game.Uuid
        };
        DateTime timeTransitional = DateTime.UnixEpoch.AddSeconds(game.EndTime); 
        MappedReport.GameTime = timeTransitional; var tuple = ConvertResult(game);
        MappedReport.GameResult = tuple.gameResult;
        MappedReport.EndingCausedBy = tuple.endingCausedBy;
        MappedReport.WhiteRating = game.White.Rating;
        MappedReport.BlackRating = game.Black.Rating;
        MappedReport.ecoCode = game.Eco;
        MappedReport.analysisLink = game.Url;
        MappedReport.WhiteAccuracy = game.Accuracies.White;
        MappedReport.BlackAccuracy = game.Accuracies.Black;
        return MappedReport;
    }
}