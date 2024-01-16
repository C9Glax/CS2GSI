using Newtonsoft.Json.Linq;

namespace CS2GSI.GameState;

public record PlayerState : GameState
{
    public int Health, Armor, Flashed, Smoked, Burning, Money, RoundKills, RoundHs, EquipmentValue;
    public bool Helmet;
    
    public override string ToString()
    {
        return base.ToString();
    }
    
    internal static PlayerState? ParseFromJObject(JObject jsonObject)
    {
        return jsonObject.SelectToken("player.state") is not null ? new PlayerState()
        {
            Health = jsonObject.SelectToken("player.state.health")!.Value<int>(),
            Armor = jsonObject.SelectToken("player.state.armor")!.Value<int>(),
            Helmet = jsonObject.SelectToken("player.state.helmet")!.Value<bool>(),
            Flashed = jsonObject.SelectToken("player.state.flashed")!.Value<int>(),
            Smoked = jsonObject.SelectToken("player.state.smoked")!.Value<int>(),
            Burning = jsonObject.SelectToken("player.state.burning")!.Value<int>(),
            Money = jsonObject.SelectToken("player.state.money")!.Value<int>(),
            RoundKills = jsonObject.SelectToken("player.state.round_kills")!.Value<int>(),
            RoundHs = jsonObject.SelectToken("player.state.round_killhs")!.Value<int>(),
            EquipmentValue = jsonObject.SelectToken("player.state.equip_value")!.Value<int>(),
        } : null;
    }
}