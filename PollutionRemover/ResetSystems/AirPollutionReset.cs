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
    public partial class AirPollutionResetSystem
    {
        public static World World { get; internal set; }

        

        public static void ResetPollution()
        {
            AirPollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<AirPollutionSystem>();

            JobHandle dependencies3;
            CellMapData<AirPollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies3);

            dependencies3.Complete();

            for (int k = 0; k < data.m_TextureSize.x * data.m_TextureSize.y; k++)
            {
                data.m_Buffer[k] = default(AirPollution);
            }
        }

       
    }
}
