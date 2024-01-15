namespace CS2GSI.GameState;

public struct CS2GameState
{
    public int Timestamp;
    public Map? Map;
    public Player? Player;

    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tTime: {Timestamp}\n" +
               $"\t{Map}\n" +
               $"\t{Player}\n";
    }
}