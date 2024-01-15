using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct Player
{
    public string SteamId, Name, Activity;
    public string? Team;
    public int? ObserverSlot;
    public PlayerState? State;
    public PlayerMatchStats? MatchStats;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\t{Name} {SteamId} {Activity} {Team}\n" +
               $"\t{State}\n" +
               $"\t{MatchStats}\n";
    }
    
    internal static Player? ParseFromJObject(JObject jsonObject)
    {
        return new Player()
        {
            SteamId = jsonObject.SelectToken("player.steamid")!.Value<string>()!,
            Name = jsonObject.SelectToken("player.name")!.Value<string>()!,
            Team = jsonObject.SelectToken("player.team")!.Value<string>()!,
            Activity = jsonObject.SelectToken("player.activity")!.Value<string>()!,
            State = PlayerState.ParseFromJObject(jsonObject),
            MatchStats = PlayerMatchStats.ParseFromJObject(jsonObject)
        };
    }
}