using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Objects;
using Game.Prefabs.Climate;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using NoPollution.ResetSystems;
using System.Collections.Generic;
using Unity.Entities;


namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUITabOrder(MainTab, ParametersTab)]
    [SettingsUIGroupOrder(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, MultipliersGroup, RadiusGroup, FadesGroup, NotificationLimitsGroup, OtherParametersGroup)]
    [SettingsUIShowGroupName(NoisePollutionGroup, NetPollutionGroup, BuildingPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, MultipliersGroup, RadiusGroup, FadesGroup, NotificationLimitsGroup, OtherParametersGroup)]

    



    public class Setting : ModSetting
    {
        public static  World World { get; set; }

        public const string MainTab = "Main";
        public const string NoisePollutionGroup = "Noise Pollution";
        public const string NetPollutionGroup = "Net Pollution";
        public const string BuildingPollutionGroup = "Building Pollution";
        public const string GroundPollutionGroup = "Ground Pollution";
        public const string GroundwaterPollutionGroup = "Groundwater Pollution";
        public const string AirPollutionGroup = "Air Pollution";
        public const string WaterPollutionGroup = "Water Pollution";

        public const string ParametersTab = "Parameters";
        public const string MultipliersGroup = "Multipliers";
        public const string RadiusGroup = "Radii's";
        public const string FadesGroup = "Fades";
        public const string NotificationLimitsGroup = "Notification Limits";
        public const string OtherParametersGroup = "Other Parameters";


        private bool _noisePollutionSystem = false;
        private bool _groundPollutionSystem = false;
        private bool _airPollutionSystem = false;
        private bool _netPollutionSystem = false;
        private bool _buildingPollutionAddSystem = false;
        private bool _groundwaterPollutionSystem = false;
        private bool _waterPollutionDecayInstantToggle;
        private int _waterPollutionDecayRate = 10;
       
        public Setting(IMod mod) : base(mod) { }
        
      

        [SettingsUISection(MainTab, NoisePollutionGroup)]
        public bool NoisePollutionToggle
        {
            get => _noisePollutionSystem;
           
            set
            {
                _noisePollutionSystem = value;
                Mod.ActiveWorld.GetOrCreateSystemManaged<NoisePollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(MainTab, NoisePollutionGroup)]
        [SettingsUIButton]
        public bool NoisePollutionResetButton
        {
            set => NoisePollutionResetSystem.ResetPollution();
        }

        [SettingsUISection(MainTab, GroundPollutionGroup)]
        public bool GroundPollutionToggle
        {
            get
            {
                return _groundPollutionSystem;
            }
            set
            {
                _groundPollutionSystem = value;
                if (!value)
                {
                    GroundPollutionResetSystem.ResetPollution();
                }
                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(MainTab, GroundPollutionGroup)]
        [SettingsUIButton]
        public bool GroundPollutionResetButton
        {
            set => GroundPollutionResetSystem.ResetPollution();
        }

        [SettingsUISection(MainTab, AirPollutionGroup)]
        public bool AirPollutionToggle
        {
            get => _airPollutionSystem;
           
            set
            {
                _airPollutionSystem = value;
               
                Mod.ActiveWorld.GetOrCreateSystemManaged<AirPollutionSystem>().Enabled = value;
            }
        }
        [SettingsUISection(MainTab, AirPollutionGroup)]
        [SettingsUIButton]
        public bool AirPollutionResetButton
        {
            set => AirPollutionResetSystem.ResetPollution();
        }

        [SettingsUISection(MainTab, NetPollutionGroup)]

        public bool NetPollutionToggle
        {
            get => _netPollutionSystem;
           
            set
            {
                _netPollutionSystem = value;
                Mod.ActiveWorld.GetOrCreateSystemManaged<NetPollutionSystem>().Enabled = value;
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

        [SettingsUISection(MainTab, GroundwaterPollutionGroup)]
        public bool GroundWaterPollutionToggle
        {
            get => _groundwaterPollutionSystem;
           
            set
            {
                _groundwaterPollutionSystem = value;
                Mod.ActiveWorld.GetOrCreateSystemManaged<GroundWaterPollutionSystem>().Enabled = value;
            }
        }

        [SettingsUISection(MainTab, WaterPollutionGroup)]
        public bool WaterPollutionDecayInstantToggle
        {
            get
            {
                return _waterPollutionDecayInstantToggle;
            }
            set
            {
                _waterPollutionDecayInstantToggle = value;
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

        //MULTIPLIERS 
      
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 20, step = 1)]
        public float GroundMultiplier { get; set; } = 20;
     

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 25, step = 1)]
        public float AirMultiplier { get; set; } = 25;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public float NoiseMultiplier { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 1.75f, step = 0.5f)]
        public float NetAirMultiplier { get; set; } = 1.75f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 5, step = 0.5f)]
        public float NetNoiseMultiplier { get; set; } = 5;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 0.001f, step = 0.001f)]
        public float PlantAirMultiplier { get; set; } = 0.001f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 0.001f, step = 0.001f)]
        public float PlantGroundMultiplier { get; set; } = 0.001f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 1, step = 0.1f)]
        public float FertilityGroundMultiplier { get; set; } = 1;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 5, step = 0.1f)]
        public float AbandonedNoisePollutionMultiplier { get; set; } = 5;

        //RADIUS
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsAirPollutionDisabled))]
        public float AirRadius { get; set; } = 100;
        public bool IsAirPollutionDisabled => !_airPollutionSystem;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public float GroundRadius { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsNoisePollutionDisabled))]
        public float NoiseRadius { get; set; } = 100;
        public bool IsNoisePollutionDisabled => !_noisePollutionSystem;


        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 8, step = 0.1f)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsNetPollutionDisabled))]
        public float NetNoiseRadius { get; set; } = 8;
        public bool IsNetPollutionDisabled => !_netPollutionSystem;
        
        
        //FADES
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, FadesGroup)]
        [SettingsUISlider(min = 0, max = 200, step = 1, unit = Unit.kPercentage)]
        public float AirFade { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, FadesGroup)]
        [SettingsUISlider(min = 0, max = 200, step = 1, unit = Unit.kPercentage)]
        public float GroundFade { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, FadesGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1)]
        public float PlantFade { get; set; } = 2;

        //NOTIFICATION LIMITS
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, NotificationLimitsGroup)]
        [SettingsUISlider(min = 0, max = -100f, step = 1)]
        public float AirPollutionNotificationLimit { get; set; } = -7;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, NotificationLimitsGroup)]
        [SettingsUISlider(min = 0, max = -100f, step = 1)]
        public float NoisePollutionNotificationLimit { get; set; } = -7;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, NotificationLimitsGroup)]
        [SettingsUISlider(min = 0, max = -100f, step = 1)]
        public float GroundPollutionNotificationLimit { get; set; } = -7;

        //OTHER PARAMETERS
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 10, step = 1)]
        public float WindAdvectionSpeed { get; set; } = 20;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 10, step = 0.5f)]
        public float DistanceExponent { get; set; } = 1.5f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1)]
        public float HomelessNoisePollution { get; set; } = 50;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public float GroundPollutionLandValueDivsor { get; set; } = 100;

     
        public override void SetDefaults()
        {
            _noisePollutionSystem = false;
            _netPollutionSystem = false;
            _buildingPollutionAddSystem = false;
            _groundPollutionSystem = false;
            _groundwaterPollutionSystem = true;
            _airPollutionSystem = false;
            WaterPollutionDecayRateSlider = 10;
            
            
        }
        public class LocaleEN : IDictionarySource
        {
            public readonly Setting m_Setting;
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
                    { m_Setting.GetOptionTabLocaleID(ParametersTab), "Parameters" },

                    //GROUPS
                    { m_Setting.GetOptionGroupLocaleID(NoisePollutionGroup), "Noise Pollution" },
                    { m_Setting.GetOptionGroupLocaleID(NetPollutionGroup), "Net pollution" },
                    { m_Setting.GetOptionGroupLocaleID(BuildingPollutionGroup), "Building pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundPollutionGroup), "Ground pollution" },
                    { m_Setting.GetOptionGroupLocaleID(GroundwaterPollutionGroup), "Groundwater pollution" },
                    { m_Setting.GetOptionGroupLocaleID(AirPollutionGroup), "Air pollution" },
                    { m_Setting.GetOptionGroupLocaleID(WaterPollutionGroup), "Water pollution" },
                    //PARAMETERS GROUPS
                    { m_Setting.GetOptionGroupLocaleID(MultipliersGroup), "Multiplier Parameters" },
                    { m_Setting.GetOptionGroupLocaleID(RadiusGroup), "Radius Parameters" },
                    { m_Setting.GetOptionGroupLocaleID(FadesGroup), "Fades Parameters" },
                    { m_Setting.GetOptionGroupLocaleID(NotificationLimitsGroup), "Notification limit Parameters" },
                    { m_Setting.GetOptionGroupLocaleID(OtherParametersGroup), "Other Parameters" },

                    //MAIN TAB OPTIONS
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionToggle)), "Noise pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionToggle)), "Enable/Disable noise pollution system (when unchecked it will stop noise pollution from being produced on the map and should reset the noise pollution back to 0)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionToggle)), "Ground pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionToggle)), "Enable/Disable ground pollution system(when unchecked it will stop ground pollution from being produced on the map and should reset the ground pollution back to 0)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionToggle)), "Air pollution system" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionToggle)), "Enable/Disable ground pollution system(when unchecked it will stop air pollution from being produced on the map and should reset the air pollution back to 0)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionResetButton)), "Noise pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionResetButton)), "This will reset the noise pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of noise pollution would be 1000 and pressing this button would reset the noise pollution back to 0" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionToggle)), "Net pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionToggle)), "Enable/Disable net pollution (when unchecked it will stop net pollution from being produced on the map, in order to fully turn of net pollution air and noise pollution systems have to be turned of as well)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(BuildingPollutionToggle)), "Building pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(BuildingPollutionToggle)), "Enable/Disable building pollution (when unchecked it will stop building pollution from being produced on the map, building pollution is also tied to noise, air and ground pollution so in some cases even if you have noise, air and ground pollution on buildings may still produce pollution)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionResetButton)), "Ground pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionResetButton)), "This will reset the ground pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of ground pollution would be 1000 and pressing this button would reset the ground pollution back to 0" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundWaterPollutionToggle)), "Ground water pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundWaterPollutionToggle)), "Enable/Disable ground water pollution (when unchecked it will stop ground water pollution from being produced on the map)" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionResetButton)), "Air pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionResetButton)), "This will reset the air pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of air pollution would be 1000 and pressing this button would reset the air pollution back to 0" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Instant decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Set water pollution decay rate to 1000" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayRateSlider)), "Water pollution decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayRateSlider)), "Set water pollution decay rate" },

                    //PARAMETERS TAB OPTIONS
                    //Multiplier Options
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundMultiplier)), "Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundMultiplier)), "Set ground pollution multiplier. Base value and default value are 20. 0 means no ground pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirMultiplier)), "Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirMultiplier)), "Set air pollution multiplier. Base value and default value are 25. 0 means no air pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoiseMultiplier)), "Noise Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoiseMultiplier)), "Set ground pollution multiplier. Base value and default value are 250. 0 means no noise pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetAirMultiplier)), "Net Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetAirMultiplier)), "Set net air pollution multiplier. Base value and default value are 1.75. 0 means no net air pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseMultiplier)), "Net Noise Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseMultiplier)), "Set ground pollution multiplier. Base value and default value are 5. 0 means no net noise pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantAirMultiplier)), "Plant Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantAirMultiplier)), "Set plant air multiplier. Base value and default value are 0.001. 0 means no plant air multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantGroundMultiplier)), "Plant Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantGroundMultiplier)), "Set plant ground multiplier. Base value and default value are 0.001. 0 means no plant ground multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FertilityGroundMultiplier)), "Fertility Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(FertilityGroundMultiplier)), "Set Fertility Ground Multiplier Base value and default value are 1. 0 means no fertility ground multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AbandonedNoisePollutionMultiplier)), "Abandoned Noise Pollution Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AbandonedNoisePollutionMultiplier)), "Set Abandoned Noise Pollution Multiplier. Base value and default value are 5. 0 means no Abandoned Noise Pollution multiplier" },

                    //Radius Options
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundRadius)), "Ground Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundRadius)), "Set ground pollution radius. Base value and default value are 500 (100%). 0 means no ground pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirRadius)), "Air Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirRadius)), "Set air pollution radius. Base value and default value are 100. 0 means no air pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoiseRadius)), "Noise Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoiseRadius)), "Set noise pollution radius. Base value and default value are 400 (100%). 0 means no noise pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseRadius)), "Net Noise Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseRadius)), "Set net air pollution radius. Base value and default value are 8. 0 means no net air pollution radius" },

                    //Fades Options
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirFade)), "Air Pollution Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirFade)), " Set air pollution fade. Base value and default value are 8000 (100%). 0 (0%) means no air pollution fade 16000 (200%) means an increased air pollution fade" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundFade)), "Ground Pollution Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundFade)), "Set ground pollution fade. Base value and default value are 4000 (100%). 0 (0%) means no air pollution fade 8000 (200%) means an increased ground pollution fade" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantFade)), "Plant Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantFade)), "Set plant fade. Base value and default value are 2. 0 means no plant fade" },

                    //Notifications limits Options
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionNotificationLimit)), "Air Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionNotificationLimit)), "If happiness effect from air pollution is less than this, the notification will be shown. Base value and default value are -7." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionNotificationLimit)), "Noise Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionNotificationLimit)), "If happiness effect from noise pollution is less than this, the notification will be shown. Base value and default value are -7." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionNotificationLimit)), "Ground Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionNotificationLimit)), "If happiness effect from ground pollution is less than this, the notification will be shown. Base value and default value are -7." },
                  
                    //Other Options
                    { m_Setting.GetOptionLabelLocaleID(nameof(WindAdvectionSpeed)), "Wind Advection Speed" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WindAdvectionSpeed)), "The speed at which wind travels from one place to another, this parameter most likely has to with how fast air pollution travels to. Base value and default value are 20." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(DistanceExponent)), "Distance Exponent" },
                    { m_Setting.GetOptionDescLocaleID(nameof(DistanceExponent)), "The exponent of the distance between air pollution (The distnce at which noise pollution clouds will fall of compared to its source. Base value and default value are 1.5" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(HomelessNoisePollution)), "Homeless Noise Pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(HomelessNoisePollution)), "Set the noise pollution of the homeless. Base value and default value are 50. 0 means no homeless noise pollution" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionLandValueDivsor)), "Ground Pollution Land Value Divsor" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionLandValueDivsor)), "The divisor is the pollution value. This means the land value will decrease based on this pollution value. So, if you have a pollution value, you divide by this number to see how much it negatively affects the land value. The higher the pollution, the more the land value goes down. Base and default value are 500." },


                };  
            }

            public void Unload() { }
        }


    }

}
