using HarmonyLib;
using KitchenData;
using System.Collections.Generic;

namespace ModifiedOptionsController.Patches
{
    [HarmonyPatch(typeof(GameDataConstructor), "BuildGameData")]
    class GameDataPatch
    {
        static void Prefix(GameDataConstructor __instance)
        {
            if (ModifiedOptionsManager.GDOCache == null)
            {
                ModifiedOptionsManager.GDOCache = new HashSet<int>();
                foreach (var gdo in __instance.GameDataObjects)
                {
                    ModifiedOptionsManager.GDOCache.Add(gdo.ID);
                }
            }
            
        }
    }
}
