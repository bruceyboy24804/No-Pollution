using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using UnityEngine.PlayerLoop;
using Unity.Entities;

namespace NoPollution
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);


        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<NetPollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<BuildingPollutionAddSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<GroundWaterPollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<WaterPipePollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<AirPollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<SewageOutletAISystem>().Enabled = false;







        }




        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }
    }
}
