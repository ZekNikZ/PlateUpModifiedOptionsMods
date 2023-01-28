using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenPreferModdedOptionsMod
{
    public class PreferencesMenu<T> : KLMenu<T>
    {
        private Option<bool> PreferModdedDishes;
        private Option<float> ModdedCardPercentage;
        private Option<bool> FixCardSelection;

        public PreferencesMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
        {
        }

        public override void Setup(int player_id)
        {
            PreferModdedDishes = new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_PREFER_MODDED_DISHES).Value,
                new List<string>
                {
                    "Off", "On"
                }
            );

            ModdedCardPercentage = new Option<float>(
                new List<float>
                {
                     0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1
                },
                PreferenceUtils.Get<KitchenLib.FloatPreference>(Mod.MOD_GUID, Mod.PREF_CARD_PERCENTAGE).Value,
                new List<string>
                {
                    "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"
                });

            FixCardSelection = new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_FIX_CARD_SELECTION).Value,
                new List<string>
                {
                    "Off", "On"
                }
            );

            AddLabel("Prefer Modded Dishes");
            Add(PreferModdedDishes).OnChanged += delegate (object _, bool newVal)
            {
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_PREFER_MODDED_DISHES).Value = newVal;
            };
            AddInfo("Forces modded dishes to appear in the hub before vanilla ones.");

            AddLabel("Modded Card Override");
            Add(ModdedCardPercentage).OnChanged += delegate (object _, float newVal)
            {
                PreferenceUtils.Get<KitchenLib.FloatPreference>(Mod.MOD_GUID, Mod.PREF_CARD_PERCENTAGE).Value = newVal;
            };
            AddInfo("Determines the chance of card choices being forced to be modded cards.");

            AddLabel("Fix Card Selection");
            Add(FixCardSelection).OnChanged += delegate (object _, bool newVal)
            {
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_FIX_CARD_SELECTION).Value = newVal;
            };
            AddInfo("(Recommended) Fixes a bug with the vanilla card selection algorithm.");

            AddButton("Apply", delegate
            {
                PreferenceUtils.Save();
                RequestPreviousMenu();
            });

            AddButton(Localisation["MENU_BACK_SETTINGS"], delegate
            {
                RequestPreviousMenu();
            });
        }
    }
}
