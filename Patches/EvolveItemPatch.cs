using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace HideDryTimeMod.Patches
{
    /// <summary>
    /// Fresh hides, pelts, gut and saplings use EvolveItem; drying duration is stored as game days.
    /// One in-game day = 24 hours on the survival clock.
    /// </summary>
    [HarmonyPatch(typeof(EvolveItem), nameof(EvolveItem.Awake))]
    internal static class EvolveItem_Awake_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(EvolveItem __instance)
        {
            DryTimeApplier.Apply(__instance);
        }
    }

    [HarmonyPatch(typeof(EvolveItem), nameof(EvolveItem.Deserialize))]
    internal static class EvolveItem_Deserialize_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(EvolveItem __instance)
        {
            DryTimeApplier.Apply(__instance);
        }
    }

    internal static class DryTimeApplier
    {
        private const float HoursPerGameDay = 24f;

        internal static void Apply(EvolveItem evolveItem)
        {
            if (evolveItem == null)
                return;

            if (Settings.UseVanilla())
                return;

            float hours = Settings.GetDryTimeHours();
            evolveItem.m_TimeToEvolveGameDays = hours / HoursPerGameDay;
        }
    }
}
