using CS2GSI.GameState;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CS2GSI;

public class CS2GSI
{
    private GSIServer _gsiServer;
    private List<CS2GameState> _allGameStates = new();
    private CS2GameState? _lastLocalGameState = null;
    private ILogger? logger;

    public CS2GSI(ILogger? logger = null)
    {
        this.logger = logger;
        this.logger?.Log(LogLevel.Information, "Installing GSI-Configfile...");
        try
        {
            GsiConfigInstaller.InstallGsi();
        }
        catch (Exception e)
        {
            this.logger?.Log(LogLevel.Error, e.StackTrace);
            this.logger?.Log(LogLevel.Critical, "Could not install GSI-Configfile. Exiting.");
            return;
        }
        this._gsiServer = new GSIServer(3000);
        this._gsiServer.OnMessage += GsiServerOnOnMessage;
        
        while(this._gsiServer.IsRunning)
            Thread.Sleep(10);
    }

    private void GsiServerOnOnMessage(string messageJson)
    {
        JObject jsonObject = JObject.Parse(messageJson);
        CS2GameState newState = CS2GameState.ParseFromJObject(jsonObject);
        this.logger?.Log(LogLevel.Debug, $"Received State:\n{newState.ToString()}");
        this._lastLocalGameState = newState.UpdateGameStateForLocal(_lastLocalGameState);
        this.logger?.Log(LogLevel.Debug, $"Updated Local State:\n{_lastLocalGameState.ToString()}");

        if (_lastLocalGameState is not null)
        {
            List<ValueTuple<CS2Event, CS2EventArgs>> generatedEvents = CS2EventGenerator.GenerateEvents(_lastLocalGameState.Value, newState, _allGameStates.Last());
            this.logger?.Log(LogLevel.Information, $"Generated {generatedEvents.Count} events.");
            if(generatedEvents.Count > 0)
                this.logger?.Log(LogLevel.Debug, $"Events:\n\t{string.Join("\n\t", generatedEvents)}");
            InvokeEvents(generatedEvents);
        }
        _allGameStates.Add(newState);
    }


    private void InvokeEvents(List<ValueTuple<CS2Event, CS2EventArgs>> cs2Events)
    {
        foreach(ValueTuple<CS2Event, CS2EventArgs> cs2Event in cs2Events)
            InvokeEvent(cs2Event);
    }

    private void InvokeEvent(ValueTuple<CS2Event, CS2EventArgs> cs2Event)
    {
        switch (cs2Event.Item1)
        {
            case CS2Event.OnKill:
                OnKill?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnHeadshot:
                OnHeadshot?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnDeath:
                OnDeath?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnFlashed:
                OnFlashed?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnBurning:
                OnBurning?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnSmoked:
                OnSmoked?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnRoundStart:
                OnRoundStart?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnRoundOver:
                OnRoundOver?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnDamageTaken:
                OnDamageTaken?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnMatchStart:
                OnMatchStart?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnMatchOver:
                OnMatchOver?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnMoneyChange:
                OnMoneyChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnHealthChange:
                OnHealthChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnArmorChange:
                OnArmorChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnHelmetChange:
                OnHelmetChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnEquipmentValueChange:
                OnEquipmentValueChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnTeamChange:
                OnTeamChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnPlayerChange:
                OnPlayerChange?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnFreezeTime:
                OnFreezeTime?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnHalfTime:
                OnHalfTime?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnBombDefused:
                OnBombDefused?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnBombExploded:
                OnBombExploded?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnBombPlanted:
                OnBombPlanted?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.AnyEvent:
                AnyEvent?.Invoke(cs2Event.Item2);
                break;
            default:
                this.logger?.Log(LogLevel.Error, $"Unknown Event {cs2Event}");
                return;
        }
    }
    
    internal enum CS2Event {
        OnKill,
        OnHeadshot,
        OnDeath,
        OnFlashed,
        OnBurning,
        OnSmoked,
        OnRoundStart,
        OnRoundOver,
        OnDamageTaken,
        OnMatchStart,
        OnMatchOver,
        OnMoneyChange,
        OnHealthChange,
        OnArmorChange,
        OnHelmetChange,
        OnEquipmentValueChange,
        OnTeamChange,
        OnPlayerChange,
        OnHalfTime,
        OnFreezeTime,
        OnBombPlanted,
        OnBombDefused,
        OnBombExploded,
        AnyEvent
    }
    
    public delegate void CS2EventHandler(CS2EventArgs eventArgs);

    public event CS2EventHandler? OnKill,
        OnHeadshot,
        OnDeath,
        OnFlashed,
        OnBurning,
        OnSmoked,
        OnRoundStart,
        OnRoundOver,
        OnDamageTaken,
        OnMatchStart,
        OnMatchOver,
        OnMoneyChange,
        OnHealthChange,
        OnArmorChange,
        OnHelmetChange,
        OnEquipmentValueChange,
        OnTeamChange,
        OnPlayerChange,
        OnHalfTime,
        OnFreezeTime,
        OnBombPlanted,
        OnBombDefused,
        OnBombExploded,
        AnyEvent;

}