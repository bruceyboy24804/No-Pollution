using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Remoting.Metadata;
using Unity.Entities;
using UnityEngine;
using static Game.Rendering.Debug.RenderPrefabRenderer;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUITabOrder(MainTab, SystemsTab)]
    [SettingsUIGroupOrder(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, SystemsGroup)]
    [SettingsUIShowGroupName(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, SystemsGroup)]

    



    public class Setting : ModSetting
    {
       
        public const string MainTab = "Main";
        public const string NoisePollutionGroup = "Noise Pollution";
        public const string NetPollutionGroup = "Net Pollution";
        public const string BuildingPollutionGroup = "Building Pollution";
        public const string GroundPollutionGroup = "Ground Pollution";
        public const string GroundwaterPollutionGroup = "Groundwater Pollution";
        public const string AirPollutionGroup = "Air Pollution";
        public const string WaterPollutionGroup = "Water Pollution";

        
        public const string SystemsTab = "Systems";
        public const string SystemsGroup = "Systems";

        public const string ParametersTab = "Parameters";
        public const string ParametersGroup = "Parameters";
      

        private bool _airPollutionSystem = true;

        private bool _groundPollutionSystem = true;

        private bool _noisePollutionSystem = true;

        private bool _netPollutionSystem = true;

        private bool _buildingPollutionAddSystem = true;

        private bool _groundwaterPollutionSystem = true;

        private bool _waterPollutionDecayInstant = false;

        private int _waterPollutionDecayRate = 10;

        





        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }
        [SettingsUISection(SystemsTab, SystemsTab)]
        public bool NoisePollutionToggle
        {
            get => _noisePollutionSystem;
            set
            {   _noisePollutionSystem = value;
                if (!value)
                {
                    Mod.NoisePollutionResetSystem.ResetPollution();
                }
                Mod.ActiveWorld.GetOrCreateSystemManaged<NoisePollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(SystemsTab, SystemsGroup)]
        public bool GroundPollutionToggle
        {
            get => _groundPollutionSystem;
            set
            {
                _groundPollutionSystem = value;
                if (!value)
                {
                    Mod.GroundPollutionResetSystem.ResetPollution();
                }

                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundPollutionSystem>().Enabled = value;

            }
        }        
        

        [SettingsUISection(SystemsTab, SystemsGroup)]
        public bool AirPollutionToggle
        {
            get => _airPollutionSystem;
            set
            {
                _airPollutionSystem = value;
                if (!value)
                {
                    Mod.AirPollutionResetSystem.ResetPollution();
                }
                Mod.ActiveWorld.GetOrCreateSystemManaged<AirPollutionSystem>().Enabled = value;
            }
        }

      

        [SettingsUISection(MainTab, NoisePollutionGroup)]
        [SettingsUIButton]
        public bool NoisePollutionResetButton
        {
            set => Mod.NoisePollutionResetSystem.ResetPollution();
        }
        


        [SettingsUISection(MainTab, NetPollutionGroup)]

        public bool NetPollutionToggle
        {
            get => _netPollutionSystem;
            set
            {
                _netPollutionSystem = value;
                Mod.ActiveWorld.GetOrCreateSystemManaged<NetPollutionSystem>();
                
            }
        }
       


        [SettingsUISection(MainTab, BuildingPollutionGroup)]
        public bool BuildingPollutionToggle
        {
            get => _buildingPollutionAddSystem;
           
            set
            {
                _buildingPollutionAddSystem = value;
                Mod.ActiveWorld.GetOrCreateSystemManaged<BuildingPollutionAddSystem>().Enabled = value;
            }
        }



        [SettingsUISection(MainTab, GroundPollutionGroup)]
        [SettingsUIButton]
        public bool GroundPollutionResetButton
        {
            set => Mod.GroundPollutionResetSystem.ResetPollution();
        }



        [SettingsUISection(MainTab, GroundwaterPollutionGroup)]
        public bool GroundWaterPollutionToggle
        {
            get => _groundwaterPollutionSystem;
            set
            {
                _groundwaterPollutionSystem = value;
                if (Mod.ActiveWorld != null)
                {
                    var system = Mod.ActiveWorld.GetOrCreateSystemManaged<GroundWaterPollutionSystem>();
                    if (system != null)
                    {
                        system.Enabled = value;
                    }
                }
            }
        }
       

        


        [SettingsUISection(MainTab, AirPollutionGroup)]
        [SettingsUIButton]
        public bool AirPollutionResetButton
        {
            set => Mod.AirPollutionResetSystem.ResetPollution();
        }

        [SettingsUISection(MainTab, WaterPollutionGroup)]
        public bool WaterPollutionDecayInstantToggle
        {
            get => _waterPollutionDecayInstant;
            set
            {
                _waterPollutionDecayInstant = value;
                if (value)
                {
                    WaterPollutionDecayRateSlider = 1000;
                }
                else
                {
                    WaterPollutionDecayRateSlider = 1;
                }

            }

        }

        [SettingsUISection(MainTab, WaterPollutionGroup)]
        [SettingsUISlider(min = 1, max = 100, step = 1)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(WaterPollutionDecayInstantToggle))]
        public int WaterPollutionDecayRateSlider
        {
            get => _waterPollutionDecayRate;
            set
            {
                _waterPollutionDecayRate = value;
                WaterSystem waterSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<WaterSystem>();
                waterSystem.m_PollutionDecayRate = _waterPollutionDecayRate * 0.001f;
            }
        }

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, ParametersGroup)]
        [SettingsUISlider(min = 1, max = 100, step = 1)]
        public float NetNoiseMultiplier { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, ParametersGroup)]
        [SettingsUISlider(min = 1, max = 100, step = 1)]
        public float NoiseRadius { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, ParametersGroup)]
        [SettingsUISlider(min = 1, max = 100, step = 1)]
        public float NetNoiseRadius { get; set; }


        public override void SetDefaults()
        {

            _netPollutionSystem = true;
          
            _buildingPollutionAddSystem = true;
         
            _groundwaterPollutionSystem = true;
         
            WaterPollutionDecayRateSlider = 10;
            _airPollutionSystem = true;
            _groundPollutionSystem = true;
            _noisePollutionSystem = true;

            NetNoiseMultiplier = 100;
            NoiseRadius = 100;
            NetNoiseRadius = 100;

        }
        public class LocaleEN : IDictionarySource
        {
            private readonly Setting m_Setting;
            public LocaleEN(Setting setting)
            {
                m_Setting = setting;
            }
            public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "No Pollution" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Main" },
                    { m_Setting.GetOptionTabLocaleID(SystemsTab), "Systems" },
                    { m_Setting.GetOptionTabLocaleID(ParametersTab), "Parameters" },

                   
                    { m_Setting.GetOptionGroupLocaleID(NoisePollutionGroup), "Noise Pollution" },
                    { m_Setting.GetOptionGroupLocaleID(NetPollutionGroup), "Net pollution" },
                    { m_Setting.GetOptionGroupLocaleID(BuildingPollutionGroup), "Building pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundPollutionGroup), "Ground pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundwaterPollutionGroup), "Groundwater pollution" },
                    { m_Setting.GetOptionGroupLocaleID(AirPollutionGroup), "Air pollution" },
                    { m_Setting.GetOptionGroupLocaleID(WaterPollutionGroup), "Water pollution" },
                    { m_Setting.GetOptionGroupLocaleID(SystemsGroup), "Systems" },
                    { m_Setting.GetOptionGroupLocaleID(ParametersGroup), "Parameters" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionToggle)), "Noise pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionToggle)), "Enable/Disable noise pollution system (when unchecked it will stop noise pollution from being produced on the map and should reset the noise pollution back to 0)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionToggle)), "Ground pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionToggle)), "Enable/Disable ground pollution system(when unchecked it will stop ground pollution from being produced on the map and should reset the ground pollution back to 0)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionToggle)), "Air pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionToggle)), "Enable/Disable ground pollution system(when unchecked it will stop air pollution from being produced on the map and should reset the air pollution back to 0)" },

                  
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionResetButton)), "Noise pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionResetButton)), "This will reset the noise pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of noise pollution would be 1000 and pressing this button would reset the noise pollution back to 0" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionToggle)), "Net pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionToggle)), "Enable/Disable net pollution" },
                   

                    { m_Setting.GetOptionLabelLocaleID(nameof(BuildingPollutionToggle)), "Building pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(BuildingPollutionToggle)), "Enable/Disable building pollution" },

                  
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionResetButton)), "Ground pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionResetButton)), "This will reset the ground pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of ground pollution would be 1000 and pressing this button would reset the ground pollution back to 0" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundWaterPollutionToggle)), "Ground water pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundWaterPollutionToggle)), "Enable/Disable ground water pollution" },

                   
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionResetButton)), "Air pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionResetButton)), "This will reset the air pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of air pollution would be 1000 and pressing this button would reset the air pollution back to 0" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Instant decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Set water pollution decay rate to 1000" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayRateSlider)), "Water pollution decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayRateSlider)), "Set water pollution decay rate" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseMultiplier)), "Net noise Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseMultiplier)), "Set net noise multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoiseRadius)), "Noise Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoiseRadius)), "Set noise radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseRadius)), "Net Noise Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseRadius)), "Set net noise radius" },





                };
            }

            public void Unload() { }
        }
        

    }

}
