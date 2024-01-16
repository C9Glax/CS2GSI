using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record Map : GameState
{
    public string Mode, MapName;
    public MapPhase Phase;
    public int Round, NumMatchesToWinSeries;
    public GameStateTeam GameStateTeamCT, GameStateTeamT;
    
    public override string ToString()
    {
        return base.ToString();
    }

    internal static Map? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("map") is { } mapToken
            ? new Map()
            {
                Mode = jsonObject.SelectToken("map.mode")!.Value<string>()!,
                MapName = jsonObject.SelectToken("map.name")!.Value<string>()!,
                Phase = MapPhaseFromString(jsonObject.SelectToken("map.phase")!.Value<string>()!),
                Round = jsonObject.SelectToken("map.round")!.Value<int>(),
                NumMatchesToWinSeries = jsonObject.SelectToken("map.num_matches_to_win_series")!.Value<int>(),
                GameStateTeamCT = GameStateTeam.ParseFromJObject(jsonObject, CS2Team.CT),
                GameStateTeamT = GameStateTeam.ParseFromJObject(jsonObject, CS2Team.T)
            }
            : null;
    }
    
    public enum MapPhase {Warmup, Live, Intermission, GameOver}
    
    private static MapPhase MapPhaseFromString(string str)
    {
        return str switch
        {
            "warmup" => MapPhase.Warmup,
            "live" => MapPhase.Live,
            "intermission" => MapPhase.Intermission,
            // ReSharper disable once StringLiteralTypo
            "gameover" => MapPhase.GameOver,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}