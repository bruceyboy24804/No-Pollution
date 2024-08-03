using Colossal.Entities;
using Game;
using Game.Prefabs;
using Game.Settings;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace NoPollution.Querys.AirPollution
{
    public partial class AirPollutionQuery : GameSystemBase
    {
        private EntityQuery _airPollutionQuery;
        private Setting setting = Mod.m_Setting;
        private NativeHashMap<Entity, float> _originalairPollutionMap;

        protected override void OnCreate()
        {
            base.OnCreate();
            _airPollutionQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    typeof(PollutionData)
                }
            });

            RequireForUpdate(_airPollutionQuery);
            _originalairPollutionMap = new NativeHashMap<Entity, float>(0, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_originalairPollutionMap.IsCreated)
            {
                _originalairPollutionMap.Dispose();
            }
        }

        protected override void OnUpdate()
        {
            using (var entities = _airPollutionQuery.ToEntityArray(Allocator.Temp))
            {
                foreach (var entity in entities)
                {
                    if (EntityManager.HasComponent<PollutionData>(entity))
                    {
                        PollutionData data = EntityManager.GetComponentData<PollutionData>(entity);

                        if (setting == null)
                        {
                            Mod.log.Info("modSettings is null");
                            return;
                        }

                        if (entity == null)
                        {
                            Mod.log.Info("entity is null");
                            return;
                        }

                        UpdateAirPollution(data, entity);
                    }
                }
            }
        }

        private void UpdateAirPollution(PollutionData data, Entity entity)
        {
            if (!_originalairPollutionMap.ContainsKey(entity))
            {
                // Store the original pollution data
                _originalairPollutionMap.Add(entity, data.m_AirPollution);
            }

            float originalPollution = _originalairPollutionMap[entity];
            float sliderValue = setting.AirPollutionDataSlider / 100f;

            // Add debug logs to verify values
            

            float updatedAirPollution = originalPollution * sliderValue;

            // Ensure that updatedGroundPollution is valid
            if (updatedAirPollution < 0)
            {
                updatedAirPollution = 0;
            }


            data.m_AirPollution = updatedAirPollution;
            EntityManager.SetComponentData(entity, data);

            // Verify if the data was correctly updated
            PollutionData updatedData = EntityManager.GetComponentData<PollutionData>(entity);
                
        }
    }
}
