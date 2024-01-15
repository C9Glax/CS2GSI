using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct Player
{
    public string SteamId, Name;
    public PlayerActivity Activity;
    public CS2Team? Team;
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
            Team = CS2TeamFromString(jsonObject.SelectToken("player.team")!.Value<string>()!),
            Activity = PlayerActivityFromString(jsonObject.SelectToken("player.activity")!.Value<string>()!),
            State = PlayerState.ParseFromJObject(jsonObject),
            MatchStats = PlayerMatchStats.ParseFromJObject(jsonObject)
        };
    }
    
    public enum PlayerActivity {Playing, Menu, TextInput}

    private static CS2Team CS2TeamFromString(string str)
    {
        return str.ToLower() switch
        {
            "t" => CS2Team.T,
            "ct" => CS2Team.CT,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static PlayerActivity PlayerActivityFromString(string str)
    {
        return str switch
        {
            "playing" => PlayerActivity.Playing,
            "menu" => PlayerActivity.Menu,
            // ReSharper disable once StringLiteralTypo
            "textinput" => PlayerActivity.TextInput,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}