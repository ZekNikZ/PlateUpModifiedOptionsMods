using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using KitchenMods;
using ModifiedOptionsController;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenPreferModdedOptionsMod
{
    public class Mod : BaseMod, IModSystem
    {
        // guid must be unique and is recommended to be in reverse domain name notation
        // mod name that is displayed to the player and listed in the mods menu
        // mod version must follow semver e.g. "1.2.3"
        public const string MOD_GUID = "io.zkz.plateup.prefermoddedoptions";
        public const string MOD_NAME = "Prefer Modded Options";
        public const string MOD_VERSION = "0.3.2";
        public const string MOD_AUTHOR = "ZekNikZ";
        public const string MOD_GAMEVERSION = ">=1.1.4";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.1" current and all future
        // e.g. ">=1.1.1 <=1.2.3" for all from/until

        private const string PREF_KEY_DISH_PERCENTAGE = "ModdedDishPercentage";
        private const string PREF_KEY_CARD_PERCENTAGE = "ModdedCardPercentage";
        private const string PREF_KEY_FIX_CARD_SELECTION = "FixCardSelection";

        public static PreferenceFloat DishPercentagePreference;
        public static PreferenceFloat CardPercentagePreference;
        public static PreferenceBool FixCardSelectionPreference;

        public static PreferenceManager PreferenceManager;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            // For log file output so the official plateup support staff can identify if/which a mod is being used
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            SetupPreferences();

            InitModifiedOptions.Init(this);
        }

        private void SetupPreferences()
        {
            PreferenceManager = new PreferenceManager(MOD_GUID);

            // Register preferences
            DishPercentagePreference = PreferenceManager.RegisterPreference(new PreferenceFloat(PREF_KEY_DISH_PERCENTAGE, 0.5f));
            CardPercentagePreference = PreferenceManager.RegisterPreference(new PreferenceFloat(PREF_KEY_CARD_PERCENTAGE, 1.0f));
            FixCardSelectionPreference = PreferenceManager.RegisterPreference(new PreferenceBool(PREF_KEY_FIX_CARD_SELECTION, true));
            PreferenceManager.Load();

            // Register menus
            ModsPreferencesMenu<MainMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferencesMenu<MainMenuAction>), typeof(MainMenuAction));
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferencesMenu<PauseMenuAction>), typeof(PauseMenuAction));
            Events.PreferenceMenu_MainMenu_CreateSubmenusEvent += (s, args) =>
            {
                args.Menus.Add(typeof(PreferencesMenu<MainMenuAction>), new PreferencesMenu<MainMenuAction>(args.Container, args.Module_list));
            };
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) =>
            {
                args.Menus.Add(typeof(PreferencesMenu<PauseMenuAction>), new PreferencesMenu<PauseMenuAction>(args.Container, args.Module_list));
            };
        }

        #region Logging
        // You can remove this, I just prefer a more standardized logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
