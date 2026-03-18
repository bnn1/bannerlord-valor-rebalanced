using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.FixValor
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("Bannerlord.FixValor");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var target = AccessTools2.DeclaredMethod(
                "TaleWorlds.CampaignSystem.CampaignBehaviors.PlayerVariablesBehavior:OnPlayerBattleEnd");

            if (target == null)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[FixValor] ERROR: Could not find OnPlayerBattleEnd method!"));
                return;
            }

            var prefix = AccessTools2.DeclaredMethod(typeof(SubModule), nameof(PrefixOnPlayerBattleEnd));

            bool patched = Harmony.TryPatch(target, prefix: prefix);

            InformationManager.DisplayMessage(
                new InformationMessage(patched
                    ? "[FixValor] Patch applied successfully."
                    : "[FixValor] ERROR: Patch failed to apply!"));
        }

        private static bool PrefixOnPlayerBattleEnd(MapEvent mapEvent)
        {
            if (mapEvent == null)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[FixValor] Prefix fired but mapEvent is null."));
                return false;
            }

            MapEventSide playerSide = mapEvent.Winner;
            if (playerSide == null)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[FixValor] Prefix fired but Winner is null."));
                return false;
            }

            if (!playerSide.IsMainPartyAmongParties())
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[FixValor] Prefix fired but main party not among winner's parties."));
                return false;
            }

            float contribution = playerSide.GetPlayerPartyContributionRate();
            int playerTroops = playerSide.HealthyTroopCountAtMapEventStart;
            int enemyTroops = playerSide.OtherSide.HealthyTroopCountAtMapEventStart;

            InformationManager.DisplayMessage(
                new InformationMessage(
                    $"[FixValor] Calling OnBattleWon — contribution: {contribution:F2}, " +
                    $"player troops: {playerTroops}, enemy troops: {enemyTroops}"));

            TraitLevelingHelper.OnBattleWon(mapEvent, contribution);
            return false;
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }
    }
}
