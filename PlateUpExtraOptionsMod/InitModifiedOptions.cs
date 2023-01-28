using KitchenLib;
using KitchenLib.Utils;
using ModifiedOptionsController;

namespace KitchenExtraOptionsMod
{
    internal class InitModifiedOptions
    {
        private static bool IsSetup = false;

        public static void Init(BaseMod mod)
        {
            if (IsSetup)
            {
                return;
            }

            ModifiedOptionsManager.AddExtraDishOptions = PreferenceUtils.Get<BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_DISH_OPTIONS).Value;
            ModifiedOptionsManager.AddExtraLayoutOptions = PreferenceUtils.Get<BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_LAYOUT_OPTIONS).Value;
            ModifiedOptionsManager.InitExtraOptions(mod);

            Mod.LogInfo($"Initial settings: AddExtraDishOptions={ModifiedOptionsManager.AddExtraDishOptions}; AddExtraLayoutOptions={ModifiedOptionsManager.AddExtraLayoutOptions}");

            IsSetup = true;
        }
    }
}
