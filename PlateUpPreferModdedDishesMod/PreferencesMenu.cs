using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenPreferModdedOptionsMod
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

            AddLabel("Modded Dish Override");
            Add(new Option<float>(
                new List<float>
                {
                     0, 0.25f, 0.5f, 0.75f, 1
                },
                Mod.DishPercentagePreference.Get(),
                new List<string>
                {
                    "0%", "25%", "50%", "75%", "100%"
                }
            )).OnChanged += delegate (object _, float newVal)
            {
                Mod.DishPercentagePreference.Set(newVal);
            };
            AddInfo("Forces modded dishes to appear in the hub before vanilla ones.");

            AddLabel("Modded Card Override");
            Add(new Option<float>(
                new List<float>
                {
                     0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1
                },
                Mod.CardPercentagePreference.Get(),
                new List<string>
                {
                    "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"
                }
            )).OnChanged += delegate (object _, float newVal)
            {
                Mod.CardPercentagePreference.Set(newVal);
            };
            AddInfo("Determines the chance of card choices being forced to be modded cards.");

            AddLabel("Fix Card Selection");
            Add(new Option<bool>(
                new List<bool>
                {
                    false, true
                },
                Mod.FixCardSelectionPreference.Get(),
                new List<string>
                {
                    "Off", "On"
                }
            )).OnChanged += delegate (object _, bool newVal)
            {
                Mod.FixCardSelectionPreference.Set(newVal);
            };
            AddInfo("(Recommended) Fixes a bug with the vanilla card selection algorithm.");

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
