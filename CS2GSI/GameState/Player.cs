using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record Player : GameState
{
    public string SteamId, Name;
    public PlayerActivity Activity;
    public CS2Team? Team;
    public int? ObserverSlot;
    public PlayerState? State;
    public PlayerMatchStats? MatchStats;
    
    public override string ToString()
    {
        return base.ToString();
    }
    
    internal static Player? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("player") is not null ? new Player()
        {
            SteamId = jsonObject.SelectToken("player.steamid")!.Value<string>()!,
            Name = jsonObject.SelectToken("player.name")!.Value<string>()!,
            Team = CS2TeamFromString(jsonObject.SelectToken("player.team")?.Value<string>()),
            Activity = PlayerActivityFromString(jsonObject.SelectToken("player.activity")!.Value<string>()!),
            State = PlayerState.ParseFromJObject(jsonObject),
            MatchStats = PlayerMatchStats.ParseFromJObject(jsonObject)
        } : null;
    }
    
    public enum PlayerActivity {Playing, Menu, TextInput}

    private static CS2Team? CS2TeamFromString(string? str)
    {
        return str?.ToLower() switch
        {
            "t" => CS2Team.T,
            "ct" => CS2Team.CT,
            _ => null
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