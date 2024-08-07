using NoPollution;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game.Prefabs
{
    public class VanillaParameterData
    {
    

        // Class properties as before...
        public static float m_GroundMultiplier = 20f;

        public static float m_AirMultiplier = 25f;

        public static float m_NoiseMultiplier = 250f;

        public static float m_NetAirMultiplier = 1.75f;

        public static float m_NetNoiseMultiplier = 5f;

        public static float m_GroundRadius = 500f;

        public static float m_AirRadius = 100f;

        public static float m_NoiseRadius = 400f;

        public static float m_NetNoiseRadius = 8f;

        public static float m_WindAdvectionSpeed = 20f;

        public static short m_AirFade = 8000;

        public static short m_GroundFade = 4000;

        public static float m_PlantAirMultiplier = 0.001f;

        public static float m_PlantGroundMultiplier = 0.001f;

        public static float m_PlantFade = 2f;

        public static float m_FertilityGroundMultiplier = 1f;

        public static float m_DistanceExponent = 1.5f;

        public static float m_AirPollutionNotificationLimit = -7f;

        public static float m_NoisePollutionNotificationLimit = -7f;

        public static float m_GroundPollutionNotificationLimit = -7f;

        public static float m_AbandonedNoisePollutionMultiplier = 5f;

        public static float m_HomelessNoisePollution = 50f;

        public static float m_GroundPollutionLandValueDivisor = 500f;

       
    }
}
