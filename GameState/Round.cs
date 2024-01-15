using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct Round
{
    public RoundPhase Phase;
    public string WinnerTeam, BombStatus;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\t{Phase} {WinnerTeam} {BombStatus}\n";
    }
    
    internal static Round? ParseFromJObject(JObject jsonObject)
    {
        return new Round()
        {
            Phase = RoundPhaseFromString(jsonObject.SelectToken("round.phase")!.Value<string>()!),
            WinnerTeam = jsonObject.SelectToken("round.win_team")!.Value<string>()!,
            BombStatus = jsonObject.SelectToken("round.bomb")!.Value<string>()!
        };
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
}