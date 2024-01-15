using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct PlayerMatchStats
{
    public int Kills, Assists, Deaths, MVPs, Score;
    
    internal static PlayerMatchStats ParseFromJObject(JObject jsonObject)
    {
        return new PlayerMatchStats()
        {
            Kills = jsonObject.SelectToken($"player.match_stats.kills")!.Value<int>(),
            Assists = jsonObject.SelectToken($"player.match_stats.assists")!.Value<int>(),
            Deaths = jsonObject.SelectToken($"player.match_stats.deaths")!.Value<int>(),
            MVPs = jsonObject.SelectToken($"player.match_stats.mvps")!.Value<int>(),
            Score = jsonObject.SelectToken($"player.match_stats.score")!.Value<int>(),
        };
    }
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tKAD: {Kills} {Assists} {Deaths}\n" +
               $"\tMVPs: {MVPs}\n" +
               $"\tScore: {Score}\n";
    }
}