using KitchenLib;
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

            ModifiedOptionsManager.ModdedDishPercentageGetter = Mod.DishPercentagePreference.Get;
            ModifiedOptionsManager.ModdedCardPercentageGetter = Mod.CardPercentagePreference.Get;
            ModifiedOptionsManager.FixCardSelectionGetter = Mod.FixCardSelectionPreference.Get;
            ModifiedOptionsManager.InitPreferMods(mod);

            Mod.LogInfo($"Initial settings: PreferModdedDishes={ModifiedOptionsManager.ModdedDishPercentage}; ModdedCardPercentage={ModifiedOptionsManager.ModdedCardPercentage}; FixCardSelection={ModifiedOptionsManager.FixCardSelection}");

            IsSetup = true;
        }
    }
}
