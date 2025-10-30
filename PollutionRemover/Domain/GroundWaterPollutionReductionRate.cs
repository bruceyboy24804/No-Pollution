using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoPollution.Domain
{
    public enum GroundWaterPollutionReductionRate
    {
        NoReduction,      // 0 reduction
        NormalReduction,  // Default normal reduction
        FastReduction,    // Fast reduction
        InstantReduction  // Instant reduction
    }

}
