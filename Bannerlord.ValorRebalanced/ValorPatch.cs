using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

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
            float threshold = settings?.StrengthRatioThreshold ?? 9.0f;
            float minXp = settings?.MinXp ?? 5;
            float maxXp = settings?.MaxXp ?? 20;
            float typeCap = GetBattleTypeCap(mapEvent, settings);

            // Use the lower of threshold/cap as the start and the higher as the
            // end.  This lets battle types whose cap is below the threshold
            // (e.g. siege attack at 0.8) still grant valor at low ratios.
            float rangeStart = Math.Min(threshold, typeCap);
            float rangeEnd = Math.Max(threshold, typeCap);

            if (strengthRatio > rangeStart)
            {
                float t = rangeEnd > rangeStart
                    ? Math.Min(1f, (strengthRatio - rangeStart) / (rangeEnd - rangeStart))
                    : 1f;
                int valorXp = Math.Min(
                    (int)((minXp + t * (maxXp - minXp)) * contribution),
                    (int)(maxXp * contribution));

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

        private static float GetBattleTypeCap(MapEvent mapEvent, Settings settings)
        {
            if (mapEvent.IsSiegeAssault)
            {
                bool playerIsAttacker =
                    PlayerEncounter.Current.PlayerSide == BattleSideEnum.Attacker;
                return playerIsAttacker
                    ? (settings?.MaxRatioSiegeAttack ?? 10f)
                    : (settings?.MaxRatioSiegeDefense ?? 10f);
            }

            if (mapEvent.IsRaid)
                return settings?.MaxRatioRaid ?? 10f;

            if (mapEvent.IsHideoutBattle)
                return settings?.MaxRatioHideout ?? 10f;

            // Field battle, sally out, siege outside, and everything else.
            return settings?.MaxRatioFieldBattle ?? 10f;
        }
    }
}
