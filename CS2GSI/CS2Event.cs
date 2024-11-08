﻿namespace CS2GSI;

public enum CS2Event : byte {
    OnKill = 0,
    OnHeadshot = 1,
    OnDeath = 2,
    OnFlashed = 3,
    OnBurning = 4,
    OnSmoked = 5,
    OnRoundStart = 6,
    OnRoundOver = 7,
    OnRoundWin = 8,
    OnRoundLoss = 9,
    OnDamageTaken = 10,
    OnMatchStart = 11,
    OnMatchOver = 12,
    OnMoneyChange = 13,
    OnHealthChange = 14,
    OnArmorChange = 15,
    OnHelmetChange = 16,
    OnEquipmentValueChange = 17,
    OnTeamChange = 18,
    OnPlayerChange = 19,
    OnHalfTime = 20,
    OnFreezeTime = 21,
    OnBombPlanted = 22,
    OnBombDefused = 23,
    OnBombExploded = 24,
    AnyEvent = 25,
    AnyMessage = 26,
    OnActivityChange = 27
}