using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public struct CS2GameState
{
    public string ProviderSteamId;
    public int Timestamp;
    public Map? Map;
    public Player? Player;
    public Round? Round;

    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tTime: {Timestamp}\tSteamId: {ProviderSteamId}\n" +
               $"\t{Map}\n" +
               $"\t{Round}\n" +
               $"\t{Player}\n";
    }
    
    internal static CS2GameState ParseFromJObject(JObject jsonObject)
    {
        return new CS2GameState()
        {
            ProviderSteamId = jsonObject.SelectToken("provider.steamid")!.Value<string>()!,
            Timestamp = jsonObject.SelectToken("provider.timestamp")!.Value<int>(),
            Map = GameState.Map.ParseFromJObject(jsonObject),
            Player = GameState.Player.ParseFromJObject(jsonObject),
            Round = GameState.Round.ParseFromJObject(jsonObject)
        };
    }

    internal CS2GameState? UpdateGameStateForLocal(CS2GameState? previousLocalState)
    {
        if (previousLocalState is null)
            return this.Player?.SteamId == ProviderSteamId ? this : null;
        if (this.Player?.SteamId != ProviderSteamId)
            return this.WithPlayer(previousLocalState.Value.Player);
        return this;
    }

    private CS2GameState WithPlayer(Player? player)
    {
        this.Player = player;
        return this;
    }

    private CS2GameState WithMap(Map? map)
    {
        this.Map = map;
        return this;
    }

    private CS2GameState WithTimestamp(int timestamp)
    {
        this.Timestamp = timestamp;
        return this;
    }
}