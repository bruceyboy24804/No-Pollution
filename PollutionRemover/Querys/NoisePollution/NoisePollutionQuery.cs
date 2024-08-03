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
    public partial class NoisePollutionQuery : GameSystemBase
    {
        private EntityQuery _noisePollutionQuery;
        private Setting setting = Mod.m_Setting;
        private NativeHashMap<Entity, float> _originalNoisePollutionMap;

        protected override void OnCreate()
        {
            base.OnCreate();
            _noisePollutionQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    typeof(PollutionData)
                }
            });

            RequireForUpdate(_noisePollutionQuery);
            _originalNoisePollutionMap = new NativeHashMap<Entity, float>(0, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_originalNoisePollutionMap.IsCreated)
            {
                _originalNoisePollutionMap.Dispose();
            }
        }

        protected override void OnUpdate()
        {
            using (var entities = _noisePollutionQuery.ToEntityArray(Allocator.Temp))
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

                        UpdateNoisePollution(data, entity);
                    }
                }
            }
        }

        private void UpdateNoisePollution(PollutionData data, Entity entity)
        {
            if (!_originalNoisePollutionMap.ContainsKey(entity))
            {
                // Store the original pollution data
                _originalNoisePollutionMap.Add(entity, data.m_NoisePollution);
            }

            float originalPollution = _originalNoisePollutionMap[entity];
            float sliderValue = setting.NoisePollutionDataSlider / 100f;

            // Add debug logs to verify values


            float updatedNoisePollution = originalPollution * sliderValue;

            // Ensure that updatedGroundPollution is valid
            if (updatedNoisePollution < 0)
            {
                updatedNoisePollution = 0;
            }



            data.m_NoisePollution = updatedNoisePollution;
            EntityManager.SetComponentData(entity, data);

            // Verify if the data was correctly updated
            PollutionData updatedData = EntityManager.GetComponentData<PollutionData>(entity);

        }
    }
}
