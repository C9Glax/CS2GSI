namespace CS2GSI.GameState;

public struct PlayerState
{
    public int Health, Armor, Flashed, Smoked, Burning, Money, RoundKills, RoundHs, EquipmentValue;
    public bool Helmet;
    
    public override string ToString()
    {
        return $"{GetType()}\n" +
               $"\tHealth: {Health}\n" +
               $"\tArmor: {Armor}\n" +
               $"\tFlashed: {Flashed}\n" +
               $"\tSmoked: {Smoked}\n" +
               $"\tBurning: {Burning}\n" +
               $"\tMoney: {Money}\n" +
               $"\tRoundKills: {RoundKills}\n" +
               $"\tRoundHs: {RoundHs}\n" +
               $"\tEquipmentValue: {EquipmentValue}\n";
    }
}