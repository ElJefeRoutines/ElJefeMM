using Styx.TreeSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx;

#region [Method] - Class Redundancy
using Action = Styx.TreeSharp.Action;
using L = ElJefeMM.Core.Utilities.Log;
using SB = ElJefeMM.Core.Helpers.Spell_Book;
using S = ElJefeMM.Core.Spell;
using U = ElJefeMM.Core.Unit;
#endregion  

namespace ElJefeMM.Rotation
{
    class Conditions
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit currentTarget { get { return StyxWoW.Me.CurrentTarget; } }

        #region [Method] - Auto Attack
        private static Composite autoAttack()
        {
            return new Action(ret =>
            {
                if (!Me.IsAutoAttacking && U.isUnitValid(currentTarget, 5))
                    Lua.DoString("StartAttack()");
                return RunStatus.Failure;
            });
        }
        #endregion  

//        #region [Method] - Blood Tap
//        public static Composite bloodTap()
//        {
//            return S.blindCast(SB.spellBloodTap, ret => U.bloodCharge > 4 && (U.bloodRunes == 0 || U.frostRunes == 0 || U.unholyRunes == 0), true);
//        }
//        #endregion 

        #region [Method] - Rapid Fire
        public static Composite rapidFire()
        {
            return S.Buff(SB.spellRapidFire, ret => true);
        }
        #endregion  

        #region [Method] - Explosive Trap
        public static Composite explosiveTrap(WoWUnit Unit, double Range)
        {
            return S.dropCast(SB.spellExplosiveTrap, ret => Unit, ret => U.isUnitValid(Unit, Range));
        }
        #endregion  

        #region [Method] - Chimaera Shot
        public static Composite chimaeraShot(WoWUnit Unit, double Range)
        {
            return S.Cast(SB.spellChimaeraShot, ret => U.isUnitValid(Unit, Range));
        }
        #endregion  

    }
}
