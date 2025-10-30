using Game;
using Game.Simulation;
using NoPollution.Domain;
using NoPollution;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;
namespace NoPollution.Systems
{
    public partial class GroundWaterPollutionReductionSystem : GameSystemBase
    {
        [BurstCompile]
        private struct ReduceGroundWaterPollutionJob : IJob
        {
            public NativeArray<GroundWater> m_GroundWaterMap;
            public float reductionRateMultiplier;

            public void Execute()
            {
                for (int i = 0; i < m_GroundWaterMap.Length; i++)
                {
                    GroundWater groundWater = m_GroundWaterMap[i];

                    if (groundWater.m_Polluted > 0)
                    {
                        // Reduce the polluted amount, ensuring it doesn't go below zero
                        groundWater.m_Polluted = (short)math.max(0, groundWater.m_Polluted - reductionRateMultiplier);
                        m_GroundWaterMap[i] = groundWater;
                    }
                }
            }
        }

        private GroundWaterSystem m_GroundWaterSystem;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 128;
        }

        public override int GetUpdateOffset(SystemUpdatePhase phase)
        {
            return 64;
        }

        [Preserve]
        protected override void OnCreate()
        {
            base.OnCreate();
            m_GroundWaterSystem = base.World.GetOrCreateSystemManaged<GroundWaterSystem>();
        }

        [Preserve]
        protected override void OnUpdate()
        {
            ReduceGroundWaterPollutionJob reducePollutionJob = default(ReduceGroundWaterPollutionJob);
            reducePollutionJob.m_GroundWaterMap = m_GroundWaterSystem.GetMap(readOnly: false, out var dependencies);

            // Determine the reduction rate based on the selected option
            var reductionRate = Mod.m_Setting.GroundWaterPollutionReductionRate;

            reducePollutionJob.reductionRateMultiplier = reductionRate switch
            {
                GroundWaterPollutionReductionRate.NoReduction => 0f,                  // No reduction
                GroundWaterPollutionReductionRate.NormalReduction => 1f,              // Normal reduction
                GroundWaterPollutionReductionRate.FastReduction => 10f,               // Fast reduction
                GroundWaterPollutionReductionRate.InstantReduction => float.MaxValue, // Instant reduction
                _ => 1f
            };

            base.Dependency = reducePollutionJob.Schedule(dependencies);
            m_GroundWaterSystem.AddWriter(base.Dependency);
        }

        [Preserve]
        public GroundWaterPollutionReductionSystem()
        {
        }
    }
}

