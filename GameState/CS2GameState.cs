namespace CS2GSI.GameState;

public struct CS2GameState
{
    public string ProviderSteamId;
    public int Timestamp;
    public Map? Map;
    public Player? Player;

    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tTime: {Timestamp}\tSteamId: {ProviderSteamId}\n" +
               $"\t{Map}\n" +
               $"\t{Player}\n";
    }
}