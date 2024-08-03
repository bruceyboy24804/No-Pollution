using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata;
using Unity.Entities;
using UnityEngine;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIShowGroupName(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup)]
    [SettingsUITabOrder(MainTab)]
    [SettingsUIGroupOrder(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup)]



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






      
       
        private bool _netPollutionSystem = true;

        private bool _buildingPollutionAddSystem = true;

        private bool _groundwaterPollutionSystem = true;

        private bool _waterPollutionDecayInstant = false;

        private int _waterPollutionDecayRate = 10;

        



        public Setting(IMod mod) : base(mod)
        {

        }

        [SettingsUISection(NoisePollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public int NoisePollutionDataSlider { get; set; }

        [SettingsUISection(NoisePollutionGroup)]
        [SettingsUIButton]
        public bool NoisePollutionResetButton
        {
            set => Mod.NoisePollutionResetSystem.ResetPollution();
        }
        


        [SettingsUISection(NetPollutionGroup)]

        public bool NetPollutionToggle
        {
            get => _netPollutionSystem;
            set
            {
                _netPollutionSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<NetPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(BuildingPollutionGroup)]

        public bool BuildingPollutionToggle
        {
            get => _buildingPollutionAddSystem;
            set
            {
                _buildingPollutionAddSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<BuildingPollutionAddSystem>().Enabled = value;
            }
        }

       
        [SettingsUISection(GroundPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public int GroundPollutionDataSlider { get; set; }
        [SettingsUISection(GroundPollutionGroup)]
        [SettingsUIButton]
        public bool GroundPollutionResetButton
        {
            set => Mod.GroundPollutionResetSystem.ResetPollution();
        }



        [SettingsUISection(GroundwaterPollutionGroup)]
        public bool GroundWaterPollutionToggle
        {
            get => _groundwaterPollutionSystem;
            set
            {
                _groundwaterPollutionSystem = value;

                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundWaterPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(AirPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public int AirPollutionDataSlider { get; set; }
        
        [SettingsUISection(AirPollutionGroup)]
        [SettingsUIButton]
        public bool AirPollutionResetButton
        {
            set => Mod.GroundPollutionResetSystem.ResetPollution();
        }

        [SettingsUISection(WaterPollutionGroup)]
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

        [SettingsUISection(WaterPollutionGroup)]
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

       

        public override void SetDefaults()
        {
            NoisePollutionDataSlider = 100;
           
            _netPollutionSystem = true;

            _buildingPollutionAddSystem = true;

            GroundPollutionDataSlider = 100;

            _groundwaterPollutionSystem = true;

            AirPollutionDataSlider = 100;

            WaterPollutionDecayRateSlider = 10;

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
                    { m_Setting.GetSettingsLocaleID(), "NoPollution" },
                    { m_Setting.GetOptionTabLocaleID(MainTab), "Main" },
                    
                    { m_Setting.GetOptionGroupLocaleID(NoisePollutionGroup), "Noise Pollution" },
                    { m_Setting.GetOptionGroupLocaleID(NetPollutionGroup), "Net pollution" },
                    { m_Setting.GetOptionGroupLocaleID(BuildingPollutionGroup), "Building pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundPollutionGroup), "Ground pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundwaterPollutionGroup), "Groundwater pollution" },
                    { m_Setting.GetOptionGroupLocaleID(AirPollutionGroup), "Air pollution" },
                    { m_Setting.GetOptionGroupLocaleID(WaterPollutionGroup), "Water pollution" },


                     { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionDataSlider)), "Noise pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionDataSlider)), "Set noise pollution" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionResetButton)), "Noise pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionResetButton)), "Reset noise pollution after it has been toggled off" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionToggle)), "Net pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionToggle)), "Enable/Disable net pollution" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(BuildingPollutionToggle)), "Building pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(BuildingPollutionToggle)), "Enable/Disable building pollution" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionDataSlider)), "Ground pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionDataSlider)), "Set ground pollution" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionResetButton)), "Ground pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionResetButton)), "Reset ground pollution after it has been toggled off" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundWaterPollutionToggle)), "Ground water pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundWaterPollutionToggle)), "Enable/Disable ground water pollution" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionDataSlider)), "Air pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionDataSlider)), "Set ground pollution" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionResetButton)), "Ground pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionResetButton)), "Reset ground pollution after it has been toggled off" },

                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Instant decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Set water pollution decay rate to 1000" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayRateSlider)), "Water pollution decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayRateSlider)), "Set water pollution decay rate" },




                };
            }

            public void Unload() { }
        }
        

    }

}
