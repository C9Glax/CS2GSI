using CS2GSI.GameState;

namespace CS2GSI;

internal static class CS2EventGenerator
{
    internal static List<ValueTuple<CS2Event, CS2EventArgs>> GenerateEvents(CS2GameState previousPlayerState, CS2GameState newGameState, CS2GameState lastGameState)
    {
        List<ValueTuple<CS2Event, CS2EventArgs>> events = new();
        events.AddRange(GeneratePlayerEvents(previousPlayerState, newGameState));
        events.AddRange(GenerateMapEvents(previousPlayerState, newGameState, lastGameState));
        
        if(lastGameState.Player?.SteamId != newGameState.Player?.SteamId)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnPlayerChange, new CS2EventArgs(newGameState.Player?.SteamId)));
        
        if(events.Count > 0)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.AnyEvent, new CS2EventArgs(events.Count)));

        if (lastGameState.Player?.Activity != newGameState.Player?.Activity)
            events.Add(new(CS2Event.OnActivityChange, new CS2EventArgs(newGameState.Player?.Activity)));
        
        events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.AnyMessage, new CS2EventArgs()));
        return events;
    }

    private static List<ValueTuple<CS2Event, CS2EventArgs>> GenerateMapEvents(CS2GameState previousPlayerState, CS2GameState newGameState, CS2GameState lastGameState)
    {
        List<ValueTuple<CS2Event, CS2EventArgs>> events = new();

        if (newGameState.Player?.Team == previousPlayerState.Player?.Team && lastGameState.Player?.Team == previousPlayerState.Player?.Team)
        {
            if(newGameState.Round?.Phase == Round.RoundPhase.Live && lastGameState.Round?.Phase != Round.RoundPhase.Live)
                events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnRoundStart, new CS2EventArgs()));
            
            if(newGameState.Round?.Phase == Round.RoundPhase.Over && lastGameState.Round?.Phase == Round.RoundPhase.Live)
                events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnRoundOver, new CS2EventArgs()));

            if (newGameState.Map?.Round is not null && newGameState.Map?.Round != previousPlayerState.Map?.Round)
            {
                if(newGameState.Round?.WinnerTeam is not null && newGameState.Round?.WinnerTeam == previousPlayerState.Player?.Team)
                    events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnRoundWin, new CS2EventArgs()));
            
                if(newGameState.Round?.WinnerTeam is not null && newGameState.Round?.WinnerTeam != previousPlayerState.Player?.Team)
                    events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnRoundLoss, new CS2EventArgs()));
            }
        }
        
        if(newGameState.Map?.Phase == Map.MapPhase.Live && lastGameState.Map?.Phase == Map.MapPhase.Warmup)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnMatchStart, new CS2EventArgs()));
        
        if(newGameState.Map?.Phase == Map.MapPhase.GameOver && lastGameState.Map?.Phase != Map.MapPhase.GameOver)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnMatchOver, new CS2EventArgs()));
        
        if(newGameState.Map?.Phase == Map.MapPhase.Intermission && lastGameState.Map?.Phase != Map.MapPhase.Intermission)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnHalfTime, new CS2EventArgs()));
        
        if(newGameState.Round?.Phase == Round.RoundPhase.Freezetime && lastGameState.Round?.Phase != Round.RoundPhase.Freezetime)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnFreezeTime, new CS2EventArgs()));
        
        if(newGameState.Round?.Bomb == Round.BombStatus.Planted && lastGameState.Round?.Bomb != Round.BombStatus.Planted)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnBombPlanted, new CS2EventArgs()));
        
        if(newGameState.Round?.Bomb == Round.BombStatus.Defused && lastGameState.Round?.Bomb != Round.BombStatus.Defused)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnBombDefused, new CS2EventArgs()));
        
        if(newGameState.Round?.Bomb == Round.BombStatus.Exploded && lastGameState.Round?.Bomb != Round.BombStatus.Exploded)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnBombExploded, new CS2EventArgs()));
        
        return events;
    }
    
    private static List<ValueTuple<CS2Event, CS2EventArgs>> GeneratePlayerEvents(CS2GameState previousPlayerState, CS2GameState newGameState)
    {
        List<ValueTuple<CS2Event, CS2EventArgs>> events = new();
        if (newGameState.Player?.SteamId != newGameState.ProviderSteamId || previousPlayerState.Player?.SteamId != newGameState.Player?.SteamId)
            return events;
        
        if(newGameState.Player?.MatchStats?.Kills > previousPlayerState.Player?.MatchStats?.Kills && newGameState.Player is { MatchStats: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnKill, new CS2EventArgs(newGameState.Player?.MatchStats?.Kills)));
        
        if(newGameState.Player?.State?.RoundHs > previousPlayerState.Player?.State?.RoundHs && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnHeadshot, new CS2EventArgs(newGameState.Player?.State?.RoundHs)));
        
        if(newGameState.Player?.MatchStats?.Deaths > previousPlayerState.Player?.MatchStats?.Deaths && newGameState.Player is { MatchStats: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnDeath, new CS2EventArgs(newGameState.Player?.MatchStats?.Deaths)));
        
        if(newGameState.Player?.State?.Flashed > 0 && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnFlashed, new CS2EventArgs()));
        
        if(newGameState.Player?.State?.Burning > 0 && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnBurning, new CS2EventArgs()));
        
        if(newGameState.Player?.State?.Smoked > 0 && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnSmoked, new CS2EventArgs()));
        
        if(newGameState.Player?.State?.Health < previousPlayerState.Player?.State?.Health && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnDamageTaken, new CS2EventArgs(previousPlayerState.Player?.State?.Health - newGameState.Player?.State?.Health)));
        
        if(newGameState.Player?.State?.Health != previousPlayerState.Player?.State?.Health && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnHealthChange, new CS2EventArgs(newGameState.Player?.State?.Health - previousPlayerState.Player?.State?.Health)));
        
        if(newGameState.Player?.State?.Money != previousPlayerState.Player?.State?.Money && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnMoneyChange, new CS2EventArgs(newGameState.Player?.State?.Money - previousPlayerState.Player?.State?.Money)));
        
        if(newGameState.Player?.State?.Armor != previousPlayerState.Player?.State?.Armor && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnArmorChange, new CS2EventArgs(newGameState.Player?.State?.Armor - previousPlayerState.Player?.State?.Armor)));
        
        if(newGameState.Player?.State?.Helmet != previousPlayerState.Player?.State?.Helmet && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnHelmetChange, new CS2EventArgs(newGameState.Player?.State?.Helmet)));
        
        if(newGameState.Player?.State?.EquipmentValue != previousPlayerState.Player?.State?.EquipmentValue && newGameState.Player is { State: not null})
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnEquipmentValueChange, new CS2EventArgs(newGameState.Player?.State?.EquipmentValue - previousPlayerState.Player?.State?.EquipmentValue)));
        
        if(newGameState.Player?.Team != previousPlayerState.Player?.Team && newGameState.Player is not null)
            events.Add(new ValueTuple<CS2Event, CS2EventArgs>(CS2Event.OnTeamChange, new CS2EventArgs(newGameState.Player?.Team)));
        
        
        return events;
    }
}