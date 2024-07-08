
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.Tools;
using Game.UI;
using System;
using Unity.Entities;
using UnityEngine;


namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIShowGroupName(Resetting, PollutionToggles, WaterPollution)]
    [SettingsUITabOrder(Resetting, PollutionToggles, WaterPollution)]
    [SettingsUIGroupOrder(Resetting, PollutionToggles, WaterPollution)]



    public class ModSettings : ModSetting
    {
        private const string Resetting = "Resetting";
        private const string PollutionToggles = "PollutionToggles";
        private const string WaterPollution = "WaterPollution";

       

        
        private bool _noisePollutionSystem = true;
        private bool _netPollutionSystem = true;
        private bool _buildingPollutionAddSystem = true;
        private bool _groundPollutionSystem = true;
        private bool _groundwaterPollutionSystem = true;
        private bool _waterPipePollutionSystem = true;
        private bool _airPollutionSystem = true;
        private float _waterPollutionDecayRate = 10f;
        private bool _waterPollutionDecayIsntant = false;
        
        

        public ModSettings(IMod mod) : base(mod)
        {
           
        }






        [SettingsUISection(Resetting)]
        [SettingsUIButton]
        public bool NoisePollutionReset
        {
            set => Mod.NoisePollutionResetSystem.ResetPollution();
        }


        [SettingsUISection(PollutionToggles)]

        public bool NoisePollution
        {
            get => _noisePollutionSystem;
            set
            {
                _noisePollutionSystem = value;



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
        [SettingsUISection(WaterPollution)]

        public bool WaterPollutionDecayInstant
        {
            get => _waterPollutionDecayIsntant;
            set
            {
                _waterPollutionDecayRate = 1000000000f;
                WaterSystem waterSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<WaterSystem>();
                waterSystem.m_PollutionDecayRate = 1000000000f;

            }

        }
    
    

          
        
           
        [SettingsUISection(WaterPollution)]
        [SettingsUISlider(min = 0, max = 1000, step = 1, scalarMultiplier = 0.0001f)]
        [SettingsUIDisableByCondition(typeof(ModSettings), nameof(WaterPollutionDecayInstant))]
        public float WaterSystem
        {
            get => _waterPollutionDecayRate;
            set
            {
                _waterPollutionDecayRate = value;
                WaterSystem waterSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<WaterSystem>();
                waterSystem.m_PollutionDecayRate = value;
                
            }
        }

       

        public override void SetDefaults()
        {
            _noisePollutionSystem = true;
            _netPollutionSystem = true;
            _buildingPollutionAddSystem = true;
            _groundPollutionSystem = true;
            _groundwaterPollutionSystem = true;
            _waterPipePollutionSystem = true;
            _airPollutionSystem = true;
            _waterPollutionDecayRate = 10f;
            
        }

        public void Unload()
        {

        }

    }

}
 