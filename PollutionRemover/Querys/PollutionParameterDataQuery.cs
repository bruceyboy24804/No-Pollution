using Game;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoPollution.Domain;
using Unity.Collections;
using Unity.Entities;

namespace NoPollution.Querys
{
    public partial class PollutionParameterDataQuery : GameSystemBase
    {
        private EntityQuery m_Query;
        private PrefabSystem _PrefabSystem;
        protected override void OnCreate()
        {
            base.OnCreate();
            _PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
             m_Query = SystemAPI.QueryBuilder()
                .WithAll<PollutionParameterData>()
                .Build();
            RequireForUpdate(m_Query);
        }
        protected override void OnUpdate()
        {
            VanillaData vanillaData = VanillaDataStorage.VanillaData;
            Setting setting = Mod.m_Setting;
            
            NativeArray<PollutionParameterData> pollutionParameterDataArry = m_Query.ToComponentDataArray<PollutionParameterData>(Allocator.Temp);
            for (int i = 0; i < pollutionParameterDataArry.Length; i++)
            {
                var pollution = pollutionParameterDataArry[i];
                pollution.m_GroundMultiplier = setting.GroundMultiplier;
                pollution.m_AirMultiplier = setting.AirMultiplier;
                pollution.m_NoiseMultiplier = setting.NoiseMultiplier;
                pollution.m_NetAirMultiplier = setting.NetAirMultiplier;
                pollution.m_NetNoiseMultiplier = setting.NetNoiseMultiplier;
                pollution.m_PlantAirMultiplier = setting.PlantAirMultiplier;
                pollution.m_PlantGroundMultiplier = setting.PlantGroundMultiplier;
                pollution.m_FertilityGroundMultiplier = setting.FertilityGroundMultiplier;

                pollution.m_GroundRadius = setting.GroundRadius;
                pollution.m_AirRadius = setting.AirRadius;
                pollution.m_NoiseRadius = setting.NoiseRadius;
                pollution.m_NetNoiseRadius = setting.NetNoiseRadius;

                pollution.m_GroundFade = (short)setting.GroundFade;
                pollution.m_AirFade = (short)setting.AirFade;
                pollution.m_PlantFade = setting.PlantFade;

                pollution.m_AirPollutionNotificationLimit = (int)(setting.AirPollutionNotificationLimit);
                pollution.m_NoisePollutionNotificationLimit = (int)(setting.NoisePollutionNotificationLimit);
                pollution.m_GroundPollutionNotificationLimit = (int)(setting.GroundPollutionNotificationLimit);

                pollution.m_WindAdvectionSpeed = setting.WindAdvectionSpeed;
                pollution.m_DistanceExponent = setting.DistanceExponent;
                pollution.m_HomelessNoisePollution = (int)setting.HomelessNoisePollution;
                pollution.m_GroundPollutionLandValueDivisor = (int)setting.GroundPollutionLandValueDivisor;

                pollutionParameterDataArry[i] = pollution;
            }
            m_Query.CopyFromComponentDataArray(pollutionParameterDataArry);
            pollutionParameterDataArry.Dispose();
        }
    }
}