using Game;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using System.Collections.Generic;
using Colossal.Entities;
using Unity.Collections;
using Unity.Entities;
using NoPollution;

namespace NoPollution.Querys
{
    public partial class PollutionDataQuery : GameSystemBase
    {
        private EntityQuery m_Query;
        private PrefabSystem _PrefabSystem;
        // Dictionary to store base pollution values per entity
        private Dictionary<Entity, PollutionData> basePollutionValues = new Dictionary<Entity, PollutionData>();
        private static readonly PrefabID _PrefabID1 = new PrefabID("ZonePrefab", "Industrial Manufacturing");

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Query = SystemAPI.QueryBuilder().WithAll<PollutionData>().Build();
            _PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            RequireForUpdate(m_Query);
        }

        protected override void OnUpdate()
        {
            Setting setting = Mod.m_Setting;  
            NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.Temp);
            NativeArray<PollutionData> pollutionDataArray = m_Query.ToComponentDataArray<PollutionData>(Allocator.Temp);

            for (int i = 0; i < pollutionDataArray.Length; i++)
            {
                var entity = entities[i];
                var pollution = pollutionDataArray[i];

                
                if (!basePollutionValues.ContainsKey(entity))
                {
                    
                    basePollutionValues[entity] = pollution;
                }

                // Retrieve the stored base values
                var basePollution = basePollutionValues[entity];

                // Apply the percentage modifications based on the settings
                pollution.m_GroundPollution = (float)(basePollution.m_GroundPollution * (setting.GroundPollutionSlider / 100.0));
                pollution.m_AirPollution = (float)(basePollution.m_AirPollution * (setting.AirPollutionSlider / 100.0));
                pollution.m_NoisePollution = (float)(basePollution.m_NoisePollution * (setting.NoisePollutionSlider / 100.0));

                // Update the array with modified data
                pollutionDataArray[i] = pollution;
            }

            // Apply the modified pollution data back to the entities
            m_Query.CopyFromComponentDataArray(pollutionDataArray);

            // Dispose of NativeArrays when done
            entities.Dispose();
            pollutionDataArray.Dispose();
        }
        public void LegacyButton()
        {
            if (_PrefabSystem.TryGetPrefab(_PrefabID1, out PrefabBase prefab) 
                && _PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.ZonePollutionData data))
            {
                data.m_NoisePollution = 15;
                EntityManager.SetComponentData(prefabEntity, data);
            }
        }
        public void CurrentButton()
        {
            if (_PrefabSystem.TryGetPrefab(_PrefabID1, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.ZonePollution zonePollution)
                && _PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.ZonePollutionData data))
            {
                data.m_NoisePollution = zonePollution.m_NoisePollution;
                EntityManager.SetComponentData(prefabEntity, data);
            }
        }
    }
}
