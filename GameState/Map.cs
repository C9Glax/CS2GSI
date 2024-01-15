namespace CS2GSI.GameState;

public struct Map
{
    public string Mode, Name, Phase;
    public int Round, NumMatchesToWinSeries;
    public Team TeamCT, TeamT;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\t{Mode} {Name} {Round} Matches to Win Series: {NumMatchesToWinSeries}\n" +
               $"\t{Phase}\n" +
               $"\t{TeamCT}\n" +
               $"\t{TeamT}\n";
    }
}