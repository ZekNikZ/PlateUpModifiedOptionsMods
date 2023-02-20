using KitchenLib;
using System.Reflection;

namespace ModifiedOptionsController
{
    public class ModifiedOptionsManager
    {
        public delegate T ValueProvider<T>();

        public static ValueProvider<bool> AddExtraDishOptionsGetter = () => false;
        public static bool AddExtraDishOptions => AddExtraDishOptionsGetter.Invoke();

        public static ValueProvider<bool> AddExtraLayoutOptionsGetter = () => false;
        public static bool AddExtraLayoutOptions => AddExtraLayoutOptionsGetter.Invoke();

        public static ValueProvider<float> ModdedDishPercentageGetter = () => 0.0f;
        public static float ModdedDishPercentage => ModdedDishPercentageGetter.Invoke();

        public static ValueProvider<float> ModdedCardPercentageGetter = () => 0.0f;
        public static float ModdedCardPercentage => ModdedCardPercentageGetter.Invoke();

        public static ValueProvider<bool> FixCardSelectionGetter = () => false;
        public static bool FixCardSelection => FixCardSelectionGetter.Invoke();

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
