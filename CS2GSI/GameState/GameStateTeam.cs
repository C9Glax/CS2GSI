using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record GameStateTeam : GameState
{
    public CS2Team Team;
    public int Score, ConsecutiveRoundLosses, TimeoutsRemaining, MatchesWonThisSeries;
    
    public override string ToString()
    {
        return base.ToString();
    }
    
    internal static GameStateTeam ParseFromJObject(JObject jsonObject, CS2Team team)
    {
        return new GameStateTeam()
        {
            Team = team,
            Score = jsonObject.SelectToken($"map.team_{team.ToString().ToLower()}.score")!.Value<int>(),
            ConsecutiveRoundLosses = jsonObject.SelectToken($"map.team_{team.ToString().ToLower()}.consecutive_round_losses")!.Value<int>(),
            TimeoutsRemaining = jsonObject.SelectToken($"map.team_{team.ToString().ToLower()}.timeouts_remaining")!.Value<int>(),
            MatchesWonThisSeries = jsonObject.SelectToken($"map.team_{team.ToString().ToLower()}.matches_won_this_series")!.Value<int>(),
        };
    }
}