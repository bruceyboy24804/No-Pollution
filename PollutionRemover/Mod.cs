﻿using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using UnityEngine.PlayerLoop;
using Unity.Entities;
using Colossal.IO.AssetDatabase;


namespace NoPollution
{
    public class Mod : IMod
    {
         internal ModSettings ActiveSettings { get; set; }
       
        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {

            ModSettings activeSettings = new(this);
            activeSettings.RegisterInOptionsUI();

            Localization.LoadTranslations(activeSettings, log);


            AssetDatabase.global.LoadSettings("NoPollution", ActiveSettings, new ModSettings(this));


            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");



            

            updateSystem.World.GetOrCreateSystemManaged<NoisePollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<NetPollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<BuildingPollutionAddSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<GroundWaterPollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<WaterPipePollutionSystem>().Enabled = false;
            updateSystem.World.GetOrCreateSystemManaged<AirPollutionSystem>().Enabled = false;
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }

    }
}