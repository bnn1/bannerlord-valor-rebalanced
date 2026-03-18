using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;

namespace Bannerlord.FixValor
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

            if (strengthRatio > minRatio)
            {
                float t = Math.Min(1f, (strengthRatio - minRatio) / (MaxRatio - minRatio));
                int valorXp = (int)((minXp + t * (maxXp - minXp)) * contribution);

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
