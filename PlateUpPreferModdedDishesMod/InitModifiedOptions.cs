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

            ModifiedOptionsManager.PreferModdedDishes = true;
            ModifiedOptionsManager.PreferModdedCards = true;
            ModifiedOptionsManager.InitPreferMods(mod);
            Mod.LogInfo("Enabled 'preferred modded dishes' setting");
            Mod.LogInfo("Enabled 'preferred modded cards' setting");

            IsSetup = true;
        }
    }
}
