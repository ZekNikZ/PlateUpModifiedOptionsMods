using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Utils;
using KitchenMods;
using ModifiedOptionsController;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenExtraOptionsMod
{
    public class Mod : BaseMod, IModSystem
    {
        // guid must be unique and is recommended to be in reverse domain name notation
        // mod name that is displayed to the player and listed in the mods menu
        // mod version must follow semver e.g. "1.2.3"
        public const string MOD_GUID = "io.zkz.plateup.extraoptions";
        public const string MOD_NAME = "Extra Options";
        public const string MOD_VERSION = "0.4.0";
        public const string MOD_AUTHOR = "ZekNikZ";
        public const string MOD_GAMEVERSION = ">=1.1.1";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.1" current and all future
        // e.g. ">=1.1.1 <=1.2.3" for all from/until

        public const string PREF_EXTRA_DISH_OPTIONS = "ExtraDishOptions";
        public const string PREF_EXTRA_LAYOUT_OPTIONS = "ExtraLayoutOptions";

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void Initialise()
        {
            base.Initialise();

            // For log file output so the official plateup support staff can identify if/which a mod is being used
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            SetupPreferences();

            var dishPref = AddPreference<KitchenLib.BoolPreference>(MOD_GUID, PREF_EXTRA_DISH_OPTIONS, "Extra Dish Options");
            dishPref.Value = true;
            var layoutPref = AddPreference<KitchenLib.BoolPreference>(MOD_GUID, PREF_EXTRA_LAYOUT_OPTIONS, "Extra Layout Options");
            layoutPref.Value = true;
            PreferenceUtils.Load();

            InitModifiedOptions.Init(this);
        }

        private void SetupPreferences()
        {
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

            Events.PreferencesSaveEvent += (s, args) =>
            {
                ModifiedOptionsManager.AddExtraDishOptions = PreferenceUtils.Get<KitchenLib.BoolPreference>(MOD_GUID, PREF_EXTRA_DISH_OPTIONS).Value;
                ModifiedOptionsManager.AddExtraLayoutOptions = PreferenceUtils.Get<KitchenLib.BoolPreference>(MOD_GUID, PREF_EXTRA_LAYOUT_OPTIONS).Value;

                LogInfo($"New settings: AddExtraDishOptions={ModifiedOptionsManager.AddExtraDishOptions}; AddExtraLayoutOptions={ModifiedOptionsManager.AddExtraLayoutOptions}");
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
