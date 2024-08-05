using Colossal.Entities;
using Game.Prefabs;
using Game;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace NoPollution.PollutionPrefabs
{
    public partial class PollutionPrefabQuery : GameSystemBase
    {
        private PrefabSystem prefabSystem;
        private EntityQuery prefabQuery;
        private Setting setting;

        protected override void OnCreate()
        {
            base.OnCreate();

            prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            prefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadWrite<PrefabData>(),
                    ComponentType.ReadWrite<Mod.PollutionPrefabWrapper>()
                }
            });
            RequireForUpdate(prefabQuery);

            setting = Mod.m_Setting; // Initialize the setting here
        }

        protected override void OnUpdate()
        {
            // Ensure settings are initialized
            if (setting == null)
            {
                // Handle the case where settings are not initialized
                Debug.LogWarning("Mod settings are not initialized.");
                return;
            }

            try
            {
                using (var entities = prefabQuery.ToEntityArray(Allocator.Temp))
                {
                    foreach (Entity entity in entities)
                    {
                        if (!prefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                        {
                            continue;
                        }

                        if (EntityManager.HasComponent<Mod.PollutionPrefabWrapper>(entity))
                        {
                            Mod.PollutionPrefabWrapper wrapper = EntityManager.GetComponentData<Mod.PollutionPrefabWrapper>(entity);
                            PollutionPrefab pollutionPrefab = EntityManager.GetComponentObject<PollutionPrefab>(wrapper.pollutionPrefabEntity);

                            // Adjust the values based on the settings
                            pollutionPrefab.m_NetNoiseMultiplier = (float)(setting.NetNoiseMultiplier / 100d * new VanillaData().m_NetNoiseMultiplier);
                            pollutionPrefab.m_NetNoiseMultiplier = (float)(setting.NoiseRadius / 100d * new VanillaData().m_NetNoiseMultiplier);
                            pollutionPrefab.m_NetNoiseMultiplier = (float)(setting.NetNoiseRadius / 100d * new VanillaData().m_NetNoiseMultiplier);
                            // You don't need to set the component data back as we are modifying the class instance directly
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
