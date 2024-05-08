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
using Unity.Jobs;
using static Game.Rendering.Debug.RenderPrefabRenderer;
using static Colossal.IO.AssetDatabase.AssetDatabase;



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
        public static NoisePollutionSystem _noisepollutionMofiier;
       

        internal ModSettings ActiveSettings { get; set; }
        internal static World ActiveWorld { get; private set; }
        public World World { get; private set; }


        internal ModSettings activeSettings { get; set; }


        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        private Mod instance;

        public static void Log(string text) => log.Info(text);

        public void OnLoad(UpdateSystem updateSystem)
        {


            instance = this;
            log.Info(nameof(OnLoad));

            ActiveWorld = updateSystem.World;

            _noisePollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
            _netPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<NetPollutionSystem>();
            _buildingPollutionAddSystem = updateSystem.World.GetOrCreateSystemManaged<BuildingPollutionAddSystem>();
            _groundPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
            _groundWaterPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<GroundWaterPollutionSystem>();
            _waterPipePollutionSystem = updateSystem.World.GetOrCreateSystemManaged<WaterPipePollutionSystem>();
            _airPollutionSystem = updateSystem.World.GetOrCreateSystemManaged<AirPollutionSystem>();
            NoisePollutionResetSystem.World = updateSystem.World;
            GroundPollutionResetSystem.World = updateSystem.World;
            AirPollutionResetSystem.World = updateSystem.World;
            _noisepollutionMofiier = updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
            

            ModSettings activeSettings = new(this);
            activeSettings.RegisterInOptionsUI();

            Localization.LoadTranslations(activeSettings, log);


            AssetDatabase.global.LoadSettings("NoPollution", activeSettings, new ModSettings(this));

            

            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");


           
        }
        
        public class NoisePollutionResetSystem
        {
            public static World World { get; set; }

            public static void ResetPollution()
            {

                NoisePollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<NoisePollutionSystem>();

                JobHandle dependencies;
                CellMapData<NoisePollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies);
                
                dependencies.Complete();
                log.Info("NoisePollution dependencies completed");

                for (int i = 0; i < data.m_TextureSize.x * data.m_TextureSize.y; i++)
                {
                    data.m_Buffer[i] = default(NoisePollution);
                }
                log.Info("NoisePollution buffer initialized");


            }
        }
        public class GroundPollutionResetSystem
        {
            public static World World { get; set; }

            public static void ResetPollution()
            {
                GroundPollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<GroundPollutionSystem>();
                log.Info("GroundPollutionSystem created");

                JobHandle dependencies2;
                CellMapData<GroundPollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies2);
                

                dependencies2.Complete();
                log.Info("GroundPollution dependencies completed");

                for (int j = 0; j < data.m_TextureSize.x * data.m_TextureSize.y; j++)
                {
                    data.m_Buffer[j] = default(GroundPollution);
                }
                log.Info("GroundPollutionSystem created");

            }

        }
        public class AirPollutionResetSystem
        {
            public static World World { get; set; }

            public static void ResetPollution()
            {
                AirPollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<AirPollutionSystem>();
                
                JobHandle dependencies3;
                CellMapData<AirPollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies3);
                
                dependencies3.Complete();

                for (int k = 0; k < data.m_TextureSize.x * data.m_TextureSize.y; k++)
                {
                    data.m_Buffer[k] = default(AirPollution);
                }
                



            }

            
        }
        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }

    }
}




       

    


