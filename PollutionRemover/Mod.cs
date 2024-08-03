using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using Unity.Entities;
using Colossal.IO.AssetDatabase;
using Unity.Jobs;
using Game.Debug;
using NoPollution.Querys.GroundPollution;
using static NoPollution.Setting;
using NoPollution.Querys.AirPollution;



namespace NoPollution
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        private Mod instance;
        public static Setting m_Setting;

        public static Mod Instance { get; private set; }
        public static DebugSystem _debugSystem;
        public static PrefabSystem _prefabSystem;
        public static NoisePollutionSystem _noisePollutionSystem;
        public static NetPollutionSystem _netPollutionSystem;
        public static BuildingPollutionAddSystem _buildingPollutionAddSystem;
        public static GroundPollutionSystem _groundPollutionSystem;
        public static GroundWaterPollutionSystem _groundWaterPollutionSystem;
        public static AirPollutionSystem _airPollutionSystem;
        public static WaterSystem _waterSystem;
       
        internal static World ActiveWorld { get; private set; }
        public World World { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            instance = this;
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            log.Info($"Current mod asset at {asset.path}");
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(NoPollution), m_Setting, new Setting(this));

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

            updateSystem.UpdateBefore<GroundPollutionQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<AirPollutionQuery>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateBefore<NoisePollutionQuery>(SystemUpdatePhase.GameSimulation);


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

                for (int i = 0; i < data.m_TextureSize.x * data.m_TextureSize.y; i++)
                {
                    data.m_Buffer[i] = default(NoisePollution);
                }
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

                for (int j = 0; j < data.m_TextureSize.x * data.m_TextureSize.y; j++)
                {
                    data.m_Buffer[j] = default(GroundPollution);
                }
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
        public class NoisePollutionModifier
        {
            public static World ActiveWorld { get; set; }
            
            public static void NoiseMultiplierr()
            {
                NoisePollutionSystem orCreateSystemManaged = ActiveWorld.GetOrCreateSystemManaged<NoisePollutionSystem>();


                JobHandle dependencies;
                CellMapData<NoisePollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies);
                dependencies.Complete();

                for (int i = 0; i < data.m_TextureSize.x * data.m_TextureSize.y; i++)
                {
                    data.m_Buffer[i] = default(NoisePollution);
                }

            }
        }
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
