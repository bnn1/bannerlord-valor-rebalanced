using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace Bannerlord.ValorRebalanced
{
    internal sealed class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "Bannerlord.ValorRebalanced_v1";
        public override string DisplayName => "Valor Rebalanced";
        public override string FolderName => "Bannerlord.ValorRebalanced";
        public override string FormatType => "json";

        [SettingPropertyInteger(
            "Min XP per Battle",
            1, 200,
            "0",
            Order = 0,
            RequireRestart = false,
            HintText = "Valor XP at the threshold ratio (barely qualifying). " +
                       "Scaled by your party's contribution. " +
                       "For reference: level 1 = 1000 XP, level 2 = 4000 XP. [Default: 5]")]
        [SettingPropertyGroup("Valor XP")]
        public int MinXp { get; set; } = 5;

        [SettingPropertyInteger(
            "Max XP per Battle",
            1, 1000,
            "0",
            Order = 1,
            RequireRestart = false,
            HintText = "Valor XP when the strength ratio reaches the battle-type cap. " +
                       "Scaled by your party's contribution. " +
                       "For reference: level 1 = 1000 XP, level 2 = 4000 XP. [Default: 20]")]
        [SettingPropertyGroup("Valor XP")]
        public int MaxXp { get; set; } = 20;

        // ── Field Battle ────────────────────────────────────────────────

        [SettingPropertyFloatingInteger(
            "Threshold — Field Battle",
            0.1f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "Strength ratio where you START earning valor in field battles. " +
                       "Also covers sally outs and other non-siege encounters. " +
                       "9.0 = vanilla. [Default: 9.0]")]
        [SettingPropertyGroup("Valor XP/Field Battle")]
        public float ThresholdFieldBattle { get; set; } = 9.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Field Battle",
            0.1f, 10.0f,
            "0.0",
            Order = 1,
            RequireRestart = false,
            HintText = "Strength ratio where you earn MAX valor in field battles. " +
                       "Must be greater than the threshold. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Field Battle")]
        public float CapFieldBattle { get; set; } = 10.0f;

        // ── Siege Attack ────────────────────────────────────────────────

        [SettingPropertyFloatingInteger(
            "Threshold — Siege Attack",
            0.1f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "Strength ratio where you START earning valor when assaulting a fortification. " +
                       "Values below 1.0 let you earn valor even when outnumbering defenders. " +
                       "9.0 = vanilla. [Default: 9.0]")]
        [SettingPropertyGroup("Valor XP/Siege Attack")]
        public float ThresholdSiegeAttack { get; set; } = 9.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Siege Attack",
            0.1f, 10.0f,
            "0.0",
            Order = 1,
            RequireRestart = false,
            HintText = "Strength ratio where you earn MAX valor when assaulting a fortification. " +
                       "Must be greater than the threshold. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Siege Attack")]
        public float CapSiegeAttack { get; set; } = 10.0f;

        // ── Siege Defense ───────────────────────────────────────────────

        [SettingPropertyFloatingInteger(
            "Threshold — Siege Defense",
            0.1f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "Strength ratio where you START earning valor when defending a fortification. " +
                       "9.0 = vanilla. [Default: 9.0]")]
        [SettingPropertyGroup("Valor XP/Siege Defense")]
        public float ThresholdSiegeDefense { get; set; } = 9.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Siege Defense",
            0.1f, 10.0f,
            "0.0",
            Order = 1,
            RequireRestart = false,
            HintText = "Strength ratio where you earn MAX valor when defending a fortification. " +
                       "Walls give a large advantage so a higher ratio is expected. " +
                       "Must be greater than the threshold. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Siege Defense")]
        public float CapSiegeDefense { get; set; } = 10.0f;
    }
}
