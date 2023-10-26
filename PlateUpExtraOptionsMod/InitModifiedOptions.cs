using ModifiedOptionsController;

namespace KitchenExtraOptionsMod
{
    internal class InitModifiedOptions
    {
        private static bool IsSetup = false;

        public static void Init()
        {
            if (IsSetup)
            {
                return;
            }

            ModifiedOptionsManager.ExtraDishOptionsCountGetter = () => Mod.PreferenceManager.Get<int>(Mod.PREF_EXTRA_DISH_OPTIONS);
            ModifiedOptionsManager.ExtraLayoutOptionsCountGetter = () => Mod.PreferenceManager.Get<int>(Mod.PREF_EXTRA_LAYOUT_OPTIONS);
            ModifiedOptionsManager.Init();

            Mod.LogInfo($"Initial settings: AddExtraDishOptions={ModifiedOptionsManager.ExtraDishOptionsCount}; AddExtraLayoutOptions={ModifiedOptionsManager.ExtraLayoutOptionsCount}");

            IsSetup = true;
        }
    }
}
