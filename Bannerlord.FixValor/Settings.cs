using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace Bannerlord.FixValor
{
    internal sealed class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "Bannerlord.FixValor_v1";
        public override string DisplayName => "Valor Rebalanced";
        public override string FolderName => "Bannerlord.FixValor";
        public override string FormatType => "json";

        [SettingPropertyFloatingInteger(
            "Strength Ratio Threshold",
            1.0f, 10.0f,
            "0.0",
            Order = 0,
            RequireRestart = false,
            HintText = "How much stronger the enemy must be before you earn valor. " +
                       "1.5 = enemy 50%% stronger, 3.0 = 3x stronger, 9.0 = vanilla. " +
                       "Lower = easier to earn valor. [Default: 9.0]")]
        [SettingPropertyGroup("Valor XP")]
        public float StrengthRatioThreshold { get; set; } = 9.0f;

        [SettingPropertyInteger(
            "Min XP per Battle",
            1, 200,
            "0",
            Order = 1,
            RequireRestart = false,
            HintText = "Valor XP at the threshold ratio (barely outnumbered). " +
                       "Scaled by your party's contribution. " +
                       "For reference: level 1 = 1000 XP, level 2 = 4000 XP. [Default: 5]")]
        [SettingPropertyGroup("Valor XP")]
        public int MinXp { get; set; } = 5;

        [SettingPropertyInteger(
            "Max XP per Battle",
            1, 500,
            "0",
            Order = 2,
            RequireRestart = false,
            HintText = "Valor XP when massively outnumbered (ratio 10x, the game's cap). " +
                       "Scaled by your party's contribution. " +
                       "For reference: level 1 = 1000 XP, level 2 = 4000 XP. [Default: 20]")]
        [SettingPropertyGroup("Valor XP")]
        public int MaxXp { get; set; } = 20;
    }
}
