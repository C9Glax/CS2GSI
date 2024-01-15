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
}