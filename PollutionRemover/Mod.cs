using NoPollution.Domain;

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
    using World = Unity.Entities.World;
    
    public sealed class Mod : IMod
    {
        public static Mod Instance { get; private set; }
        internal Setting ActiveSettings { get; private set; }
        internal static World ActiveWorld { get; private set; }
        public static ILog Log { get; private set; } = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        // Systems managed by the mod
        public static Setting m_Setting;
        public static PrefabSystem _prefabSystem;
        public static DebugSystem _debugSystem;
        
        public void OnLoad(UpdateSystem updateSystem)
        {
            Instance = this;
            ActiveWorld = updateSystem.World;

            Log.Info(nameof(OnLoad));


            // Load and register settings
            m_Setting = new Setting(this);
            if (m_Setting == null)
            {
                Log.Error("Failed to initialize settings.");
                return;
            }
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(nameof(NoPollution), m_Setting, new Setting(this));

            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            m_Setting.VanillaDataFromStorage = VanillaDataStorage.VanillaData;
            // Assign world to reset systems
            NoisePollutionResetSystem.World = updateSystem.World;
            GroundPollutionResetSystem.World = updateSystem.World;
            AirPollutionResetSystem.World = updateSystem.World;

          
            updateSystem.UpdateAt<PollutionParameterDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<PollutionDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<NetPollutionDataQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<GroundWaterPollutionReductionSystem>(SystemUpdatePhase.GameSimulation);

            Log.Info($"Loaded {nameof(NoPollution)} mod successfully.");
        }
        
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
