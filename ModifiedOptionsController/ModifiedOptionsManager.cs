using KitchenLib;
using KitchenLib.Registry;
using System.Linq;
using System.Reflection;

namespace ModifiedOptionsController
{
    public class ModifiedOptionsManager
    {
        public static bool AddExtraDishOptions = false;
        public static bool AddExtraLayoutOptions = false;

        public static bool PreferModdedDishes = false;
        public static bool PreferModdedCards = false;
        public static bool FixCardSelection = false;

        internal static bool IsSeedExplorerInstalled => ModRegistery.Registered.Any(kv => kv.Value.ModID == "beaudenon.PlateUp.SeedExplorer");


        public static void InitExtraOptions(BaseMod mod)
        {
            Init();

            if (IsSeedExplorerInstalled)
            {
                AddExtraLayoutOptions = false;
            }
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
