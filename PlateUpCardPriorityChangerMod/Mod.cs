using HarmonyLib;
using KitchenMods;
using PreferenceSystem;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ModifiedOptionsController;

namespace KitchenPreferModdedOptionsMod
{
    public class Mod : IModInitializer
    {
        public const string MOD_GUID = "io.zkz.plateup.cardprioritychanger";
        public const string MOD_NAME = "Card Priority Changer";
        public const string MOD_VERSION = "1.0.0";
        public const string MOD_AUTHOR = "ZekNikZ";

        private const string PREF_MULTIPLIER_VANILLA = "weightMultiplierVanilla";
        private const string PREF_MULTIPLIER_MODDED = "weightMultiplierModded";
        private const string PREF_WEIGHT_MAINS = "weightMains";
        private const string PREF_WEIGHT_ADDONS = "weightAddons";
        private const string PREF_WEIGHT_STARTERS = "weightStarters";
        private const string PREF_WEIGHT_DESSERTS = "weightDesserts";
        private const string PREF_WEIGHT_CUSTOMERS = "weightCustomers";
        private const string PREF_WEIGHT_BEHAVIOR = "weightBehavior";

        private static readonly int[] WEIGHT_OPTIONS = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private static readonly string[] WEIGHT_OPTION_LABELS = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        private static readonly float[] MULTIPLIER_OPTIONS = new float[] { 0, 0.5f, 1, 1.5f, 2f, 3f, };
        private static readonly string[] MULTIPLIER_OPTION_LABELS = new string[] { "0x", "0.5x", "1x", "1.5x", "2x", "3x" };

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
                .AddInfo("This mod works by assigning \"weights\" to each card type that influence their likelihood of being picked in the card selector.")
                .AddInfo("Multipliers can also be set to change the likelihood of vanilla vs. modded cards being picked.")
                .AddOption(PREF_WEIGHT_BEHAVIOR, "RANDOM", new string[] { "RANDOM", "STRICT" }, new string[] { "Random", "Strict" }, "Weight Behavior", new string[] { "Random: weights will influence chances of card choices (twice the weight = twice the likelihood of being picked). Default: Random", "Strict: cards with higher weight will ALWAYS be offered before those with lower weights. Default: Random" })
                .AddLabel("Configure Weights:")
                .AddSubmenu("Weights", "weights")
                    .AddInfo("Weights determine the relative probabilty of types of cards being picked. Twice the weight = twice the likelihood of being picked.")
                    .AddOption(PREF_WEIGHT_CUSTOMERS, 1, WEIGHT_OPTIONS, WEIGHT_OPTION_LABELS, "Customer Cards")
                    .AddOption(PREF_WEIGHT_MAINS, 1, WEIGHT_OPTIONS, WEIGHT_OPTION_LABELS, "Food Cards: Mains")
                    .AddOption(PREF_WEIGHT_ADDONS, 1, WEIGHT_OPTIONS, WEIGHT_OPTION_LABELS, "Food Cards: Addons & Extras", "These are cards like \"Onion Pizza\" and \"Mustard\" which expand the main dish.")
                    .AddOption(PREF_WEIGHT_STARTERS, 1, WEIGHT_OPTIONS, WEIGHT_OPTION_LABELS, "Food Cards: Starters")
                    .AddOption(PREF_WEIGHT_DESSERTS, 1, WEIGHT_OPTIONS, WEIGHT_OPTION_LABELS, "Food Cards: Desserts")
                .SubmenuDone()
                .AddSubmenu("Multipliers", "multipliers")
                    .AddInfo("Multipliers can be set to change the likelihood of vanilla vs. modded cards being picked.")
                    .AddOption(PREF_MULTIPLIER_VANILLA, 1f, MULTIPLIER_OPTIONS, MULTIPLIER_OPTION_LABELS, "Vanilla")
                    .AddOption(PREF_MULTIPLIER_MODDED, 1f, MULTIPLIER_OPTIONS, MULTIPLIER_OPTION_LABELS, "Modded")
                .SubmenuDone()
                .AddSpacer();

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
