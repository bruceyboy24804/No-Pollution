using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Net;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using JetBrains.Annotations;
using NoPollution;
using System.Collections.Generic;
using Unity.Entities;
using static Colossal.IO.AssetDatabase.GeometryAsset;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIShowGroupName(Reseting, PollutionToggles, PolltionSliders, PollutionMasterSlider)]
    [SettingsUITabOrder(Reseting, PollutionToggles, PolltionSliders, PollutionMasterSlider)]
    [SettingsUIGroupOrder(Reseting, PollutionToggles, PolltionSliders, PollutionMasterSlider)]

    public class ModSettings : ModSetting
    {
        private const string Reseting = "Reseting";
        private const string PollutionToggles = "Pollution Toggles";
        private const string PolltionSliders = "Pollution Sliders";
        private const string PollutionMasterSlider = "Pollution Master Slider";

        public ModSettings(IMod mod) : base(mod)
        {
        }

        [SettingsUISection(Reseting)]
        [SettingsUIButton]
        public bool ResetPollution
        {
            set
            {
                Mod._debugSystem.ResetPollution();
            }
        }






        public override void SetDefaults()
        {

        }
        public void Unload()
        {

        }
    }
}