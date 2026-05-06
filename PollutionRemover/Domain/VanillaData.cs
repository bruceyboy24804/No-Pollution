namespace NoPollution.Domain
{
    public static class VanillaDataStorage
    {
        public static VanillaData VanillaData { get; set; } = new ();
    }
    public struct VanillaData
    {
        public float m_GroundMultiplier;
		public float m_AirMultiplier;
		public float m_NoiseMultiplier;
		public float m_NetAirMultiplier;
		public float m_NetNoiseMultiplier;
		public float m_GroundRadius;
		public float m_AirRadius;
		public float m_NoiseRadius;
		public float m_NetNoiseRadius;
		public float m_WindAdvectionSpeed;
		public short m_AirFade;
		public short m_GroundFade;
		public float m_PlantAirMultiplier;
		public float m_PlantGroundMultiplier;
		public float m_PlantFade;
		public float m_FertilityGroundMultiplier;
		public float m_DistanceExponent;
		public int m_AirPollutionNotificationLimit;
		public int m_NoisePollutionNotificationLimit;
		public int m_GroundPollutionNotificationLimit;
		public float m_AbandonedNoisePollutionMultiplier;
		public int m_HomelessNoisePollution;
		public int m_GroundPollutionLandValueDivisor;
    }
    public static class VanillaDataLoader
    {
        public static void Load()
        {
            VanillaDataStorage.VanillaData = new VanillaData
            {
	            m_GroundMultiplier = 20,
			    m_AirMultiplier = 40,
			    m_NoiseMultiplier = 250,
			    m_NetAirMultiplier = 1,
			    m_NetNoiseMultiplier = 2,
			    m_GroundRadius = 500,
			    m_AirRadius = 100,
			    m_NoiseRadius = 600,
			    m_NetNoiseRadius = 3,
			    m_WindAdvectionSpeed = 30,
			    m_AirFade = 5000,
			    m_GroundFade = 4000,
			    m_PlantAirMultiplier = 0.001f,
			    m_PlantGroundMultiplier = 0.001f,
			    m_PlantFade = 2,
			    m_FertilityGroundMultiplier = 1,
			    m_DistanceExponent = 1.5f,
			    m_AirPollutionNotificationLimit = -7,
			    m_NoisePollutionNotificationLimit = -7,
			    m_GroundPollutionNotificationLimit = -7,
			    m_AbandonedNoisePollutionMultiplier = 5,
			    m_HomelessNoisePollution = 50,
			    m_GroundPollutionLandValueDivisor = 500
	            
            };
        }
    }
        
}