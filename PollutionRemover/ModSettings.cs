using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Net;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using Game.UI.Widgets;
using JetBrains.Annotations;
using NoPollution;
using System;
using System.Collections.Generic;
using Unity.Entities;
using static Colossal.IO.AssetDatabase.GeometryAsset;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIShowGroupName(PollutionToggles, PolltionSliders, PollutionMasterSlider)]
    [SettingsUITabOrder(PollutionToggles, PolltionSliders, PollutionMasterSlider)]
    [SettingsUIGroupOrder(PollutionToggles, PolltionSliders, PollutionMasterSlider)]

    public class ModSettings : ModSetting
    {
        
        private const string PollutionToggles = "PollutionToggles";
        private const string PolltionSliders = "PollutionSliders";
        private const string PollutionMasterSlider = "PollutionMasterSlider";

        private bool _noisePollutionSystem = true;
        private bool _netPollutionSystem = true;
        private bool _buildingPollutionAddSystem = true;
        private bool _groundPollutionSystem = true;
        private bool _groundwaterPollutionSystem = true;
        private bool _waterPipePollutionSystem = true;
        private bool _airPollutionSystem = true;

        

        public ModSettings(IMod mod) : base(mod)
        {
        }
        [SettingsUISection(PollutionToggles)]
        
        public bool NoisePollution
        {
            get => _noisePollutionSystem;
            set
            {
                _noisePollutionSystem = value;

                if (!value)
                Mod.NoisePollutionResetSystem.ResetPollution();

                Mod.ActiveWorld.GetOrCreateSystemManaged<NoisePollutionSystem>().Enabled = value;
            }
        }

        [SettingsUISection(PollutionToggles)]
        
        public bool NetPollution
        {
            get => _netPollutionSystem;
            set
            {
                _netPollutionSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<NetPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(PollutionToggles)]
        
        public bool BuildingPollution
        {
            get => _buildingPollutionAddSystem;
            set
            {
                _buildingPollutionAddSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<BuildingPollutionAddSystem>().Enabled = value;
            }
        }
        [SettingsUISection(PollutionToggles)]

        public bool GroundPollution
        {
            get => _groundPollutionSystem;
            set
            {
                _groundPollutionSystem = value;

                if (!value)
                    Mod.GroundPollutionResetSystem.ResetPollution();

                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(PollutionToggles)]
        public bool GroundWaterPollution
        {
            get => _groundwaterPollutionSystem;
            set
            {
                _groundwaterPollutionSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundWaterPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(PollutionToggles)]
        public bool WaterPipePollution
        {
            get => _waterPipePollutionSystem;
            set
            {
                _waterPipePollutionSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<WaterPipePollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(PollutionToggles)]
        public bool AirPollution
        {
            get => _airPollutionSystem;
            set
            {
                _airPollutionSystem = value;

                if (!value)
                    Mod.AirPollutionResetSystem.ResetPollution();

                Mod.ActiveWorld.GetOrCreateSystemManaged<AirPollutionSystem>().Enabled = value;
            }
        }



        public override void SetDefaults()
        {
            bool _noisePollutionSystem = true;
            bool _netPollutionSystem = true;
            bool _buildingPollutionAddSystem = true;
            bool _groundPollutionSystem = true;
            bool _groundwaterPollutionSystem = true;
            bool _waterPipePollutionSystem = true;
            bool _airPollutionSystem = true;


        }
        public void Unload()
        {

        }
    }
}