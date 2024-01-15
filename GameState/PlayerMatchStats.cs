namespace CS2GSI.GameState;

public struct PlayerMatchStats
{
    public int Kills, Assists, Deaths, MVPs, Score;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tKAD: {Kills} {Assists} {Deaths}\n" +
               $"\tMVPs: {MVPs}\n" +
               $"\tScore: {Score}\n";
    }
}