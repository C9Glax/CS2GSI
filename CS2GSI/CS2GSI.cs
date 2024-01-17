using CS2GSI.GameState;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CS2GSI;

public class CS2GSI
{
    private readonly GSIServer _gsiServer = null!;
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
        GetEventHandlerForEvent(cs2Event.Item1)?.Invoke(cs2Event.Item2);
    }
    
    public CS2EventHandler? GetEventHandlerForEvent(CS2Event cs2Event)
    {
        return cs2Event switch
        {
            CS2Event.OnKill => this.OnKill,
            CS2Event.OnHeadshot => this.OnHeadshot,
            CS2Event.OnDeath => this.OnDeath,
            CS2Event.OnFlashed => this.OnFlashed,
            CS2Event.OnBurning => this.OnBurning,
            CS2Event.OnSmoked => this.OnSmoked,
            CS2Event.OnRoundStart => this.OnRoundStart,
            CS2Event.OnRoundOver => this.OnRoundOver,
            CS2Event.OnRoundWin => this.OnRoundWin,
            CS2Event.OnRoundLoss => this.OnRoundLoss,
            CS2Event.OnDamageTaken => this.OnDamageTaken,
            CS2Event.OnMatchStart => this.OnMatchStart,
            CS2Event.OnMatchOver => this.OnMatchOver,
            CS2Event.OnMoneyChange => this.OnMoneyChange,
            CS2Event.OnHealthChange => this.OnHealthChange,
            CS2Event.OnArmorChange => this.OnArmorChange,
            CS2Event.OnHelmetChange => this.OnHelmetChange,
            CS2Event.OnEquipmentValueChange => this.OnEquipmentValueChange,
            CS2Event.OnTeamChange => this.OnTeamChange,
            CS2Event.OnPlayerChange => this.OnPlayerChange,
            CS2Event.OnHalfTime => this.OnHalfTime,
            CS2Event.OnFreezeTime => this.OnFreezeTime,
            CS2Event.OnBombPlanted => this.OnBombPlanted,
            CS2Event.OnBombDefused => this.OnBombDefused,
            CS2Event.OnBombExploded => this.OnBombExploded,
            CS2Event.AnyEvent => this.AnyEvent,
            CS2Event.AnyMessage => this.AnyMessage,
            _ => throw new ArgumentException("Unknown Event", nameof(cs2Event))
        };
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