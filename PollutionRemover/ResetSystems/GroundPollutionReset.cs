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
    public class GroundPollutionResetSystem
    {
        public static World World { get; internal set; }



        public static void ResetPollution()
        {
            GroundPollutionSystem orCreateSystemManaged = World.GetOrCreateSystemManaged<GroundPollutionSystem>();

            JobHandle dependencies3;
            CellMapData<GroundPollution> data = orCreateSystemManaged.GetData(readOnly: false, out dependencies3);

            dependencies3.Complete();

            for (int j = 0; j < data.m_TextureSize.x * data.m_TextureSize.y; j++)
            {
                data.m_Buffer[j] = default(GroundPollution);
            }
        }

       
    }
}