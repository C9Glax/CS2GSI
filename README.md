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
* OnKill
* OnHeadshot
* OnDeath
* OnFlashed
* OnBurning
* OnSmoked
* OnRoundStart
* OnRoundOver
* OnRoundWin
* OnRoundLoss
* OnDamageTaken
* OnMatchStart
* OnMatchOver
* OnMoneyChange
* OnHealthChange
* OnArmorChange
* OnHelmetChange
* OnEquipmentValueChange
* OnTeamChange
* OnPlayerChange
* OnHalfTime
* OnFreezeTime
* OnBombPlanted
* OnBombDefused
* OnBombExploded
* AnyEvent
* AnyMessage