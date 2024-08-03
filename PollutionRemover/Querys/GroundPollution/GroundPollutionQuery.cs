using Colossal.Entities;
using Game;
using Game.Prefabs;
using Game.Settings;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace NoPollution.Querys.GroundPollution
{
    public partial class GroundPollutionQuery : GameSystemBase
    {
        private EntityQuery _groundPollutionQuery;
        private Setting setting = Mod.m_Setting;
        private NativeHashMap<Entity, float> _originalGroundPollutionMap;

        protected override void OnCreate()
        {
            base.OnCreate();
            _groundPollutionQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    typeof(PollutionData)
                }
            });

            RequireForUpdate(_groundPollutionQuery);
            _originalGroundPollutionMap = new NativeHashMap<Entity, float>(0, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_originalGroundPollutionMap.IsCreated)
            {
                _originalGroundPollutionMap.Dispose();
            }
        }

        protected override void OnUpdate()
        {
            using (var entities = _groundPollutionQuery.ToEntityArray(Allocator.Temp))
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

                        UpdateGroundPollution(data, entity);
                    }
                }
            }
        }

        private void UpdateGroundPollution(PollutionData data, Entity entity)
        {
            if (!_originalGroundPollutionMap.ContainsKey(entity))
            {
                // Store the original pollution data
                _originalGroundPollutionMap.Add(entity, data.m_GroundPollution);
            }

            float originalPollution = _originalGroundPollutionMap[entity];
            float sliderValue = setting.GroundPollutionDataSlider / 100f;

            // Add debug logs to verify values
           

            float updatedGroundPollution = originalPollution * sliderValue;

            // Ensure that updatedGroundPollution is valid
            if (updatedGroundPollution < 0)
            {
                updatedGroundPollution = 0;
            }

          

            data.m_GroundPollution = updatedGroundPollution;
            EntityManager.SetComponentData(entity, data);

            // Verify if the data was correctly updated
            PollutionData updatedData = EntityManager.GetComponentData<PollutionData>(entity);
          
        }
    }
}
