using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ModifiedOptionsController.Patches
{
    /// <summary>
    /// For priority sorting unlock selecting, prioritizes modded cards.
    /// </summary>
    [HarmonyPatch(typeof(UnlockSorterPriority), "SortCards")]
    class UnlockSorterPriorityPatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref List<Unlock> candidates, HashSet<int> current_cards, UnlockRequest request, UnlockSorterPriority __instance)
        {
            if (!ModifiedOptionsManager.PreferModdedCards)
            {
                return true;
            }

            var mIsPriority = ReflectionUtils.GetMethod<UnlockSorterPriority>("IsPriority");

            bool is_priority = UnityEngine.Random.value < __instance.PriorityProbability;
            candidates = candidates.OrderByDescending((Unlock u) => ((is_priority && (bool)mIsPriority.Invoke(__instance, new object[] { u })) ? 1 : 0) + (Utils.IsModded(u) ? 5 : 0)).ToList();

            return false;
        }
    }
}
