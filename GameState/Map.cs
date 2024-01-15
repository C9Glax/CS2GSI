using Newtonsoft.Json.Linq;

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

    internal static Map? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("map") is { } mapToken
            ? new Map()
            {
                Mode = jsonObject.SelectToken("map.mode")!.Value<string>()!,
                Name = jsonObject.SelectToken("map.name")!.Value<string>()!,
                Phase = jsonObject.SelectToken("map.phase")!.Value<string>()!,
                Round = jsonObject.SelectToken("map.round")!.Value<int>(),
                NumMatchesToWinSeries = jsonObject.SelectToken("map.num_matches_to_win_series")!.Value<int>(),
                TeamCT = Team.ParseFromJObject(jsonObject, "ct"),
                TeamT = Team.ParseFromJObject(jsonObject, "t")
            }
            : null;
    }
}