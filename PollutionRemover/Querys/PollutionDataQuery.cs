using Game;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using NoPollution;

namespace NoPollution.Querys
{
    public partial class PollutionDataQuery : GameSystemBase
    {
        private EntityQuery m_Query;

        // Dictionary to store base pollution values per entity
        private Dictionary<Entity, PollutionData> basePollutionValues = new Dictionary<Entity, PollutionData>();

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Query = SystemAPI.QueryBuilder().WithAll<PollutionData>().Build();
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
    }
}
