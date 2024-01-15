using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct Round
{
    public string Phase, WinnerTeam, BombStatus;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\t{Phase} {WinnerTeam} {BombStatus}\n";
    }
    
    internal static Round? ParseFromJObject(JObject jsonObject)
    {
        return new Round()
        {
            Phase = jsonObject.SelectToken("round.phase")!.Value<string>()!,
            WinnerTeam = jsonObject.SelectToken("round.win_team")!.Value<string>()!,
            BombStatus = jsonObject.SelectToken("round.bomb")!.Value<string>()!
        };
    }
}