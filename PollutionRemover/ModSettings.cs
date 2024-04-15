using Colossal;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using JetBrains.Annotations;
using NoPollution;
using System.Collections.Generic;
using Unity.Entities;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]

    public class ModSettings : ModSetting
    {
        
        public ModSettings(IMod mod) : base(mod)
        {
        }

        [SettingsUIButton]
        public bool ResetPollution
        {
            set
            {
                // Log a message here using your mod's logger.
            }
        }






        public override void SetDefaults()
        {

        }
        public void Unload()
        {

        }
    }
}