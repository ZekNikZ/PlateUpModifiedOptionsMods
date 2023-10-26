using HarmonyLib;
using KitchenData;
using PreferenceSystem;
using System;
using System.Collections.Generic;

namespace KitchenPreferModdedOptionsMod
{
    internal static class Utils
    {

        private static HashSet<int> GDOCache;

        [HarmonyPatch(typeof(GameDataConstructor), "BuildGameData")]
        class GameDataPatch
        {
            static void Prefix(GameDataConstructor __instance)
            {
                if (GDOCache == null)
                {
                    GDOCache = new HashSet<int>();
                    foreach (var gdo in __instance.GameDataObjects)
                    {
                        GDOCache.Add(gdo.ID);
                    }
                }

            }
        }

        public static bool IsModded(int id)
        {
            return !GDOCache.Contains(id);
        }

        public static bool IsModded(GameDataObject gdo)
        {
            return IsModded(gdo.ID);
        }

        public static PreferenceSystemManager AddOption<T>(this PreferenceSystemManager manager, string key, T initialValue, T[] values, string[] strings, string title, string description) where T : IEquatable<T>
        {
            manager
                .AddLabel(title)
                .AddOption(key, initialValue, values, strings)
                .AddInfo(description);

            return manager;
        }

        public static PreferenceSystemManager AddOption<T>(this PreferenceSystemManager manager, string key, T initialValue, T[] values, string[] strings, string title, params string[] descriptions) where T : IEquatable<T>
        {
            manager
                .AddLabel(title)
                .AddOption(key, initialValue, values, strings);

            foreach (var description in descriptions)
            {
                manager.AddInfo(description);
            }

            return manager;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
