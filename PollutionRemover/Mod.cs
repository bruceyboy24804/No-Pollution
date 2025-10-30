namespace NoPollution
{
    using System;
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Simulation;
    using Unity.Entities;
    using NoPollution.ResetSystems;
    using NoPollution.Querys;
    using Game.Settings;
    using Game.Debug;
    using Game.Prefabs;
    using static NoPollution.Setting;
    using NoPollution.Systems;

    /// <summary>
    /// The base mod class for NoPollution mod.
    /// </summary>
    public sealed class Mod : IMod
    {
        /// <summary>
        /// Gets the active instance of the mod.
        /// </summary>
        public static Mod Instance { get; private set; }

        /// <summary>
        /// Gets or sets the active settings for the mod.
        /// </summary>
        internal Setting ActiveSettings { get; private set; }

        /// <summary>
        /// Gets or sets the active world for the mod.
        /// </summary>
        internal static World ActiveWorld { get; private set; }

        /// <summary>
        /// Logger for the mod.
        /// </summary>
        public static ILog Log { get; private set; } = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        // Systems managed by the mod
        public static Setting m_Setting;
        public static PrefabSystem _prefabSystem;
        public static DebugSystem _debugSystem;
      

        /// <summary>
        /// Called by the game when the mod is loaded.
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        public void OnLoad(UpdateSystem updateSystem)
        {
            // Set the active instance and world.
            Instance = this;
            ActiveWorld = updateSystem.World;

            Log.Info(nameof(OnLoad));

            // Initialize systems
            InitializeSystems(updateSystem);

            // Load and register settings
            m_Setting = new Setting(this);
            if (m_Setting == null)
            {
                Log.Error("Failed to initialize settings.");
                return;
            }
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(nameof(NoPollution), m_Setting, new Setting(this));

            // Load localization
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            // Assign world to reset systems
            NoisePollutionResetSystem.World = updateSystem.World;
            GroundPollutionResetSystem.World = updateSystem.World;
            AirPollutionResetSystem.World = updateSystem.World;
            
            // Register update phases
          
            updateSystem.UpdateAt<PollutionParameterDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<PollutionDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<NetPollutionDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<GroundWaterPollutionReductionSystem>(SystemUpdatePhase.GameSimulation);



            // Log mod load completion
            Log.Info($"Loaded {nameof(NoPollution)} mod successfully.");
        }


        /// <summary>
        /// Initializes the necessary systems for the mod.
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        private void InitializeSystems(UpdateSystem updateSystem)
        {
            _debugSystem = updateSystem.World.GetOrCreateSystemManaged<DebugSystem>();
           
          
        }

        /// <summary>
        /// Called by the game when the mod is disposed of.
        /// </summary>
        public void OnDispose()
        {
            Log.Info(nameof(OnDispose));

            // Unregister and clear settings
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }

            // Clear the instance
            Instance = null;

            Log.Info($"{nameof(NoPollution)} mod disposed successfully.");
        }
    }
}
