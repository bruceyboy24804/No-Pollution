using Colossal.Entities;
using Game;
using Game.Prefabs;
using Game.UI.InGame;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace NoPollution
{
    public partial class PollutionModiferDataQuery : GameSystemBase
    {
        
     
        private EntityQuery Query;
    
        
        protected override void OnCreate()
        {
            base.OnCreate();
           
            Query = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    
                    ComponentType.ReadOnly<PollutionParameterData>()
                }
            });
            RequireForUpdate(Query);
            
        }
        protected override void OnUpdate()
        {
            Setting setting = Mod.m_Setting;




            using (var entities = Query.ToEntityArray(Allocator.Temp))
            {
                foreach (var entity in entities)
                {
                    if (EntityManager.TryGetComponent(entity, out PollutionParameterData data))
                    {
                        data.m_GroundMultiplier = (int)setting.GroundMultiplier;
                        data.m_AirMultiplier = (int)setting.AirMultiplier;
                        data.m_NoiseMultiplier = (int)(setting.NoiseMultiplier / 100d * VanillaParameterData.m_NoiseMultiplier);
                        data.m_NetAirMultiplier = (int)setting.NetAirMultiplier;
                        data.m_NetNoiseMultiplier = (int)setting.NetNoiseMultiplier;
                        data.m_PlantAirMultiplier = (int)setting.PlantAirMultiplier;
                        data.m_PlantGroundMultiplier = (int)(setting.PlantGroundMultiplier);
                        data.m_FertilityGroundMultiplier = (int)setting.FertilityGroundMultiplier;

                        data.m_GroundRadius = (int)(setting.GroundRadius / 100d * VanillaParameterData.m_GroundRadius);
                        data.m_AirRadius = setting.AirRadius;
                        data.m_NoiseRadius = (int)(setting.NoiseRadius / 100d * VanillaParameterData.m_NoiseRadius);
                        data.m_NetNoiseRadius = setting.NetNoiseRadius;

                        data.m_GroundFade = (short)(setting.GroundFade / 100d * VanillaParameterData.m_GroundFade);
                        data.m_AirFade = (short)(setting.AirFade / 100d * VanillaParameterData.m_AirFade);
                        data.m_PlantFade = setting.PlantFade;

                        data.m_AirPollutionNotificationLimit = (int)(setting.AirPollutionNotificationLimit);
                        data.m_NoisePollutionNotificationLimit = (int)(setting.NoisePollutionNotificationLimit);
                        data.m_GroundPollutionNotificationLimit = (int)(setting.GroundPollutionNotificationLimit);

                        data.m_WindAdvectionSpeed = setting.WindAdvectionSpeed;
                        data.m_DistanceExponent = setting.DistanceExponent;
                        data.m_HomelessNoisePollution = (int)setting.HomelessNoisePollution;
                        data.m_GroundPollutionLandValueDivisor = (int)(setting.GroundPollutionLandValueDivsor / 100d * VanillaParameterData.m_GroundPollutionLandValueDivisor);

                        // Set the updated data back to the entity
                        EntityManager.SetComponentData(entity, data);
                    }

                }
            }

        }
    }
}
