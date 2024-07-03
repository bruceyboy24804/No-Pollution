using Colossal.Logging;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoPollution
{
    public static class PollutionDecayRate
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(NoPollution)).SetShowsErrorsInUI(false);
        private static FieldInfo _pollutionDecayRateField;
        private static WaterSystem _waterSystemInstance;
        private static float value;
        

        static PollutionDecayRate()
        {
            // Get the WaterSystem type and field info
            Type waterSystemType = typeof(WaterSystem);
            _pollutionDecayRateField = waterSystemType.GetField("m_PollutionDecayRate", BindingFlags.NonPublic | BindingFlags.Instance);

            // Assume Mod._waterSystem is set and accessible
            _waterSystemInstance = Mod._waterSystem;

            log.Info("PollutionDecayRate initialized successfully.");
        
            
        }
        public static float GetPollutionDecayRate()
        {
            float value = 50f; // Default value if the field or instance is not available
            try
            {
                if (_pollutionDecayRateField != null && _waterSystemInstance != null)
                {
                    log.Info("Getting PollutionDecayRate...");
                    value = (float)_pollutionDecayRateField.GetValue(_waterSystemInstance);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error getting PollutionDecayRate: {ex}");
            }
            log.Info($"Retrieved PollutionDecayRate: {value}");
            return value;
        }
        public static void SetPollutionDecayRate(float value)
        {
            try
            {
                if (_pollutionDecayRateField != null && _waterSystemInstance != null)
                {
                    log.Info($"Setting PollutionDecayRate to: {value}");
                    _pollutionDecayRateField.SetValue(_waterSystemInstance, value);
                    log.Info($"Set PollutionDecayRate to: {value}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error setting PollutionDecayRate: {ex}");
            }
        }

    }

}

