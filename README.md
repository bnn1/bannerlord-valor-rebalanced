# Valor Rebalanced

A Mount & Blade II: Bannerlord mod that fixes and rebalances the Valor trait, making it actually achievable in a normal playthrough.

## The Problem

In vanilla Bannerlord, the Valor trait is **completely broken** — you will never earn a single point of Valor XP, no matter how outnumbered you are.

### Bug: strength calculated after the enemy is already defeated

The game calculates the strength ratio between sides to determine valor XP. However, `PlayerEncounter` calls `MapEvent.RecalculateStrengthOfSides()` **after** the battle resolves — at which point the defeated enemy has **0 remaining strength**. The resulting ratio is always near zero, which never meets the threshold, so **valor is never awarded**.

### Balance: even without the bug, the numbers are absurd

Even if the timing bug were fixed, the vanilla thresholds make valor nearly impossible to earn in a normal playthrough:

| What | Vanilla Value |
|---|---|
| Strength ratio threshold | **9.0** (enemy must be 9x stronger) |
| XP per battle | 5–20 |
| Valor level 1 | 1,000 XP → 50+ qualifying battles |
| Valor level 2 | 4,000 XP → 200+ qualifying battles |

## What This Mod Does

1. **Fixes the timing bug**: Bypasses the vanilla handler and reads the strength ratio that was recorded *before* the battle resolved, when enemy strength was still intact.

2. **Fixes the win check**: The vanilla code runs the valor calculation on battle end regardless of outcome. This mod only awards valor when you actually **win**.

3. **Fully configurable via MCM**: All valor parameters are exposed as sliders in the Mod Configuration Menu. Tweak to your liking, or leave at vanilla defaults.

4. **Defaults to vanilla values**: Out of the box, the mod only fixes the bugs. The threshold and XP range remain at vanilla values until you change them.

## MCM Settings

Open **Mod Options → Valor Rebalanced → Valor XP** in the game menu.

| Setting | Range | Default | What it does |
|---|---|---|---|
| **Strength Ratio Threshold** | 1.0–10.0 | 9.0 | How much stronger the enemy must be before valor is awarded. Lower = easier. (1.5 = 50% stronger, 3.0 = 3x, 9.0 = vanilla) |
| **Min XP per Battle** | 1–200 | 5 | XP when barely above the threshold |
| **Max XP per Battle** | 1–500 | 20 | XP when massively outnumbered (10x, the game's cap) |

All values are scaled by your party's **contribution rate** — in large army battles where you're one of many parties, you'll earn proportionally less.

### Suggested "rebalanced" settings

For a challenging but achievable valor progression:

| Setting | Value |
|---|---|
| Strength Ratio Threshold | **1.5** |
| Min XP per Battle | **10** |
| Max XP per Battle | **50** |

This makes Valor 1 achievable in ~20–100 qualifying battles and Valor 2 in ~80–400, depending on how outnumbered you are.

## Requirements

- [Bannerlord Harmony](https://www.nexusmods.com/mountandblade2bannerlord/mods/2006)
- [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612)

## Installation

1. Install the requirements above if you haven't already.
2. Extract the `Bannerlord.FixValor` folder into your game's `Modules` directory:
   ```
   \Modules\Bannerlord.FixValor\
       SubModule.xml
       bin\Win64_Shipping_Client\Bannerlord.FixValor.dll
   ```
3. Enable **Valor Rebalanced** in the launcher. Make sure it loads after Harmony and MCM.
4. Adjust settings in **Mod Options** in-game. No restart required.

## Compatibility

- **Game version**: 1.3.x
- **Save-safe**: Can be added or removed mid-playthrough.
- Works with other mods that don't also patch `PlayerVariablesBehavior.OnPlayerBattleEnd`.

## Source Code

[GitHub](https://github.com/bnn1/bannerlord-valor-rebalanced)

## Credits

[Fix Valor](https://github.com/ivokosir/Bannerlord.FixValor) was used as a base.