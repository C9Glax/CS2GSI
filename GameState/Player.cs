namespace CS2GSI.GameState;

public struct Player
{
    public string SteamId, Name, Activity;
    public string? Team;
    public int? ObserverSlot;
    public PlayerState? State;
    public PlayerMatchStats? MatchStats;
}