using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Kitchen;
using Unity.Entities;
using UnityEngine;

namespace ModifiedOptionsController.Patches
{
    [HarmonyPatch(typeof(CreateLayoutSlots), "Initialise")]
    class CreateLayoutSlotsInitializePatch
    {
        internal static EntityQuery LayoutSizeUpgrades;

        [HarmonyPostfix]
        static void Postfix(CreateLayoutSlots __instance)
        {
            LayoutSizeUpgrades = __instance.EntityManager.CreateEntityQuery(new QueryHelper()
                .All(typeof(CUpgradeExtraLayout)));
        }
    }

    [HarmonyPatch(typeof(CreateLayoutSlots), "OnUpdate")]
    class CreateLayoutSlotsOnUpdatePatch
    {
        [HarmonyPrefix]
        static bool Prefix(CreateLayoutSlots __instance)
        {
            if (!ModifiedOptionsManager.AddExtraLayoutOptions || ModifiedOptionsManager.IsSeedExplorerInstalled)
            {
                return true;
            }

            MethodInfo mInfo = typeof(CreateLayoutSlots).GetMethod("CreateMapSource", BindingFlags.NonPublic | BindingFlags.Instance);

            List<Vector3> positions = new()
            {
                new Vector3(-2f, 0f, -7f),
                new Vector3(-3f, 0f, -7f),
                new Vector3(-4f, 0f, -7f),
                new Vector3(-4f, 0f, -6f),
                new Vector3(-1f, 0f, -7f),
                new Vector3(0f, 0f, -7f)
            };
            for (int i = 0; i < Mathf.Min(4, 2 + CreateLayoutSlotsInitializePatch.LayoutSizeUpgrades.CalculateEntityCount()) + 2; i++)
            {
                mInfo.Invoke(__instance, new object[] { positions[i] });
            }

            return false;
        }
    }
}
