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
| Strength ratio threshold | **9.0** (enemy must be 9× stronger) |
| XP per battle | 5–20 |
| Valor level 1 | 1,000 XP → 50+ qualifying battles |
| Valor level 2 | 4,000 XP → 200+ qualifying battles |

## What This Mod Does

1. **Fixes the timing bug**: Bypasses the vanilla handler and reads the strength ratio that was recorded *before* the battle resolved, when enemy strength was still intact.

2. **Fixes the win check**: The vanilla code runs the valor calculation on battle end regardless of outcome. This mod only awards valor when you actually **win**.

3. **Per-battle-type configuration**: Field battles, siege attacks, and siege defenses each have independent threshold and cap sliders — because storming a castle is nothing like defending one.

4. **Defaults to vanilla values**: Out of the box, the mod only fixes the bugs. Thresholds and XP remain at vanilla values until you change them.

5. **Hands off raids & hideouts**: Village raids and hideout battles are left to vanilla — they're not "significant" battles worth Valor.

## How It Works

Every battle type has two sliders:

| Slider | What it does |
|---|---|
| **Threshold** | The strength ratio where you **start** earning Valor (Min XP). |
| **Full Valor Ratio** | The strength ratio where you earn **maximum** Valor (Max XP). |

> **Strength ratio** = enemy strength ÷ your strength.
> A ratio of 3.0 means the enemy is 3× stronger. A ratio of 0.5 means **you** outnumber them 2:1.

Between threshold and cap, XP scales **linearly**. Below the threshold → nothing. Above the cap → max XP.

All XP is scaled by your party's **contribution rate** — in large army battles where allies do most of the fighting, you earn proportionally less.

## MCM Settings

Open **Mod Options → Valor Rebalanced** in the game menu. No restart required.

### Valor XP (global)

| Setting | Range | Default | Description |
|---|---|---|---|
| Min XP per Battle | 1–200 | 5 | XP at the threshold (barely qualifying). |
| Max XP per Battle | 1–1000 | 20 | XP at the cap (maximum difficulty). |

> **Leveling reference:** Valor level 1 = 1,000 XP. Level 2 = 4,000 XP.

### Per-Battle-Type Settings

Each has a **Threshold** and **Full Valor Ratio** slider (range 0.1–10.0).

| Battle Type | Applies When | Default Threshold | Default Cap |
|---|---|---|---|
| **Field Battle** | Open field, sally outs, siege outside, and other non-siege combat | 9.0 | 10.0 |
| **Siege Attack** | You are **storming** a castle or town | 9.0 | 10.0 |
| **Siege Defense** | You are **defending** a castle or town | 9.0 | 10.0 |

> **Important:** Threshold must be **less than** the Full Valor Ratio. If equal or reversed, no Valor is awarded for that type.

> Values **below 1.0** on the threshold mean you earn Valor even when **outnumbering** the enemy — useful for siege attacks where walls negate your advantage.

## Configuration Examples

### 🟢 Vanilla (default — mod only fixes bugs)

All thresholds = 9.0, all caps = 10.0, Min XP = 5, Max XP = 20.

You must be outnumbered 9:1 to earn 5 XP, and 10:1 for 20 XP. This is the base game but now actually functional.

---

### ⭐ Recommended: Balanced Rebalance

A good starting point that rewards skilled play without giving Valor away for free.

| Setting | Value |
|---|---|
| Min XP | 5 |
| Max XP | 500 |
| Field Battle — Threshold | 1.5 |
| Field Battle — Full Valor | 5.0 |
| Siege Attack — Threshold | 0.5 |
| Siege Attack — Full Valor | 1.5 |
| Siege Defense — Threshold | 3.0 |
| Siege Defense — Full Valor | 10.0 |

**What this feels like:**

#### ⚔️ Field Battle (threshold 1.5 → cap 5.0)

| Scenario | Ratio | Valor XP |
|---|---|---|
| You: 100 vs Enemy: 100 | 1.0× | — |
| You: 100 vs Enemy: 150 | 1.5× | 5 (min) |
| You: 100 vs Enemy: 250 | 2.5× | 148 |
| You: 100 vs Enemy: 350 | 3.5× | 360 |
| You: 100 vs Enemy: 500+ | 5.0×+ | **500** (max) |

Beating an army 5× your size in the open field is an incredible feat — this rewards it accordingly.

#### 🏰 Siege Attack (threshold 0.5 → cap 1.5)

Storming walls is brutal — you earn Valor even when you outnumber the garrison.

| Scenario | Ratio | Valor XP |
|---|---|---|
| You: 200 vs Garrison: 100 (2:1 advantage) | 0.5× | 5 (min) |
| You: 200 vs Garrison: 150 | 0.75× | 252 |
| You: 200 vs Garrison: 200 (equal) | 1.0× | 500 (max!) |
| You: 100 vs Garrison: 150+ | 1.5×+ | **500** (max) |

Even with a 2:1 numbers advantage, storming a castle is a meat grinder — this config acknowledges that.

#### 🛡️ Siege Defense (threshold 3.0 → cap 10.0)

Walls help a lot, so you need much bigger odds to earn Valor.

| Scenario | Ratio | Valor XP |
|---|---|---|
| You: 100 vs Attackers: 200 | 2.0× | — |
| You: 100 vs Attackers: 300 | 3.0× | 5 (min) |
| You: 100 vs Attackers: 500 | 5.0× | 147 |
| You: 100 vs Attackers: 800 | 8.0× | 360 |
| You: 100 vs Attackers: 1000+ | 10.0×+ | **500** (max) |

With the Recommended config, reaching Valor level 1 (1,000 XP) takes roughly **2–10 qualifying battles** depending on difficulty, and level 2 (4,000 XP) takes **8–40 battles**.

---

### 🟡 Generous: Easy Valor

For players who want Valor to grow naturally without needing extreme odds.

| Setting | Value |
|---|---|
| Min XP | 10 |
| Max XP | 200 |
| Field Battle — Threshold | 1.0 |
| Field Battle — Full Valor | 3.0 |
| Siege Attack — Threshold | 0.3 |
| Siege Attack — Full Valor | 1.0 |
| Siege Defense — Threshold | 2.0 |
| Siege Defense — Full Valor | 5.0 |

---

### 🔴 Hardcore: Earn Every Point

For players who want Valor to feel like a real achievement.

| Setting | Value |
|---|---|
| Min XP | 5 |
| Max XP | 100 |
| Field Battle — Threshold | 3.0 |
| Field Battle — Full Valor | 8.0 |
| Siege Attack — Threshold | 1.0 |
| Siege Attack — Full Valor | 3.0 |
| Siege Defense — Threshold | 5.0 |
| Siege Defense — Full Valor | 10.0 |

## Requirements

- Bannerlord **1.3.15**
- [Bannerlord Harmony](https://www.nexusmods.com/mountandblade2bannerlord/mods/2006)
- [Mod Configuration Menu (MCM)](https://www.nexusmods.com/mountandblade2bannerlord/mods/612)

## Installation

1. Install the requirements above if you haven't already.
2. Extract the `Bannerlord.ValorRebalanced` folder into your game's `Modules` directory:
   ```
   Modules\Bannerlord.ValorRebalanced\
       SubModule.xml
       bin\Win64_Shipping_Client\Bannerlord.ValorRebalanced.dll
   ```
3. Enable **Valor Rebalanced** in the launcher. Make sure it loads after Harmony and MCM.
4. Adjust settings in **Mod Options** in-game. No restart required.

## Compatibility

- **Game version**: 1.3.x
- **Save-safe**: Can be added or removed mid-playthrough.
- Works with other mods that don't also patch `PlayerVariablesBehavior.OnPlayerBattleEnd`.

## Building from Source

```
dotnet build -c Release -p:Platform=x64
```

The compiled DLL will be in `Bannerlord.ValorRebalanced\bin\x64\Release\net472\`.

## Credits

Based on [Fix Valor](https://github.com/ivokosir/Bannerlord.FixValor) by ivokosir.