    namespace MyChess;
    public enum Result
    {
        White,
        Black,
        Draw
    }
    public enum EndingCausedBy
    {
        Checkmate,
        Time,
        Stalemate,

        Abandonment,
        Agreement,
        Resignation,
        Repetition,
        Agreed,
        Insufficient,
        Timevsinsufficient,
        Fiftymove
}
public record GameReport
{
    public string ChessComGameId {get; set;}= ""; 
    public string WhiteName {get; set;}= "";
    public string BlackName {get; set;}= "";
    public DateTime GameTime{get; set;}  
    public Result GameResult{get; set;}
    public EndingCausedBy EndingCausedBy{get; set;}
    public int WhiteRating{get; set;} = 0;
    public int BlackRating{get; set;} = 0;
    public string analysisLink {get; set;}= "";
    public string ecoCode {get; set;}= "";
    public double WhiteAccuracy{get; set;}= 0;
    public double BlackAccuracy{get; set;} = 0;
}
