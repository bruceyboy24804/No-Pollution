
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.Simulation;
using Game.UI;


namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIShowGroupName(Resetting, PollutionToggles)]
    [SettingsUITabOrder(Resetting, PollutionToggles)]
    [SettingsUIGroupOrder(Resetting, PollutionToggles)]

    public class ModSettings : ModSetting
    {
        private const string Resetting = "Resetting";
        private const string PollutionToggles = "PollutionToggles";
       

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
       


        public override void SetDefaults()
        {
            _noisePollutionSystem = true;
            _netPollutionSystem = true;
            _buildingPollutionAddSystem = true;
            _groundPollutionSystem = true;
            _groundwaterPollutionSystem = true;
            _waterPipePollutionSystem = true;
            _airPollutionSystem = true;

          

        }
        public void Unload()
        {

        }
    }
}