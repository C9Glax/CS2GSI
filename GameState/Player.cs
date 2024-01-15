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
}