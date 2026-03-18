using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;

namespace Bannerlord.ValorRebalanced
{
    internal static class ValorPatch
    {
        private static MethodInfo _addTraitXpMethod;

        public static bool TryResolve()
        {
            _addTraitXpMethod = AccessTools.DeclaredMethod(
                typeof(TraitLevelingHelper), "AddPlayerTraitXPAndLogEntry");

            return _addTraitXpMethod != null;
        }

        public static bool PrefixOnPlayerBattleEnd(MapEvent mapEvent)
        {
            if (mapEvent == null)
                return false;

            MapEventSide playerSide = mapEvent.Winner;
            if (playerSide == null || !playerSide.IsMainPartyAmongParties())
                return false;

            float contribution = playerSide.GetPlayerPartyContributionRate();

            float strengthRatio = mapEvent
                .GetMapEventSide(PlayerEncounter.Current.PlayerSide)
                .StrengthRatio;

            var settings = Settings.Instance;
            float minRatio = settings?.StrengthRatioThreshold ?? 9.0f;
            const float MaxRatio = 10f; // game's internal cap
            float minXp = settings?.MinXp ?? 5;
            float maxXp = settings?.MaxXp ?? 20;
            bool exponential = settings?.UseExponentialCurve ?? false;

            if (strengthRatio > minRatio)
            {
                float t = Math.Min(1f, (strengthRatio - minRatio) / (MaxRatio - minRatio));

                // Linear: XP scales evenly between min and max.
                //
                // Exponential (t²): XP is back-loaded — easy wins give near-minimum,
                // hard victories give disproportionately more. The user's "Max XP" slider
                // stays meaningful: it controls the midpoint reference. Internally we
                // scale the ceiling so that t=1 gives 2× the slider's max, keeping the
                // midpoint (t≈0.7) close to the slider value.
                //
                // With threshold=1.5, min=10, max=50:
                //   Linear:      2x→12  4x→22  8x→41  10x→50
                //   Exponential: 2x→10  4x→13  8x→55  10x→90
                float curved = exponential ? t * t : t;
                float effectiveMax = exponential ? minXp + (maxXp - minXp) * 2f : maxXp;
                int valorXp = (int)((minXp + curved * (effectiveMax - minXp)) * contribution);

                if (valorXp > 0 && _addTraitXpMethod != null)
                {
                    _addTraitXpMethod.Invoke(null, new object[]
                    {
                        DefaultTraits.Valor,
                        valorXp,
                        ActionNotes.BattleValor,
                        null!
                    });
                }
            }

            return false;
        }
    }
}
