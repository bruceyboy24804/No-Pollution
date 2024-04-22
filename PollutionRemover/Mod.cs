using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using UnityEngine.PlayerLoop;
using Unity.Entities;
using Colossal.IO.AssetDatabase;
using Game.Debug;
using System.Runtime.InteropServices;



namespace NoPollution
{
    public class Mod : IMod
    {
        public static DebugSystem _debugSystem;
        public static PrefabSystem _prefabSystem;
        public static NoisePollutionSystem _noisePollutionSystem;
        public static NetPollutionSystem _netPollutionSystem;
        public static BuildingPollutionAddSystem _buildingPollutionAddSystem;
        public static GroundPollutionSystem _groundPollutionSystem;
        public static GroundWaterPollutionSystem _groundWaterPollutionSystem;
        public static WaterPipePollutionSystem _waterPipePollutionSystem;
        public static AirPollutionSystem _airPollutionSystem;
        internal ModSettings ActiveSettings { get; set; }
        internal static World ActiveWorld { get; private set; }



         internal ModSettings activeSettings { get; set; }
       

        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {

            ActiveWorld = updateSystem.World;
            _debugSystem = updateSystem.World.GetOrCreateSystemManaged<DebugSystem>();
            _noisePollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
            _netPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NetPollutionSystem>();
            _buildingPollutionAddSystem = updateSystem.World.GetOrCreateSystemManaged<BuildingPollutionAddSystem>();
            _groundPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
            _groundWaterPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundWaterPollutionSystem>();
            _waterPipePollutionSystem = updateSystem.World.GetOrCreateSystemManaged<WaterPipePollutionSystem>();
            _airPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<AirPollutionSystem>();
          
            _debugSystem = updateSystem.World.GetOrCreateSystemManaged<DebugSystem>();


            ModSettings activeSettings = new(this);
            activeSettings.RegisterInOptionsUI();

            Localization.LoadTranslations(activeSettings, log);


            AssetDatabase.global.LoadSettings("NoPollution", activeSettings, new ModSettings(this));


            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }

    }
}