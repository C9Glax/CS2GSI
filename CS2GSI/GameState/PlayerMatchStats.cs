using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record PlayerMatchStats : GameState
{
    public int Kills, Assists, Deaths, MVPs, Score;
    
    public override string ToString()
    {
        return base.ToString();
    }
    
    internal static PlayerMatchStats? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("player.match_stats") is not null ? new PlayerMatchStats()
        {
            Kills = jsonObject.SelectToken("player.match_stats.kills")!.Value<int>(),
            Assists = jsonObject.SelectToken("player.match_stats.assists")!.Value<int>(),
            Deaths = jsonObject.SelectToken("player.match_stats.deaths")!.Value<int>(),
            MVPs = jsonObject.SelectToken("player.match_stats.mvps")!.Value<int>(),
            Score = jsonObject.SelectToken("player.match_stats.score")!.Value<int>(),
        } : null;
    }
}