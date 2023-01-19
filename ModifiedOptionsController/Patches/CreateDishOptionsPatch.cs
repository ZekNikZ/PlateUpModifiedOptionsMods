using HarmonyLib;
using Kitchen;
using KitchenData;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ModifiedOptionsController.Patches
{
    [HarmonyPatch(typeof(CreateDishOptions), "Initialise")]
    class CreateDishOptionsInitializePatch
    {
        internal static EntityQuery DishUpgrades;
        internal static EntityQuery DishSizeUpgrades;

        [HarmonyPostfix]
        static void Postfix(CreateDishOptions __instance)
        {
            DishUpgrades = __instance.EntityManager.CreateEntityQuery(new QueryHelper()
                .All(typeof(CDishUpgrade)));
            DishSizeUpgrades = __instance.EntityManager.CreateEntityQuery(new QueryHelper()
                .All(typeof(CUpgradeExtraDish)));
        }
    }

    [HarmonyPatch(typeof(CreateDishOptions), "OnUpdate")]
    class CreateDishOptionsOnUpdatePatch
    {
        [HarmonyPrefix]
        static bool Prefix(CreateDishOptions __instance)
        {
            MethodInfo mInfo = typeof(CreateDishOptions).GetMethod("CreateFoodSource", BindingFlags.NonPublic | BindingFlags.Instance);

            int extraDishOptions = 0;

            List<Vector3> positions = new()
            {
                new Vector3(2f, 0f, -7f),
                new Vector3(3f, 0f, -7f),
                new Vector3(4f, 0f, -7f),
                new Vector3(4f, 0f, -6f)
            };
            List<Vector3> extraPositions = new()
            {
            };

            if (ModifiedOptionsManager.AddExtraDishOptions || AssetReference.AlwaysAvailableDish != 0)
            {
                if (ModifiedOptionsManager.IsSeedExplorerInstalled)
                {
                    extraPositions.Add(new Vector3(4f, 0f, -5f));
                    extraPositions.Add(new Vector3(4f, 0f, -4f));
                }
                else
                {
                    extraPositions.Add(new Vector3(4f, 0f, -5f));
                    extraPositions.Add(new Vector3(1f, 0f, -7f));
                }

                var level = __instance.GetSingleton<SPlayerLevel>().Level;
                if (level >= 2 || AssetReference.AlwaysAvailableDish != 0)
                {
                    extraDishOptions += 1;
                }
                if (level >= 8)
                {
                    extraDishOptions += 1;
                }
            }

            NativeArray<CDishUpgrade> nativeArray = CreateDishOptionsInitializePatch.DishUpgrades.ToComponentDataArray<CDishUpgrade>(Allocator.Temp);
            List<CDishUpgrade> dishOptions = Kitchen.RandomExtensions.Shuffle(nativeArray.ToList());

            if (ModifiedOptionsManager.PreferModdedDishes)
            {
                dishOptions = dishOptions
                    .GroupBy(cdu => cdu.DishID)
                    .Select(g => g.First())
                    .OrderBy(cdu => Utils.IsModded(cdu.DishID) ? 0 : 1)
                    .ToList();
            }

            // Main set
            int i;
            for (i = 0; i < Mathf.Min(4, 1 + CreateDishOptionsInitializePatch.DishSizeUpgrades.CalculateEntityCount()); i++)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { positions[i], (i < dishOptions.Count()) ? output : null, false });
                }
            }

            // Extra dish 1 / special event dish
            if (AssetReference.AlwaysAvailableDish != 0)
            {
                mInfo.Invoke(__instance, new object[] { extraPositions[0], GameData.Main.Get<Dish>(AssetReference.AlwaysAvailableDish), false });
            }
            else if (extraDishOptions >= 1)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i++].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { extraPositions[1], (i < dishOptions.Count()) ? output : null, false });
                }
            }

            // Extra dish 2
            if (extraDishOptions >= 1)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i++].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { extraPositions[1], (i < dishOptions.Count()) ? output : null, false });
                }
            }

            return false;
        }
    }
}
