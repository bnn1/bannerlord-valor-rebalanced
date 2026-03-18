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

        [SettingPropertyFloatingInteger(
            "Strength Ratio Threshold",
            0.5f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "Minimum enemy-to-player strength ratio before you earn valor. " +
                       "Below 1.0 = earn valor even when you outnumber the enemy. " +
                       "9.0 = vanilla. [Default: 9.0]")]
        [SettingPropertyGroup("Valor XP")]
        public float StrengthRatioThreshold { get; set; } = 9.0f;

        [SettingPropertyInteger(
            "Min XP per Battle",
            1, 200,
            "0",
            Order = 1,
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
            Order = 2,
            RequireRestart = false,
            HintText = "Valor XP when the strength ratio reaches the battle-type cap. " +
                       "Scaled by your party's contribution. " +
                       "For reference: level 1 = 1000 XP, level 2 = 4000 XP. [Default: 20]")]
        [SettingPropertyGroup("Valor XP")]
        public int MaxXp { get; set; } = 20;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Field Battle",
            0.5f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "Strength ratio for max valor in open field battles. " +
                       "Also covers sally outs and other non-siege encounters. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Battle Type Caps")]
        public float MaxRatioFieldBattle { get; set; } = 10.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Siege Attack",
            0.5f, 10.0f,
            "0.0",
            Order = 1,
            RequireRestart = false,
            HintText = "Strength ratio for max valor when YOU assault a fortification. " +
                       "Storming walls is brutal — values below 1.0 mean you earn max valor " +
                       "even when outnumbering defenders. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Battle Type Caps")]
        public float MaxRatioSiegeAttack { get; set; } = 10.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Siege Defense",
            0.5f, 10.0f,
            "0.0",
            Order = 2,
            RequireRestart = false,
            HintText = "Strength ratio for max valor when YOU defend a fortification. " +
                       "Walls give a large advantage so a higher ratio is expected. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Battle Type Caps")]
        public float MaxRatioSiegeDefense { get; set; } = 10.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Village Raid",
            0.5f, 10.0f,
            "0.0",
            Order = 3,
            RequireRestart = false,
            HintText = "Strength ratio for max valor during village raids. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Battle Type Caps")]
        public float MaxRatioRaid { get; set; } = 10.0f;

        [SettingPropertyFloatingInteger(
            "Full Valor Ratio — Hideout",
            0.5f, 10.0f,
            "0.0",
            Order = 4,
            RequireRestart = false,
            HintText = "Strength ratio for max valor in hideout battles. " +
                       "Limited party size makes these challenging. [Default: 10.0]")]
        [SettingPropertyGroup("Valor XP/Battle Type Caps")]
        public float MaxRatioHideout { get; set; } = 10.0f;
    }
}
