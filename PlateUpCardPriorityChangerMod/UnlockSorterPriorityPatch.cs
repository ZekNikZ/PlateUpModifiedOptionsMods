using HarmonyLib;
using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenPreferModdedOptionsMod.Patches
{
    /// <summary>
    /// For priority sorting unlock selecting, prioritizes modded cards.
    /// </summary>
    [HarmonyPatch(typeof(UnlockSorterPriority), "SortCards")]
    class UnlockSorterPriorityPatch
    {
        private static readonly Dictionary<DishType, string> DISH_TYPE_TO_PREF_KEY = new()
        {
            { DishType.Base, Mod.PREF_WEIGHT_MAINS },
            { DishType.Main, Mod.PREF_WEIGHT_MAINS },
            { DishType.Side, Mod.PREF_WEIGHT_SIDES },
            { DishType.Starter, Mod.PREF_WEIGHT_STARTERS },
            { DishType.Dessert, Mod.PREF_WEIGHT_DESSERTS },
            { DishType.Extra, Mod.PREF_WEIGHT_ADDONS },
            { DishType.Null, Mod.PREF_WEIGHT_ADDONS },
        };

        private static float ComputeWeight(Unlock unlock)
        {
            float result;

            // Weight
            if (unlock is Dish dish)
            {
                result = Mod.PreferenceManager.Get<int>(DISH_TYPE_TO_PREF_KEY[dish.Type]);
            }
            else
            {
                result = Mod.PreferenceManager.Get<int>(Mod.PREF_WEIGHT_CUSTOMERS);
            }

            // Multiplier
            if (Utils.IsModded(unlock.ID))
            {
                result *= Mod.PreferenceManager.Get<float>(Mod.PREF_MULTIPLIER_MODDED);
            }
            else
            {
                result *= Mod.PreferenceManager.Get<float>(Mod.PREF_MULTIPLIER_VANILLA);
            }

            return result;
        }

        [HarmonyPrefix]
        static bool Prefix(ref List<Unlock> candidates, HashSet<int> current_cards, UnlockRequest request, UnlockSorterPriority __instance)
        {
            switch(Mod.PreferenceManager.Get<string>(Mod.PREF_WEIGHT_BEHAVIOR))
            {
                case "RANDOM":
                    candidates = candidates.OrderByDescending((Unlock u) => Mathf.Pow(Random.value, 1f / ComputeWeight(u))).ToList();
                    return false;
                case "STRICT":
                    candidates = candidates.OrderByDescending((Unlock u) => ComputeWeight(u)).ToList();
                    return false;
                case "DISABLED":
                default:
                    return true;
            }
        }
    }
}
