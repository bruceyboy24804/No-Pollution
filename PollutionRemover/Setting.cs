using System;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Objects;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Settings;
using Game.Simulation;
using Game.UI;
using NoPollution.ResetSystems;
//using NoPollution.Systems;
using System.Collections.Generic;
using Unity.Entities;
using static Colossal.IO.AssetDatabase.AssetDatabase;
using Game.UI.Debug;
using Unity.Collections;
using Unity.Jobs;
using NoPollution.Querys;
using Game.UI.Widgets;
using NoPollution.Domain;
using NoPollution.Systems;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUITabOrder(MainTab, ParametersTab)]
    [SettingsUIGroupOrder(NoisePollutionGroup, NetPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, MultipliersGroup, RadiusGroup, FadesGroup, NotificationLimitsGroup, OtherParametersGroup)]
    [SettingsUIShowGroupName(NoisePollutionGroup, NetPollutionGroup, GroundPollutionGroup, GroundwaterPollutionGroup, AirPollutionGroup, WaterPollutionGroup, MultipliersGroup, RadiusGroup, FadesGroup, NotificationLimitsGroup, OtherParametersGroup)]
    public class Setting : ModSetting
    {


        // Static fields
        public static World World { get; set; }

        // Constants
        public const string MainTab = "Main";
        public const string NoisePollutionGroup = "Noise Pollution";
        public const string NetPollutionGroup = "Net Pollution";
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

        // Instance fields
       
       
       
       
        private bool _waterPollutionDecayInstantToggle;
        private int _waterPollutionDecayRate = 10;
        private readonly double defaultGroundPollutionPercentage = 100.0;
        private readonly double defaultAirPollutionPercentage = 100.0;
        private readonly double defaultNoisePollutionPercentage = 100.0;
        private readonly int defaultNetPollutionPercentage = 100;
        public bool _airPollutionToggle;
        public bool _groundPollutionToggle;
        public bool _noisePollutionToggle;
        public bool _netPollutionToggle;
        public bool _groundWaterPollutionToggle;
      
        // Constructor
        public Setting(IMod mod) : base(mod) { }

        

        // Properties

       

        /// <summary>
        /// Noise pollution group
        /// </summary>

        [SettingsUISection(MainTab, NoisePollutionGroup)]
        public bool NoisePollutionToggle
        {
            get => _noisePollutionToggle;
            set
            {
                _noisePollutionToggle = value;
                if (!value)
                {
                    NoisePollutionSlider = 100f;
                }
                else
                {
                    NoisePollutionSlider = 0f;
                }
    
            }
        }
        [SettingsUISection(MainTab, NoisePollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NoisePollutionToggle))]
        public float NoisePollutionSlider { get; set; } = 100f;

        [SettingsUISection(MainTab, NoisePollutionGroup)]
        [SettingsUIButton]
        public bool NoisePollutionResetButton
        {
            set => NoisePollutionResetSystem.ResetPollution();
        }

        /// <summary>
        /// Ground pollution group
        /// </summary>

        [SettingsUISection(MainTab, GroundPollutionGroup)]
        public bool GroundPollutionToggle
        {
            get => _groundPollutionToggle;
            set
            {
                _groundPollutionToggle = value;
               if (!value)
                {
                    GroundPollutionSlider = 100f;
                }
                else
                {
                    GroundPollutionSlider = 0f;
                }
            }
        }
        [SettingsUISection(MainTab, GroundPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(GroundPollutionToggle))]
        public float GroundPollutionSlider { get; set; } = 100f;
       
        [SettingsUISection(MainTab, GroundPollutionGroup)]
        [SettingsUIButton]
        public bool GroundPollutionResetButton
        {
           
            set => GroundPollutionResetSystem.ResetPollution();
        }

        /// <summary>
        /// Air pollution group
        /// </summary>

        [SettingsUISection(MainTab, AirPollutionGroup)]
        public bool AirPollutionToggle
        {
            get => _airPollutionToggle;
            set
            {
                _airPollutionToggle = value;
                if (!value)
                {
                    AirPollutionSlider = 100f;
                }
                else
                {
                    AirPollutionSlider = 0f;
                }


            }
        }
        [SettingsUISection(MainTab, AirPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(AirPollutionToggle))]
        public float AirPollutionSlider { get; set; } = 100f;
        
        [SettingsUISection(MainTab, AirPollutionGroup)]
        [SettingsUIButton]
        public bool AirPollutionResetButton
        {
            set => AirPollutionResetSystem.ResetPollution();
        }


        [SettingsUISection(MainTab, NetPollutionGroup)]
        public bool NetPollutionToggle
        {
            get => _netPollutionToggle;
            set
            {
                _netPollutionToggle = value;
                if (!value)
                {
                    NetPollutionSlider1 = 100f;
                    NetPollutionSlider2 = 100f;
                    NetPollutionAccumulationSlider1 = 100f;
                    NetPollutionAccumulationSlider2 = 100f;
                }
                else
                {
                    NetPollutionSlider1 = 0f;
                    NetPollutionSlider2 = 0f;
                    NetPollutionAccumulationSlider1 = 0f;
                    NetPollutionAccumulationSlider2 = 0f;
                }

            }
        }
        [SettingsUISection(MainTab, NetPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NetPollutionToggle))]
        public float NetPollutionSlider1  { get; set; } = 100f;
        [SettingsUISection(MainTab, NetPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NetPollutionToggle))]
        public float NetPollutionSlider2  { get; set; } = 100f;
        [SettingsUISection(MainTab, NetPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NetPollutionToggle))]
        public float NetPollutionAccumulationSlider1 { get; set; } = 100f;
        [SettingsUISection(MainTab, NetPollutionGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1f, unit = Unit.kPercentage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(NetPollutionToggle))]
        public float NetPollutionAccumulationSlider2 { get; set; } = 100f;
        [SettingsUISection(MainTab, NetPollutionGroup)]
        [SettingsUIButton]
        public bool NetPollutionResetButton
        {
            set
            {
                if (value)
                {
                    var world = World.DefaultGameObjectInjectionWorld;
                    var netPollutionSystem = world.GetExistingSystemManaged<NetPollutionDataQuery>();

                    if (netPollutionSystem != null)
                    {
                        netPollutionSystem.ResetPollutionToOriginal();
                    }
                }
            }
        }


        [SettingsUISection(MainTab, GroundwaterPollutionGroup)]
        public GroundWaterPollutionReductionRate GroundWaterPollutionReductionRate { get; set; } = GroundWaterPollutionReductionRate.NoReduction;





        [SettingsUISection(MainTab, WaterPollutionGroup)]
        public bool WaterPollutionDecayInstantToggle
        {
            get => _waterPollutionDecayInstantToggle;
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
                if (waterSystem != null)
                {
    
    
                    waterSystem.WaterSimulation.PollutionDecayRate = _waterPollutionDecayRate * 0.001f;
    
                }


            }
        }
        
        // Multipliers

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
        [SettingsUISlider(min = 0, max = 1.75f, step = 0.5f, unit = Unit.kFloatSingleFraction)]
        public float NetAirMultiplier { get; set; } = 1.75f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 5, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        public float NetNoiseMultiplier { get; set; } = 5;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 0.001f, step = 0.001f, unit = Unit.kFloatThreeFractions)]
        public float PlantAirMultiplier { get; set; } = 0.001f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 0.001f, step = 0.001f, unit = Unit.kFloatThreeFractions)]
        public float PlantGroundMultiplier { get; set; } = 0.001f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 1, step = 0.1f, unit = Unit.kFloatSingleFraction)]
        public float FertilityGroundMultiplier { get; set; } = 1;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, MultipliersGroup)]
        [SettingsUISlider(min = 0, max = 5, step = 0.1f, unit = Unit.kFloatSingleFraction)]
        public float AbandonedNoisePollutionMultiplier { get; set; } = 5;

        //RADIUS
        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1)]

        public float AirRadius { get; set; } = 100;


        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public float GroundRadius { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]

        public float NoiseRadius { get; set; } = 100;



        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, RadiusGroup)]
        [SettingsUISlider(min = 0, max = 8, step = 0.1f, unit = Unit.kFloatSingleFraction)]

        public float NetNoiseRadius { get; set; } = 8;



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
        [SettingsUISlider(min = 0, max = 10, step = 0.5f, unit = Unit.kFloatSingleFraction)]
        public float DistanceExponent { get; set; } = 1.5f;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1)]
        public float HomelessNoisePollution { get; set; } = 50;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 1, unit = Unit.kPercentage)]
        public float GroundPollutionLandValueDivisor { get; set; } = 100;

        [SettingsUIAdvanced]
        [SettingsUISection(ParametersTab, OtherParametersGroup)]
        [SettingsUIButton]
        public bool ResetAdvancedParametersButton
        {
            set
            {
                // Reset only advanced parameters
                ResetAdvancedParameters();
            }
        }

       
        // Methods
        private void ResetAdvancedParameters()
        {
            // Reset multipliers
            GroundMultiplier = 20;
            AirMultiplier = 25;
            NoiseMultiplier = 100;
            NetAirMultiplier = 1.75f;
            NetNoiseMultiplier = 5;
            PlantAirMultiplier = 0.001f;
            PlantGroundMultiplier = 0.001f;
            FertilityGroundMultiplier = 1;
            AbandonedNoisePollutionMultiplier = 5;

            // Reset radii
            AirRadius = 100;
            GroundRadius = 100;
            NoiseRadius = 100;
            NetNoiseRadius = 8;

            // Reset fades
            AirFade = 100;
            GroundFade = 100;
            PlantFade = 2;

            // Reset notification limits
            AirPollutionNotificationLimit = -7;
            NoisePollutionNotificationLimit = -7;
            GroundPollutionNotificationLimit = -7;

            // Reset other advanced parameters
            WindAdvectionSpeed = 20;
            DistanceExponent = 1.5f;
            HomelessNoisePollution = 50;
            GroundPollutionLandValueDivisor = 100;
        }

        public override void SetDefaults()
        {
            Setting setting = Mod.m_Setting;
            setting.GroundPollutionSlider = (float)defaultGroundPollutionPercentage;
            setting.AirPollutionSlider = (float)defaultAirPollutionPercentage;
            setting.NoisePollutionSlider = (float)defaultNoisePollutionPercentage;
            setting.NetPollutionSlider1 = (float)defaultNetPollutionPercentage;
            setting.NetPollutionSlider2 = (float)defaultNetPollutionPercentage;
            setting.NetPollutionAccumulationSlider1 = (float)defaultNetPollutionPercentage;
            setting.NetPollutionAccumulationSlider2 = (float)defaultNetPollutionPercentage;

            WaterPollutionDecayRateSlider = 10;
        }

        // Locale Class
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

                    // Group Labels
                    { m_Setting.GetOptionGroupLocaleID(Setting.NoisePollutionGroup), "Noise Pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.NetPollutionGroup), "Net pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.GroundPollutionGroup), "Ground pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.GroundwaterPollutionGroup), "Groundwater pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.AirPollutionGroup), "Air pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.WaterPollutionGroup), "Water pollution" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.MultipliersGroup), "Multipliers" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.RadiusGroup), "Radii's" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.FadesGroup), "Fades" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.NotificationLimitsGroup), "Notification Limits" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.OtherParametersGroup), "Other" },
                    
                    
                    
                    // Air Pollution Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionToggle)), "Air pollution producers" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionToggle)), "When enabled air pollution from entities that cause air pollution will be set to 0 and disables the air pollution slider" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionSlider)), "Air pollution slider" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionSlider)), "Gives control over how much should entiites that cause air pollution be set to" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionResetButton)), "Air pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionResetButton)), "This will reset the air pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of air pollution would be 1000 and pressing this button would reset the air pollution back to 0" },
                    

                    // Net Pollution Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionToggle)), "Net pollution producers" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionToggle)), "When enabled net pollution from net entities (for example roads) that cause net pollution will be set to 0 and disables the net pollution slider" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionSlider1)), "Net pollution slider (X)" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionSlider1)), "Gives control over how much should entiites that cause net pollution be set to" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionSlider2)), "Net pollution slider (Y)" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionSlider2)), "Gives control over how much should entiites that cause net pollution be set to" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionAccumulationSlider1)), "Net pollution accumulation slider (X)" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionAccumulationSlider1)), "Adjusts the accumulation rate of pollution generated by net entities, such as roads and other infrastructure. Use this slider to control the impact of pollution accumulation over time." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionAccumulationSlider2)), "Net pollution accumulation slider (Y)" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionAccumulationSlider2)), "Adjusts the accumulation rate of pollution generated by net entities, such as roads and other infrastructure. Use this slider to control the impact of pollution accumulation over time." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetPollutionResetButton)), "Net pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetPollutionResetButton)), "Reset the net pollution that is seen on the map" },
                    
                    // Noise Pollution Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionToggle)), "Noise pollution producers" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionToggle)), "When enabled noise pollution from entities that cause noise pollution will be set to 0 and disables the noise pollution slider" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionSlider)), "Noise pollution slider" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionSlider)), "Gives control over how much should entiites that cause noise pollution be set to" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionResetButton)), "Noise pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionResetButton)), "This will reset the noise pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of noise pollution would be 1000 and pressing this button would reset the noise pollution back to 0" },

                    // Ground Pollution Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionToggle)), "Ground pollution producers" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionToggle)), "When enabled ground pollution from entities that cause ground pollution will be set to 0 and disables the ground pollution slider" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionSlider)), "Ground pollution slider" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionSlider)), "Gives control over how much should entiites that cause ground pollution be set to" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionResetButton)), "Ground pollution Reset" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionResetButton)), "This will reset the ground pollution that is seen on the map so pollution will build up over time. So a hypothetical amount of ground pollution would be 1000 and pressing this button would reset the ground pollution back to 0" },

                    // Ground Water Pollution Group
                   { m_Setting.GetOptionLabelLocaleID(nameof(GroundWaterPollutionReductionRate)), "Groundwater Pollution Reduction" },
                   { m_Setting.GetOptionDescLocaleID(nameof(GroundWaterPollutionReductionRate)), "Adjust the speed at which groundwater pollution is reduced. Choose between no reduction, normal, fast, or instant reduction." },
                   { m_Setting.GetEnumValueLocaleID(GroundWaterPollutionReductionRate.NoReduction), "No Reduction" },
                   { m_Setting.GetEnumValueLocaleID(GroundWaterPollutionReductionRate.NormalReduction), "Normal Reduction" },
                   { m_Setting.GetEnumValueLocaleID(GroundWaterPollutionReductionRate.FastReduction), "Fast Reduction" },
                   { m_Setting.GetEnumValueLocaleID(GroundWaterPollutionReductionRate.InstantReduction), "Instant Reduction" },

                   
                   
                    
                    // Water Pollution Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Instant decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayInstantToggle)), "Set water pollution decay rate to 1000" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(WaterPollutionDecayRateSlider)), "Water pollution decay rate" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WaterPollutionDecayRateSlider)), "Set water pollution decay rate" },

                    // Parameters Tab - Multipliers Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundMultiplier)), "Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundMultiplier)), "Set ground pollution multiplier. Base value and default value are 20. 0 means no ground pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirMultiplier)), "Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirMultiplier)), "Set air pollution multiplier. Base value and default value are 25. 0 means no air pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoiseMultiplier)), "Noise Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoiseMultiplier)), "Set noise pollution multiplier. Base value and default value are 100. 0 means no noise pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetAirMultiplier)), "Net Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetAirMultiplier)), "Set net air pollution multiplier. Base value and default value are 1.75. 0 means no net air pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseMultiplier)), "Net Noise Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseMultiplier)), "Set net noise pollution multiplier. Base value and default value are 5. 0 means no net noise pollution multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantAirMultiplier)), "Plant Air Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantAirMultiplier)), "Set plant air multiplier. Base value and default value are 0.001. 0 means no plant air multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantGroundMultiplier)), "Plant Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantGroundMultiplier)), "Set plant ground multiplier. Base value and default value are 0.001. 0 means no plant ground multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(FertilityGroundMultiplier)), "Fertility Ground Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(FertilityGroundMultiplier)), "Set Fertility Ground Multiplier Base value and default value are 1. 0 means no fertility ground multiplier" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AbandonedNoisePollutionMultiplier)), "Abandoned Noise Pollution Multiplier" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AbandonedNoisePollutionMultiplier)), "Set Abandoned Noise Pollution Multiplier. Base value and default value are 5. 0 means no Abandoned Noise Pollution multiplier" },

                    // Parameters Tab - Radius Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundRadius)), "Ground Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundRadius)), "Set ground pollution radius. Base value and default value are 100. 0 means no ground pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirRadius)), "Air Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirRadius)), "Set air pollution radius. Base value and default value are 100. 0 means no air pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoiseRadius)), "Noise Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoiseRadius)), "Set noise pollution radius. Base value and default value are 100. 0 means no noise pollution radius" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NetNoiseRadius)), "Net Noise Pollution Radius" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NetNoiseRadius)), "Set net noise pollution radius. Base value and default value are 8. 0 means no net noise pollution radius" },

                    // Parameters Tab - Fades Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirFade)), "Air Pollution Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirFade)), "Set air pollution fade. Base value and default value are 100. 0 (0%) means no air pollution fade. 200% means an increased air pollution fade" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundFade)), "Ground Pollution Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundFade)), "Set ground pollution fade. Base value and default value are 100. 0 (0%) means no ground pollution fade. 200% means an increased ground pollution fade" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(PlantFade)), "Plant Fade" },
                    { m_Setting.GetOptionDescLocaleID(nameof(PlantFade)), "Set plant fade. Base value and default value are 2. 0 means no plant fade" },

                    // Parameters Tab - Notification Limits Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(AirPollutionNotificationLimit)), "Air Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(AirPollutionNotificationLimit)), "If happiness effect from air pollution is less than this, the notification will be shown. Base value and default value are -7." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(NoisePollutionNotificationLimit)), "Noise Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(NoisePollutionNotificationLimit)), "If happiness effect from noise pollution is less than this, the notification will be shown. Base value and default value are -7." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionNotificationLimit)), "Ground Pollution Notification Limit" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionNotificationLimit)), "If happiness effect from ground pollution is less than this, the notification will be shown. Base value and default value are -7." },

                    // Parameters Tab - Other Parameters Group
                    { m_Setting.GetOptionLabelLocaleID(nameof(WindAdvectionSpeed)), "Wind Advection Speed" },
                    { m_Setting.GetOptionDescLocaleID(nameof(WindAdvectionSpeed)), "The speed at which wind travels from one place to another. This parameter likely affects how fast air pollution travels too. Base value and default value are 20." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(DistanceExponent)), "Distance Exponent" },
                    { m_Setting.GetOptionDescLocaleID(nameof(DistanceExponent)), "The exponent of the distance between air pollution sources. The distance at which noise pollution clouds will fall off compared to their source. Base value and default value are 1.5." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(HomelessNoisePollution)), "Homeless Noise Pollution" },
                    { m_Setting.GetOptionDescLocaleID(nameof(HomelessNoisePollution)), "Set the noise pollution of the homeless. Base value and default value are 50. 0 means no homeless noise pollution" },
                    { m_Setting.GetOptionLabelLocaleID(nameof(GroundPollutionLandValueDivisor)), "Ground Pollution Land Value Divisor" },
                    { m_Setting.GetOptionDescLocaleID(nameof(GroundPollutionLandValueDivisor)), "The divisor is the pollution value. The higher the pollution, the more the land value goes down. Base and default value are 500." },
                    { m_Setting.GetOptionLabelLocaleID(nameof(ResetAdvancedParametersButton)), "Reset Advanced Parameters" },
                    { m_Setting.GetOptionDescLocaleID(nameof(ResetAdvancedParametersButton)), "This will reset all advanced parameters (multipliers, radii, fades, etc.) to their default settings." },
                };
            }

            public void Unload() { }
        }
    }
}
