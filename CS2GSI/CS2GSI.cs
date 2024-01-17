using CS2GSI.GameState;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CS2GSI;

public class CS2GSI
{
    private readonly GSIServer _gsiServer;
    private readonly List<CS2GameState> _allGameStates = new();
    private CS2GameState? _lastLocalGameState = null;
    private readonly ILogger? _logger;
    public bool IsRunning => this._gsiServer.IsRunning;

    public CS2GSI(ILogger? logger = null)
    {
        this._logger = logger;
        this._logger?.Log(LogLevel.Information, "Installing GSI-Configfile...");
        try
        {
            GsiConfigInstaller.InstallGsi();
        }
        catch (Exception e)
        {
            this._logger?.Log(LogLevel.Error, e.StackTrace);
            this._logger?.Log(LogLevel.Critical, "Could not install GSI-Configfile. Exiting.");
            return;
        }
        this._gsiServer = new GSIServer(3000, logger);
        this._gsiServer.OnMessage += GsiServerOnOnMessage;
    }

    private void GsiServerOnOnMessage(string messageJson)
    {
        JObject jsonObject = JObject.Parse(messageJson);
        CS2GameState newState = CS2GameState.ParseFromJObject(jsonObject);
        this._logger?.Log(LogLevel.Debug, $"Received State:\n{newState.ToString()}");

        if (_lastLocalGameState is not null && _allGameStates.Count > 0)
        {
            List<ValueTuple<CS2Event, CS2EventArgs>> generatedEvents = CS2EventGenerator.GenerateEvents(_lastLocalGameState, newState, _allGameStates.Last());
            this._logger?.Log(LogLevel.Information, $"Generated {generatedEvents.Count} event{(generatedEvents.Count > 1 ? 's' : null)}:\n- {string.Join("\n- ", generatedEvents)}");
            InvokeEvents(generatedEvents);
        }
        this._lastLocalGameState = newState.UpdateGameStateForLocal(_lastLocalGameState);
        this._logger?.Log(LogLevel.Debug, $"\nUpdated Local State:\n{_lastLocalGameState}");
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
            case CS2Event.OnRoundWin:
                OnRoundWin?.Invoke(cs2Event.Item2);
                break;
            case CS2Event.OnRoundLoss:
                OnRoundLoss?.Invoke(cs2Event.Item2);
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
            case CS2Event.AnyMessage:
                AnyMessage?.Invoke(cs2Event.Item2);
                break;
            default:
                this._logger?.Log(LogLevel.Error, $"Unknown Event {cs2Event}");
                return;
        }
    }
    
    public enum CS2Event {
        OnKill,
        OnHeadshot,
        OnDeath,
        OnFlashed,
        OnBurning,
        OnSmoked,
        OnRoundStart,
        OnRoundOver,
        OnRoundWin,
        OnRoundLoss,
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
        AnyEvent,
        AnyMessage
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
        OnRoundWin,
        OnRoundLoss,
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
        AnyEvent,
        AnyMessage;

}