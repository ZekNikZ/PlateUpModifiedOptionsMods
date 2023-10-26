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

            Vector3 office = LobbyPositionAnchors.Office;
            List<Vector3> positions = new()
            {
                new Vector3(2f, 0f, -5f),
                new Vector3(3f, 0f, -5f),
                new Vector3(4f, 0f, -5f),
                new Vector3(4f, 0f, -4f),
                new Vector3(4f, 0f, -3f),
                new Vector3(1f, 0f, -5f)
            };
            List<Vector3> extraPositions = new()
            {
            };

            if (ModifiedOptionsManager.ExtraDishOptionsCount > 0 || AssetReference.AlwaysAvailableDish != 0)
            {
                extraPositions.Add(new Vector3(0f, 0f, -5f));
                extraPositions.Add(new Vector3(4f, 0f, -1f));

                var level = __instance.GetSingleton<SPlayerLevel>().Level;
                if ((ModifiedOptionsManager.ExtraDishOptionsCount >= 1 && level >= 13) || AssetReference.AlwaysAvailableDish != 0)
                {
                    extraDishOptions += 1;
                }
                if (ModifiedOptionsManager.ExtraDishOptionsCount >= 2 && level >= 15)
                {
                    extraDishOptions += 1;
                }
            }

            NativeArray<CDishUpgrade> nativeArray = CreateDishOptionsInitializePatch.DishUpgrades.ToComponentDataArray<CDishUpgrade>(Allocator.Temp);
            List<CDishUpgrade> dishOptions = Kitchen.RandomExtensions.Shuffle(nativeArray.ToList());
            dishOptions = dishOptions
                .GroupBy(cdu => cdu.DishID)
                .Select(g => g.First())
                .ToList();

            int baseDishCount = Mathf.Min(positions.Count, 1 + CreateDishOptionsInitializePatch.DishSizeUpgrades.CalculateEntityCount());
            int totalDishCount = baseDishCount + extraDishOptions - (AssetReference.AlwaysAvailableDish != 0 ? 1 : 0);

            if (ModifiedOptionsManager.ModdedDishPercentage > 0)
            {
                var numModdedDishes = Mathf.FloorToInt(totalDishCount * ModifiedOptionsManager.ModdedDishPercentage);
                var numVanillaDishes = totalDishCount - numModdedDishes;

                var moddedDishes = dishOptions
                    .Where(cdu => Utils.IsModded(cdu.DishID))
                    .Take(numModdedDishes)
                    .ToList();
                var vanillaDishes = dishOptions
                    .Where(cdu => !Utils.IsModded(cdu.DishID))
                    .Take(numVanillaDishes + (numModdedDishes - moddedDishes.Count))
                    .ToList();

                dishOptions = Kitchen.RandomExtensions.Shuffle(moddedDishes.Concat(vanillaDishes).ToList());
            }

            // Main set
            int i;
            for (i = 0; i < baseDishCount; i++)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { office + positions[i], (i < dishOptions.Count()) ? output : null, false });
                }
            }

            // Extra dish 1 / special event dish
            if (AssetReference.AlwaysAvailableDish != 0)
            {
                mInfo.Invoke(__instance, new object[] { extraPositions[0], GameData.Main.Get<Dish>(AssetReference.AlwaysAvailableDish), false });
            }
            else if (extraDishOptions >= 1)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { office + extraPositions[0], (i < dishOptions.Count()) ? output : null, false });
                }
                ++i;
            }

            // Extra dish 2
            if (extraDishOptions >= 2)
            {
                if (GameData.Main.TryGet<Dish>(dishOptions[i].DishID, out var output, warn_if_fail: true))
                {
                    mInfo.Invoke(__instance, new object[] { office + extraPositions[1], (i < dishOptions.Count()) ? output : null, false });
                }
                ++i;
            }

            return false;
        }
    }
}
