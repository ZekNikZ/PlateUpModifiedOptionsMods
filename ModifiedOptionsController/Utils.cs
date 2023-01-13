using KitchenData;
using KitchenLib.Customs;

namespace ModifiedOptionsController
{
    internal class Utils
    {
        public static bool IsModded(int id)
        {
            return CustomGDO.GDOs.ContainsKey(id);
        }

        public static bool IsModded(GameDataObject gdo)
        {
            return IsModded(gdo.ID);
        }
    }
}
