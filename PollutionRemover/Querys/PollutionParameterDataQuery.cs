using Game;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace NoPollution.Querys
{
    public partial class PollutionParameterDataQuery : GameSystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }
        protected override void OnUpdate()
        {
            Setting setting = Mod.m_Setting;
            EntityQuery m_Query = SystemAPI.QueryBuilder()
                .WithAll<PollutionParameterData>()
                .Build();
            RequireForUpdate(m_Query);
            NativeArray<PollutionParameterData> pollutionParameterDataArry = m_Query.ToComponentDataArray<PollutionParameterData>(Allocator.Temp);
            for (int i = 0; i < pollutionParameterDataArry.Length; i++)
            {
                var pollution = pollutionParameterDataArry[i];
                pollution.m_GroundMultiplier = (int)setting.GroundMultiplier;
                pollution.m_AirMultiplier = (int)setting.AirMultiplier;
                pollution.m_NoiseMultiplier = (int)(setting.NoiseMultiplier / 100d * VanillaParameterData.m_NoiseMultiplier);
                pollution.m_NetAirMultiplier = (int)setting.NetAirMultiplier;
                pollution.m_NetNoiseMultiplier = (int)setting.NetNoiseMultiplier;
                pollution.m_PlantAirMultiplier = (int)setting.PlantAirMultiplier;
                pollution.m_PlantGroundMultiplier = (int)(setting.PlantGroundMultiplier);
                pollution.m_FertilityGroundMultiplier = (int)setting.FertilityGroundMultiplier;

                pollution.m_GroundRadius = (int)(setting.GroundRadius / 100d * VanillaParameterData.m_GroundRadius);
                pollution.m_AirRadius = setting.AirRadius;
                pollution.m_NoiseRadius = (int)(setting.NoiseRadius / 100d * VanillaParameterData.m_NoiseRadius);
                pollution.m_NetNoiseRadius = setting.NetNoiseRadius;

                pollution.m_GroundFade = (short)(setting.GroundFade / 100d * VanillaParameterData.m_GroundFade);
                pollution.m_AirFade = (short)(setting.AirFade / 100d * VanillaParameterData.m_AirFade);
                pollution.m_PlantFade = setting.PlantFade;

                pollution.m_AirPollutionNotificationLimit = (int)(setting.AirPollutionNotificationLimit);
                pollution.m_NoisePollutionNotificationLimit = (int)(setting.NoisePollutionNotificationLimit);
                pollution.m_GroundPollutionNotificationLimit = (int)(setting.GroundPollutionNotificationLimit);

                pollution.m_WindAdvectionSpeed = setting.WindAdvectionSpeed;
                pollution.m_DistanceExponent = setting.DistanceExponent;
                pollution.m_HomelessNoisePollution = (int)setting.HomelessNoisePollution;
                pollution.m_GroundPollutionLandValueDivisor = (int)(setting.GroundPollutionLandValueDivisor / 100d * VanillaParameterData.m_GroundPollutionLandValueDivisor);

                pollutionParameterDataArry[i] = pollution;
            }
            m_Query.CopyFromComponentDataArray(pollutionParameterDataArry);
        }
    }
}
