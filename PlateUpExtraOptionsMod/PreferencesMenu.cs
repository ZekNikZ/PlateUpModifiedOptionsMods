using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenExtraOptionsMod
{
    public class PreferencesMenu<T> : KLMenu<T>
    {
        private Option<bool> ExtraDishOptions;
        private Option<bool> ExtraLayoutOptions;

        public PreferencesMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
        {
        }

        public override void Setup(int player_id)
        {
            ExtraDishOptions = new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_DISH_OPTIONS).Value,
                new List<string>
                {
                    "Off", "On"
                }
            );


            ExtraLayoutOptions = new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_LAYOUT_OPTIONS).Value,
                new List<string>
                {
                    "Off", "On"
                }
            );

            AddLabel("Extra Dish Options");
            Add(ExtraDishOptions).OnChanged += delegate (object _, bool newVal)
            {
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_DISH_OPTIONS).Value = newVal;
            };
            AddInfo("Adds two extra dish options to the hub.");

            AddLabel("Extra Layout Options");
            Add(ExtraLayoutOptions).OnChanged += delegate (object _, bool newVal)
            {
                PreferenceUtils.Get<KitchenLib.BoolPreference>(Mod.MOD_GUID, Mod.PREF_EXTRA_LAYOUT_OPTIONS).Value = newVal;
            };
            AddInfo("Adds two extra layout options to the hub.");

            AddInfo("Note: the 'Extra Layout Options' is ignored with Seed Explorer is installed.");

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
