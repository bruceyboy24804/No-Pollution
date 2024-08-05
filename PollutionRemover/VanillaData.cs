using NoPollution;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game.Prefabs
{
    public class VanillaData : PrefabBase
    {
        // Class properties as before...
        public float m_GroundMultiplier = 25f;

        public float m_AirMultiplier = 25f;

        public float m_NoiseMultiplier = 100f;

        public float m_NetAirMultiplier = 25f;

        public float m_NetNoiseMultiplier = 100f;

        public float m_GroundRadius = 150f;

        public float m_AirRadius = 75f;

        public float m_NoiseRadius = 200f;

        public float m_NetNoiseRadius = 50f;

        public float m_WindAdvectionSpeed = 8f;

        public short m_AirFade = 5;

        public short m_GroundFade = 10;

        public float m_PlantAirMultiplier = 0.001f;

        public float m_PlantGroundMultiplier = 0.001f;

        public float m_PlantFade = 2f;

        public float m_FertilityGroundMultiplier = 1f;

        public float m_DistanceExponent = 2f;


        public override void GetPrefabComponents(HashSet<ComponentType> components)
        {
            base.GetPrefabComponents(components);
            components.Add(ComponentType.ReadWrite<PollutionParameterData>());
            components.Add(ComponentType.ReadWrite<Mod.PollutionPrefabWrapper>()); // Add this line
        }

        public override void LateInitialize(EntityManager entityManager, Entity entity)
        {
            base.LateInitialize(entityManager, entity);

            PrefabSystem orCreateSystemManaged = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

            // Set the PollutionPrefabWrapper data
            Mod.PollutionPrefabWrapper wrapper = new Mod.PollutionPrefabWrapper
            {
                pollutionPrefabEntity = entity
            };
            entityManager.SetComponentData(entity, wrapper);

            // Existing component data setup...
        }
    }
}
