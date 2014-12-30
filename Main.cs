// Hunter MM 6.0.3
// By papagal

using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

#region [Method] - Class Redundancy
using HKM = ElJefeMM.Core.Managers.Hotkey_Manager;
using R = ElJefeMM.Rotation.Rotation;
using U = ElJefeMM.Core.Unit;
#endregion 

namespace ElJefeMM
{
    public class Main : CombatRoutine
    {
        private static readonly Version version = new Version(0, 0, 1);
        public override string Name { get { return "ElJefeMM v" + version; } }
        public override WoWClass Class { get { return WoWClass.Hunter; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region [Method] - Implementations
        private Composite _preCombatBehavior, _combatBuffBehavior, _combatBehavior;
        public override Composite PreCombatBuffBehavior { get { return _preCombatBehavior ?? (_preCombatBehavior = R.preCombatBuffing()); } }
        public override Composite CombatBuffBehavior { get { return _combatBuffBehavior ?? (_combatBuffBehavior = R.combatBuffing()); } }
        public override Composite CombatBehavior { get { return _combatBehavior ?? (_combatBehavior = R.rotationSelector()); } }

        #region [Method] - Hidden Overrides
        public override void Initialize()
        {
            Logging.Write(Colors.OrangeRed, "G'day Mate!");
            Logging.Write(Colors.White, "");
            Logging.Write(Colors.OrangeRed, "This CR is only tested with Enyo");
            Logging.Write(Colors.OrangeRed, "  and WoW patch 6.0.3");
            Logging.Write(Colors.White, "");
            Logging.Write(Colors.OrangeRed, "Current Version:");
            Logging.Write(Colors.OrangeRed, "-- ElJefeMM v" + version + " --");
            Logging.Write(Colors.OrangeRed, "-- by papagal --");
            Logging.Write(Colors.OrangeRed, "-- A Marksman Hunter CR --");
            HKM.registerHotKeys();
        }
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress() { Logging.Write(Colors.OrangeRed, "Coming soon!"); }
        public override void ShutDown() { HKM.removeHotkeys(); }
        #endregion  

        #region [Method] - Pulse
        public override void Pulse()
        {
            if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || !Me.IsAlive)
                return;
            if (!Me.Combat)
                return;
            U.Cache();
            U.enemyAnnex(30);
        }
        #endregion  

        #endregion
    }
}