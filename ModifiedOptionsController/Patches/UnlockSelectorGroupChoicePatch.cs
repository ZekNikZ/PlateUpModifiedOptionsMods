using HarmonyLib;
using KitchenData;
using System.Collections.Generic;

namespace ModifiedOptionsController.Patches
{
	/// <summary>
	/// Fixes a bug in the vanilla game where the first unlock card in the 'canditates' list is never selected.
	/// </summary>
    [HarmonyPatch(typeof(UnlockSelectorGroupChoice), "GetOptions")]
    class UnlockSelectorGroupChoicePatch
    {
        [HarmonyPostfix]
        static void Postfix(List<Unlock> candidates, HashSet<int> current_cards, UnlockRequest request, UnlockSelectorGroupChoice __instance, ref UnlockOptions __result)
        {
			UnlockOptions result = default;
			foreach (Unlock candidate in candidates)
			{
				if (candidate.UnlockGroup == __instance.Group1 || candidate.UnlockGroup == __instance.Group2)
				{
					if (result.Unlock1 == null || (candidate.UnlockGroup == __instance.Group1 && result.Unlock1.UnlockGroup != __instance.Group1))
					{
						result.Unlock1 = candidate;
					}
					if (result.Unlock2 == null || (candidate.UnlockGroup == __instance.Group2 && result.Unlock2.UnlockGroup != __instance.Group2))
					{
						result.Unlock2 = candidate;
					}
				}
			}
			__result = result;
		}
    }
}
