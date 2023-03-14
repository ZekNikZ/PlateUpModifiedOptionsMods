using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenExtraOptionsMod
{
    public class PreferencesMenu<T> : KLMenu<T>
    {
        public PreferencesMenu(Transform container, ModuleList moduleList) : base(container, moduleList)
        {
        }

        public override void Setup(int player_id)
        {
            AddLabel("Settings Profile");
            AddProfileSelector(Mod.MOD_GUID, newProfile =>
            {
                Mod.PreferenceManager.Save();
                Mod.PreferenceManager.SetProfile(newProfile);
                Mod.PreferenceManager.Load();
            }, Mod.PreferenceManager);

            AddLabel("Extra Dish Options");
            Add(new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                Mod.ExtraDishOptionsPreference.Get(),
                new List<string>
                {
                    "Off", "On"
                }
            )).OnChanged += delegate (object _, bool newVal)
            {
                Mod.ExtraDishOptionsPreference.Set(newVal);
            };
            AddInfo("Adds two extra dish options to the hub.");

            AddLabel("Extra Layout Options");
            Add(new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                Mod.ExtraLayoutOptionsPreference.Get(),
                new List<string>
                {
                    "Off", "On"
                }
            )).OnChanged += delegate (object _, bool newVal)
            {
                Mod.ExtraLayoutOptionsPreference.Set(newVal);
            };
            AddInfo("Adds two extra layout options to the hub.");

            AddButton("Apply", delegate
            {
                Mod.PreferenceManager.Save();
                RequestPreviousMenu();
            });

            AddButton(Localisation["MENU_BACK_SETTINGS"], delegate
            {
                RequestPreviousMenu();
            });
        }
    }
}
