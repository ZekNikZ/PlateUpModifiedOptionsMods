using KitchenLib;
using ModifiedOptionsController;

namespace KitchenExtraOptionsMod
{
    internal class InitModifiedOptions
    {
        private static bool IsSetup = false;

        public static void Init(BaseMod mod)
        {
            Mod.LogInfo("here 2");

            if (IsSetup)
            {
                return;
            }
            Mod.LogInfo("here 3");

            ModifiedOptionsManager.AddExtraDishOptions = true;
            ModifiedOptionsManager.AddExtraLayoutOptions = true;
            ModifiedOptionsManager.InitExtraOptions(mod);
            Mod.LogInfo("Enabled 'extra dish options' setting");
            Mod.LogInfo("Enabled 'extra layout options' setting");

            IsSetup = true;
        }
    }
}
