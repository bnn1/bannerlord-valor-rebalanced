using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.ValorRebalanced
{
    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("Bannerlord.ValorRebalanced");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ValorPatch.TryResolve())
            {
                ShowMessage("[ValorRebalanced] ERROR: Could not resolve AddPlayerTraitXPAndLogEntry.");
                return;
            }

            var target = AccessTools2.DeclaredMethod(
                "TaleWorlds.CampaignSystem.CampaignBehaviors.PlayerVariablesBehavior:OnPlayerBattleEnd");

            if (target == null)
            {
                ShowMessage("[ValorRebalanced] ERROR: Could not find OnPlayerBattleEnd.");
                return;
            }

            var prefix = AccessTools2.DeclaredMethod(typeof(ValorPatch), nameof(ValorPatch.PrefixOnPlayerBattleEnd));
            bool patched = Harmony.TryPatch(target, prefix: prefix);

            ShowMessage(patched
                ? "[ValorRebalanced] Patch applied successfully."
                : "[ValorRebalanced] ERROR: Patch failed to apply.");
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        private static void ShowMessage(string text)
        {
            InformationManager.DisplayMessage(new InformationMessage(text));
        }
    }
}
