using HarmonyLib;
using KitchenData;
using PreferenceSystem.Utils;
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
            if (ModifiedOptionsManager.ModdedCardPercentage == 0 && ModifiedOptionsManager.AddonCardPercentage == __instance.PriorityProbability)
            {
                return true;
            }

            var mIsPriority = ReflectionUtils.GetMethod<UnlockSorterPriority>("IsPriority");

            bool isPriority = UnityEngine.Random.value < ModifiedOptionsManager.AddonCardPercentage;
            bool isModdedPriority = UnityEngine.Random.value < ModifiedOptionsManager.ModdedCardPercentage;
            candidates = candidates.OrderByDescending((Unlock u) => ((isPriority && (bool)mIsPriority.Invoke(__instance, new object[] { u })) ? 1 : 0) + ((isModdedPriority && Utils.IsModded(u)) ? 5 : 0)).ToList();

            return false;
        }
    }
}
