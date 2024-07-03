
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.Tools;
using System;


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

        private float _m_waterPollutionDecayRate = 1f;


        private bool _noisePollutionSystem = true;
        private bool _netPollutionSystem = true;
        private bool _buildingPollutionAddSystem = true;
        private bool _groundPollutionSystem = true;
        private bool _groundwaterPollutionSystem = true;
        private bool _waterPipePollutionSystem = true;
        private bool _airPollutionSystem = true;
        private float _waterPollutionDecayRate = 1f;

        public ModSettings(IMod mod) : base(mod)
        {
            // Initialize the slider value with the current field value from PollutionDecayRate
            WaterPollutionDecayRate = PollutionDecayRate.GetPollutionDecayRate() * 100f;
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
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, scalarMultiplier = 1f)]
        [SettingsUISection(WaterPollution)]
        public float WaterPollutionDecayRate
        {
            get => _waterPollutionDecayRate;
            set
            {
                log.Info($"Setting WaterPollutionDecayRate to: {value}");
                _waterPollutionDecayRate = value;
                // Update the m_PollutionDecayRate field using PollutionDecayRate
                PollutionDecayRate.SetPollutionDecayRate(WaterPollutionDecayRate);
                log.Info($"WaterPollutionDecayRate set to: {_waterPollutionDecayRate}");
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
            _waterPollutionDecayRate = 50f;

            log.Info("Setting default values...");
            // Reset the m_PollutionDecayRate field to its default value using WaterPollutionManager
            PollutionDecayRate.SetPollutionDecayRate(_waterPollutionDecayRate);
            log.Info("SetDefaults called. Reset WaterPollutionDecayRate to default (50)");
        }

        public void Unload()
        {

        }

    }

}
 