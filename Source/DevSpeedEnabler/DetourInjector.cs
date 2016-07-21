using System;
using System.Linq;
using System.Reflection;
using CommunityCoreLibrary;
using Verse;
using RimWorld;

namespace DevSpeedEnabler
{

    public class DetourInjector : SpecialInjector
    {

        public override bool Inject()
        {
        	
            // Detour RimWorld.TimeControls
            MethodInfo RimWorld_TimeControls_DoTimeControlsGUI = typeof( TimeControls ).GetMethod("DoTimeControlsGUI", BindingFlags.Static | BindingFlags.Public );
            MethodInfo DevSpeedEnabler_TimeControls_DoTimeControlsGUI = typeof( _TimeControls ).GetMethod("_DoTimeControlsGUI", BindingFlags.Static | BindingFlags.Public );
            if( !Detours.TryDetourFromTo(RimWorld_TimeControls_DoTimeControlsGUI, DevSpeedEnabler_TimeControls_DoTimeControlsGUI) )
			{
                return false;
			}

            return true;
		}
	}
}