using System.Collections.Generic;
using System.Reflection;

namespace ModifiedOptionsController
{
    public class ModifiedOptionsManager
    {
        public delegate T ValueProvider<T>();

        #region Extra Options
        public static ValueProvider<int> ExtraDishOptionsCountGetter = () => 0;
        public static int ExtraDishOptionsCount => ExtraDishOptionsCountGetter.Invoke();

        public static ValueProvider<int> ExtraLayoutOptionsCountGetter = () => 0;
        public static int ExtraLayoutOptionsCount => ExtraLayoutOptionsCountGetter.Invoke();
        #endregion

        #region Card Priority Changer
        public enum WeightBehavior
        {
            RANDOM,
            STRICT
        }

        public static ValueProvider<float> ModdedDishPercentageGetter = () => 0.0f;
        public static float ModdedDishPercentage => ModdedDishPercentageGetter.Invoke();

        public static ValueProvider<float> ModdedCardPercentageGetter = () => 0.0f;
        public static float ModdedCardPercentage => ModdedCardPercentageGetter.Invoke();

        public static ValueProvider<float> AddonCardPercentageGetter = () => 0.0f;
        public static float AddonCardPercentage => AddonCardPercentageGetter.Invoke();

        public static ValueProvider<bool> FixCardSelectionGetter = () => false;
        public static bool FixCardSelection => FixCardSelectionGetter.Invoke();
        #endregion

        private static bool IsSetup = false;
        private static HarmonyLib.Harmony HarmonyInstance;
        public static void Init()
        {
            if (IsSetup)
            {
                return;
            }

            HarmonyInstance = new HarmonyLib.Harmony("ModifiedOptionsController");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            IsSetup = true;
        }

        internal static HashSet<int> GDOCache;
    }
}
