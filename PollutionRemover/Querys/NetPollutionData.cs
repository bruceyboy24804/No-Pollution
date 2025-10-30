using Game;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Pollution = Game.Net.Pollution;

namespace NoPollution.Querys
{
    public partial class NetPollutionDataQuery : GameSystemBase
    {
        private EntityQuery m_Query;
        private Dictionary<Entity, Pollution> previousAdjustments = new Dictionary<Entity, Pollution>();

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Query = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadWrite<Pollution>()
                }
            });
            RequireForUpdate(m_Query);
        }

        protected override void OnUpdate()
        {
            Setting setting = Mod.m_Setting;
            NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.Temp);
            NativeArray<Pollution> netPollutionDataArray = m_Query.ToComponentDataArray<Pollution>(Allocator.Temp);

            for (int i = 0; i < netPollutionDataArray.Length; i++)
            {
                var entity = entities[i];
                var netPollution = netPollutionDataArray[i];

                // Retrieve or initialize the previous adjustments
                if (!previousAdjustments.TryGetValue(entity, out var previousAdjustment))
                {
                    previousAdjustment = new Pollution
                    {
                        m_Pollution = new float2(1f, 1f),
                        m_Accumulation = new float2(1f, 1f)
                    };
                    previousAdjustments[entity] = previousAdjustment;
                }

                // Reverse the previous adjustment, ensuring no division by zero or invalid values
                if (IsValid(previousAdjustment.m_Pollution.x))
                    netPollution.m_Pollution.x /= previousAdjustment.m_Pollution.x;
                if (IsValid(previousAdjustment.m_Pollution.y))
                    netPollution.m_Pollution.y /= previousAdjustment.m_Pollution.y;
                if (IsValid(previousAdjustment.m_Accumulation.x))
                    netPollution.m_Accumulation.x /= previousAdjustment.m_Accumulation.x;
                if (IsValid(previousAdjustment.m_Accumulation.y))
                    netPollution.m_Accumulation.y /= previousAdjustment.m_Accumulation.y;

                // Apply the new adjustment based on the current slider values
                previousAdjustment.m_Pollution.x = setting.NetPollutionSlider1 / 100.0f;
                previousAdjustment.m_Pollution.y = setting.NetPollutionSlider2 / 100.0f;
                previousAdjustment.m_Accumulation.x = setting.NetPollutionAccumulationSlider1 / 100.0f;
                previousAdjustment.m_Accumulation.y = setting.NetPollutionAccumulationSlider2 / 100.0f;

                netPollution.m_Pollution.x *= previousAdjustment.m_Pollution.x;
                netPollution.m_Pollution.y *= previousAdjustment.m_Pollution.y;
                netPollution.m_Accumulation.x *= previousAdjustment.m_Accumulation.x;
                netPollution.m_Accumulation.y *= previousAdjustment.m_Accumulation.y;

                // Update the dictionary with the new adjustments
                previousAdjustments[entity] = previousAdjustment;

                // Update the array with modified data
                netPollutionDataArray[i] = netPollution;
            }

            m_Query.CopyFromComponentDataArray(netPollutionDataArray);

            // Dispose of NativeArrays when done
            entities.Dispose();
            netPollutionDataArray.Dispose();
        }

        // Reset method to revert non-edited values
        public void ResetPollutionToOriginal()
        {
            NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.Temp);
            NativeArray<Pollution> netPollutionDataArray = m_Query.ToComponentDataArray<Pollution>(Allocator.Temp);

            for (int i = 0; i < netPollutionDataArray.Length; i++)
            {
                var entity = entities[i];
                var netPollution = netPollutionDataArray[i];

                // Reverse the previous adjustment to restore original values
                if (previousAdjustments.TryGetValue(entity, out var previousAdjustment))
                {
                    if (IsValid(previousAdjustment.m_Pollution.x))
                        netPollution.m_Pollution.x /= previousAdjustment.m_Pollution.x;
                    if (IsValid(previousAdjustment.m_Pollution.y))
                        netPollution.m_Pollution.y /= previousAdjustment.m_Pollution.y;
                    if (IsValid(previousAdjustment.m_Accumulation.x))
                        netPollution.m_Accumulation.x /= previousAdjustment.m_Accumulation.x;
                    if (IsValid(previousAdjustment.m_Accumulation.y))
                        netPollution.m_Accumulation.y /= previousAdjustment.m_Accumulation.y;

                    // Remove the adjustment entry after resetting
                    previousAdjustments.Remove(entity);
                }

                // Update the array with the reset data
                netPollutionDataArray[i] = netPollution;
            }

            m_Query.CopyFromComponentDataArray(netPollutionDataArray);

            // Dispose of NativeArrays when done
            entities.Dispose();
            netPollutionDataArray.Dispose();
        }

        private bool IsValid(float value)
        {
            // Ensure the value is valid for division (not zero, NaN, or infinity)
            return value != 0f && !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
