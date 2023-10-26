using ModifiedOptionsController;

namespace KitchenPreferModdedOptionsMod
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

            //ModifiedOptionsManager.ModdedDishPercentageGetter = Mod.DishPercentagePreference.Get;
            //ModifiedOptionsManager.ModdedCardPercentageGetter = Mod.CardPercentagePreference.Get;
            //ModifiedOptionsManager.AddonCardPercentageGetter = Mod.AddonCardPercentagePreference.Get;
            //ModifiedOptionsManager.FixCardSelectionGetter = Mod.FixCardSelectionPreference.Get;
            ModifiedOptionsManager.Init();

            Mod.LogInfo($"Initial settings: PreferModdedDishes={ModifiedOptionsManager.ModdedDishPercentage}; ModdedCardPercentage={ModifiedOptionsManager.ModdedCardPercentage}; AddonCardPercentage={ModifiedOptionsManager.ModdedCardPercentage}; FixCardSelection={ModifiedOptionsManager.FixCardSelection}");

            IsSetup = true;
        }
    }
}
