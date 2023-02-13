using KitchenLib;
using KitchenLib.Utils;
using ModifiedOptionsController;

namespace KitchenPreferModdedOptionsMod
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

            ModifiedOptionsManager.ModdedDishPercentage = PreferenceUtils.Get<FloatPreference>(Mod.MOD_GUID, Mod.PREF_DISH_PERCENTAGE).Value;
            ModifiedOptionsManager.FixCardSelection = PreferenceUtils.Get<BoolPreference>(Mod.MOD_GUID, Mod.PREF_FIX_CARD_SELECTION).Value;
            ModifiedOptionsManager.ModdedCardPercentage = PreferenceUtils.Get<FloatPreference>(Mod.MOD_GUID, Mod.PREF_CARD_PERCENTAGE).Value;
            ModifiedOptionsManager.InitPreferMods(mod);

            Mod.LogInfo($"Initial settings: PreferModdedDishes={ModifiedOptionsManager.ModdedDishPercentage}; ModdedCardPercentage={ModifiedOptionsManager.ModdedCardPercentage}; FixCardSelection={ModifiedOptionsManager.FixCardSelection}");

            IsSetup = true;
        }
    }
}
