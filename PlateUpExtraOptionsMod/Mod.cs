using HarmonyLib;
using KitchenMods;
using ModifiedOptionsController;
using PreferenceSystem;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenExtraOptionsMod
{
    public class Mod : IModInitializer
    {
        public const string MOD_GUID = "io.zkz.plateup.extraoptions";
        public const string MOD_NAME = "Extra Options";
        public const string MOD_VERSION = "1.0.0";
        public const string MOD_AUTHOR = "ZekNikZ";

        public const string PREF_EXTRA_DISH_OPTIONS = "extraDishOptionsCount";
        public const string PREF_EXTRA_LAYOUT_OPTIONS = "extraLayoutOptionsCount";

        public static PreferenceSystemManager PreferenceManager;

        private Harmony HarmonyInstance;
        private List<Assembly> PatchedAssemblies = new List<Assembly>();

        public Mod()
        {
            if (HarmonyInstance == null)
            {
                HarmonyInstance = new Harmony(MOD_GUID);
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null && !PatchedAssemblies.Contains(assembly))
            {
                HarmonyInstance.PatchAll(assembly);
                PatchedAssemblies.Add(assembly);
            }
        }

        public void PostActivate(KitchenMods.Mod mod)
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");

            SetupPreferences();

            InitModifiedOptions.Init();
        }

        public void PreInject() { }

        public void PostInject() { }

        private void SetupPreferences()
        {
            PreferenceManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);

            // Register preferences
            PreferenceManager
                .AddOption(PREF_EXTRA_DISH_OPTIONS, 2, new int[] { 0, 1, 2 }, new string[] { "0", "1", "2" }, "Extra Dish Options", "The amount of extra dish options to add to the HQ. Default: 2")
                .AddOption(PREF_EXTRA_LAYOUT_OPTIONS, 2, new int[] { 0, 1, 2 }, new string[] { "0", "1", "2" }, "Extra Layout Options", "The amount of extra layout options to add to the HQ. Default: 2");

            // Register menus
            PreferenceManager.RegisterMenu(PreferenceSystemManager.MenuType.MainMenu);
            PreferenceManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
