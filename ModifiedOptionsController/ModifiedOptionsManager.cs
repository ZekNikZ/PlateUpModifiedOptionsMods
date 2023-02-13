using KitchenLib;
using System.Reflection;

namespace ModifiedOptionsController
{
    public class ModifiedOptionsManager
    {
        public static bool AddExtraDishOptions = false;
        public static bool AddExtraLayoutOptions = false;

        public static float ModdedDishPercentage = 0.0f;
        public static float ModdedCardPercentage = 0.0f;
        public static bool FixCardSelection = false;

        public static void InitExtraOptions(BaseMod mod)
        {
            Init();
        }

        public static void InitPreferMods(BaseMod mod)
        {
            Init();
        }

        private static bool IsSetup = false;
        private static HarmonyLib.Harmony HarmonyInstance;
        private static void Init()
        {
            if (IsSetup)
            {
                return;
            }

            HarmonyInstance = new HarmonyLib.Harmony("ModifiedOptionsController");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            IsSetup = true;
        }
    }
}
