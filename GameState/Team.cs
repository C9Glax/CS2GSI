using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct Team
{
    public int Score, ConsecutiveRoundLosses, TimeoutsRemaining, MatchesWonThisSeries;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tScore: {Score}\n" +
               $"\tConsecutiveRoundLosses: {ConsecutiveRoundLosses}\n" +
               $"\tTimeoutsRemaining: {TimeoutsRemaining}\n" +
               $"\tMatchesWonThisSeries: {MatchesWonThisSeries}\n";
    }
    
    internal static Team ParseFromJObject(JObject jsonObject, string team)
    {
        return new Team()
        {
            Score = jsonObject.SelectToken($"map.team_{team}.score")!.Value<int>(),
            ConsecutiveRoundLosses = jsonObject.SelectToken($"map.team_{team}.consecutive_round_losses")!.Value<int>(),
            TimeoutsRemaining = jsonObject.SelectToken($"map.team_{team}.timeouts_remaining")!.Value<int>(),
            MatchesWonThisSeries = jsonObject.SelectToken($"map.team_{team}.matches_won_this_series")!.Value<int>(),
        };
    }
}