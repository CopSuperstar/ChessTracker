ChessTracker is an ASP.NET Core API that takes chess.com game reports, deserializes them into local database, and executes further analysis of Accuracy, opening and game results.

How To Run: First, we need to download the data from chess.com endpoints in our local database: http://localhost:_____/api/games/fetch?username=_____
Then, new endpoints can be used:

Opening performance stats(http://localhost:_____/api/games/openings?username=username)
Accuracy by color(/api/games/bycolor?username=username)
Monthly accuracy trend(/api/games/monthly_comparison?username=username)
Win rate by ending type(/api/games/endings?username=username)
