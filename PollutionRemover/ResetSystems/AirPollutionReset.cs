using Game.Simulation;
//using NoPollution.Systems;
using Unity.Entities;
using Unity.Jobs;

namespace NoPollution.ResetSystems
{
    public class AirPollutionResetSystem
    {
        public static World World { get; set; }

        public static void ResetPollution()
        {
            AirPollutionSystem orCreateSystemManaged2 = World.GetOrCreateSystemManaged<AirPollutionSystem>();
          
            JobHandle dependencies2;
            CellMapData<AirPollution> data2 = orCreateSystemManaged2.GetData(readOnly: false, out dependencies2);
         
            dependencies2.Complete();
            for (int j = 0; j < data2.m_TextureSize.x * data2.m_TextureSize.y; j++)
            {
                data2.m_Buffer[j] = default(AirPollution);
                
            }
          
        }
    }
}