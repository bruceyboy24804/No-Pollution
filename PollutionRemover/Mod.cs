using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using Game;
using NoPollution.ResetSystems;
using static NoPollution.Setting;
using Unity.Entities;
using System;
using Game.Settings;



namespace NoPollution
{
    public sealed class Mod : IMod
    {
        // Static fields
        public static Mod Instance { get; private set; }
        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        public static Setting m_Setting;
        public static PrefabSystem _prefabSystem;
        public static NoisePollutionSystem _noisePollutionSystem;
        public static NetPollutionSystem _netPollutionSystem;
        public static BuildingPollutionAddSystem _buildingPollutionAddSystem;
        public static GroundPollutionSystem _groundPollutionSystem;
        public static GroundWaterPollutionSystem _groundWaterPollutionSystem;
        public static AirPollutionSystem _airPollutionSystem;
        public static WaterSystem _waterSystem;

        // Instance fields
        private Mod instance;

        // Properties
        internal Setting activeSettings { get; private set; }
        internal static World ActiveWorld { get; private set; }
        public World World { get; private set; }

        // Methods
        /// <summary>
        /// Called by the game when the mod is loaded.
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        public void OnLoad(UpdateSystem updateSystem)
        {
            instance = this;
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                log.Info($"Current mod asset at {asset.path}");
            }

            ActiveWorld = updateSystem.World;
            
            _noisePollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
            _netPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NetPollutionSystem>();
            _buildingPollutionAddSystem = updateSystem.World.GetOrCreateSystemManaged<BuildingPollutionAddSystem>();
            _groundPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
            _groundWaterPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundWaterPollutionSystem>();
            _airPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<AirPollutionSystem>();
            _waterSystem = updateSystem.World.GetOrCreateSystemManaged<WaterSystem>();

            NoisePollutionResetSystem.World = updateSystem.World;
            GroundPollutionResetSystem.World = updateSystem.World;
            AirPollutionResetSystem.World = updateSystem.World;

            // Ensure ActiveSettings and m_Setting are properly initialized
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(nameof(NoPollution), m_Setting, new Setting(this));
            

            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            updateSystem.UpdateAt<PollutionModiferDataQuery>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAt<PollutionModiferDataQuery>(SystemUpdatePhase.PrefabReferences);

            
        }



        /// <summary>
        /// Called by the game when the mod is disposed of.
        /// </summary>
        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}
