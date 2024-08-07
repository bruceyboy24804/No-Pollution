using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;

namespace NoPollution.ResetSystems
{
    public class NoisePollutionResetSystem 
    {
      public static World World { get; set; }

        public static void ResetPollution()
        {
            NoisePollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<NoisePollutionSystem>();

            JobHandle dependencies;
            CellMapData<NoisePollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies);

            dependencies.Complete();

            for (int i = 0; i < data.m_TextureSize.x * data.m_TextureSize.y; i++)
            {
                data.m_Buffer[i] = default(NoisePollution);
            }
        }

        
    }
}
