# CS2GSI

## Example Usage
```csharp
public static void Main(string[] args)
    {
        CS2GSI.CS2GSI gsi = new ();
        gsi.AnyMessage += eventArgs => Console.WriteLine("Message");
        gsi.OnKill += eventArgs => Console.WriteLine($"Kill number {eventArgs.ValueAsOrDefault<int>()}");
        while(gsi.IsRunning)
            Thread.Sleep(10);
    }
```

### Events

`EventName` (_ParameterType_) Description

* `OnKill` (_int_) Number of Kills in Match
* `OnHeadshot` (_int_) Number of Headshots in Round
* `OnDeath` (_int_) Number of Deaths in Match
* `OnFlashed`
* `OnBurning`
* `OnSmoked`
* `OnRoundStart`
* `OnRoundOver`
* `OnRoundWin`
* `OnRoundLoss`
* `OnDamageTaken` (_int_) Amount of Damage Taken
* `OnMatchStart` 
* `OnMatchOver`
* `OnMoneyChange` (_int_) Delta in Money
* `OnHealthChange` (_int_) Delta in Health
* `OnArmorChange` (_int_) Delta in Armor
* `OnHelmetChange` (_bool_) Helmet on/off
* `OnEquipmentValueChange` (_int_) Delta in Equipmentvalue
* `OnTeamChange`
* `OnPlayerChange` (_string_) SteamId64
* `OnHalfTime`
* `OnFreezeTime`
* `OnBombPlanted`
* `OnBombDefused`
* `OnBombExploded`
* `AnyEvent`
* `AnyMessage`