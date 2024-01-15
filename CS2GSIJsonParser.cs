using CS2GSI.GameState;
using Newtonsoft.Json.Linq;

namespace CS2GSI;

internal static class CS2GSIJsonParser
{
    internal static CS2GameState ParseGameStateFromJson(string jsonString)
    {
        JObject jsonObject = JObject.Parse(jsonString);
        return new CS2GameState()
        {
            ProviderSteamId = jsonObject.SelectToken("provider.steamid")!.Value<string>()!,
            Timestamp = jsonObject.SelectToken("provider.timestamp")!.Value<int>(),
            Map = ParseMapFromJObject(jsonObject),
            Player = ParsePlayerFromJObject(jsonObject)
        };
    }

    private static Map? ParseMapFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("map") is { } mapToken
            ? new Map()
            {
                Mode = jsonObject.SelectToken("map.mode")!.Value<string>()!,
                Name = jsonObject.SelectToken("map.name")!.Value<string>()!,
                Phase = jsonObject.SelectToken("map.phase")!.Value<string>()!,
                Round = jsonObject.SelectToken("map.round")!.Value<int>(),
                NumMatchesToWinSeries = jsonObject.SelectToken("map.num_matches_to_win_series")!.Value<int>(),
                TeamCT = ParseTeamFromJObject(jsonObject, "ct"),
                TeamT = ParseTeamFromJObject(jsonObject, "t")
            }
            : null;
    }

    private static Team ParseTeamFromJObject(JObject jsonObject, string team)
    {
        return new Team()
        {
            Score = jsonObject.SelectToken($"map.team_{team}.score")!.Value<int>(),
            ConsecutiveRoundLosses = jsonObject.SelectToken($"map.team_{team}.consecutive_round_losses")!.Value<int>(),
            TimeoutsRemaining = jsonObject.SelectToken($"map.team_{team}.timeouts_remaining")!.Value<int>(),
            MatchesWonThisSeries = jsonObject.SelectToken($"map.team_{team}.matches_won_this_series")!.Value<int>(),
        };
    }

    private static Player? ParsePlayerFromJObject(JObject jsonObject)
    {
        return new Player()
        {
            SteamId = jsonObject.SelectToken("player.steamid")!.Value<string>()!,
            Name = jsonObject.SelectToken("player.name")!.Value<string>()!,
            Team = jsonObject.SelectToken("player.team")!.Value<string>()!,
            Activity = jsonObject.SelectToken("player.activity")!.Value<string>()!,
            State = ParsePlayerStateFromJObject(jsonObject),
            MatchStats = ParsePlayerMatchStatsFromJObject(jsonObject)
        };
    }

    private static PlayerState ParsePlayerStateFromJObject(JObject jsonObject)
    {
        return new PlayerState()
        {
            Health = jsonObject.SelectToken($"player.state.health")!.Value<int>(),
            Armor = jsonObject.SelectToken($"player.state.armor")!.Value<int>(),
            Helmet = jsonObject.SelectToken($"player.state.helmet")!.Value<bool>(),
            Flashed = jsonObject.SelectToken($"player.state.flashed")!.Value<int>(),
            Smoked = jsonObject.SelectToken($"player.state.smoked")!.Value<int>(),
            Burning = jsonObject.SelectToken($"player.state.burning")!.Value<int>(),
            Money = jsonObject.SelectToken($"player.state.money")!.Value<int>(),
            RoundKills = jsonObject.SelectToken($"player.state.round_kills")!.Value<int>(),
            RoundHs = jsonObject.SelectToken($"player.state.round_killhs")!.Value<int>(),
            EquipmentValue = jsonObject.SelectToken($"player.state.equip_value")!.Value<int>(),
        };
    }
    
    private static PlayerMatchStats ParsePlayerMatchStatsFromJObject(JObject jsonObject)
    {
        return new PlayerMatchStats()
        {
            Kills = jsonObject.SelectToken($"player.match_stats.kills")!.Value<int>(),
            Assists = jsonObject.SelectToken($"player.match_stats.assists")!.Value<int>(),
            Deaths = jsonObject.SelectToken($"player.match_stats.deaths")!.Value<int>(),
            MVPs = jsonObject.SelectToken($"player.match_stats.mvps")!.Value<int>(),
            Score = jsonObject.SelectToken($"player.match_stats.score")!.Value<int>(),
        };
    }
}