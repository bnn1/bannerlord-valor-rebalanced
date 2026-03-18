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

            // Raids, hideouts, and other minor encounters → let vanilla handle them.
            if (mapEvent.IsRaid || mapEvent.IsHideoutBattle)
                return true;

            MapEventSide playerSide = mapEvent.Winner;
            if (playerSide == null || !playerSide.IsMainPartyAmongParties())
                return false;

            float contribution = playerSide.GetPlayerPartyContributionRate();

            float strengthRatio = mapEvent
                .GetMapEventSide(PlayerEncounter.Current.PlayerSide)
                .StrengthRatio;

            var settings = Settings.Instance;
            float minXp = settings?.MinXp ?? 5;
            float maxXp = settings?.MaxXp ?? 20;

            GetBattleTypeRange(mapEvent, settings, out float threshold, out float cap);

            // Misconfiguration guard: threshold must be less than cap.
            if (threshold >= cap)
                return false;

            if (strengthRatio > threshold)
            {
                float t = Math.Min(1f, (strengthRatio - threshold) / (cap - threshold));
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

        private static void GetBattleTypeRange(
            MapEvent mapEvent, Settings settings,
            out float threshold, out float cap)
        {
            if (mapEvent.IsSiegeAssault)
            {
                bool playerIsAttacker =
                    PlayerEncounter.Current.PlayerSide == BattleSideEnum.Attacker;

                if (playerIsAttacker)
                {
                    threshold = settings?.ThresholdSiegeAttack ?? 9f;
                    cap = settings?.CapSiegeAttack ?? 10f;
                }
                else
                {
                    threshold = settings?.ThresholdSiegeDefense ?? 9f;
                    cap = settings?.CapSiegeDefense ?? 10f;
                }
                return;
            }

            // Field battle, sally out, siege outside, and everything else.
            threshold = settings?.ThresholdFieldBattle ?? 9f;
            cap = settings?.CapFieldBattle ?? 10f;
        }
    }
}
