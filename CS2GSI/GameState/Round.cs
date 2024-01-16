using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record Round : GameState
{
    public RoundPhase Phase;
    public BombStatus? Bomb;
    public CS2Team? WinnerTeam;
    
    public override string ToString()
    {
        return base.ToString();
    }
    
    internal static Round? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("round") is not null ? new Round()
        {
            Phase = RoundPhaseFromString(jsonObject.SelectToken("round.phase")!.Value<string>()!),
            WinnerTeam = CS2TeamFromString(jsonObject.SelectToken("round.win_team")?.Value<string>()),
            Bomb = BombStatusFromString(jsonObject.SelectToken("round.bomb")?.Value<string>())
        } : null;
    }
    
    public enum RoundPhase
    {
        Over, Freezetime, Live
    }
    
    private static RoundPhase RoundPhaseFromString(string str)
    {
        return str switch
        {
            "over" => RoundPhase.Over,
            "live" => RoundPhase.Live,
            "freezetime" => RoundPhase.Freezetime,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public enum BombStatus
    {
        Planted, Exploded, Defused
    }
    
    private static BombStatus? BombStatusFromString(string? str)
    {
        return str switch
        {
            "planted" => BombStatus.Planted,
            "exploded" => BombStatus.Exploded,
            "defused" => BombStatus.Defused,
            _ => null
        };
    }
    
    private static CS2Team? CS2TeamFromString(string? str)
    {
        return str?.ToLower() switch
        {
            "t" => CS2Team.T,
            "ct" => CS2Team.CT,
            _ => null
        };
    }
}