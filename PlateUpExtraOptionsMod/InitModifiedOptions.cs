using KitchenLib;
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

            ModifiedOptionsManager.AddExtraDishOptionsGetter = Mod.ExtraDishOptionsPreference.Get;
            ModifiedOptionsManager.AddExtraLayoutOptionsGetter = Mod.ExtraLayoutOptionsPreference.Get;
            ModifiedOptionsManager.InitExtraOptions(mod);

            Mod.LogInfo($"Initial settings: AddExtraDishOptions={ModifiedOptionsManager.AddExtraDishOptions}; AddExtraLayoutOptions={ModifiedOptionsManager.AddExtraLayoutOptions}");

            IsSetup = true;
        }
    }
}
