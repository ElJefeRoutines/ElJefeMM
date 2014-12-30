using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region [Method] - Class Redundancy
using L = ElJefeMM.Core.Utilities.Log;
using SB = ElJefeMM.Core.Helpers.Spell_Book;
#endregion  

namespace ElJefeMM.Core
{
    static class Unit
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit currentTarget { get { return StyxWoW.Me.CurrentTarget; } }  

        #region [Method] - Active Enemies
        public static IEnumerable activeEnemies(WoWPoint fromLocation, double Range)
        {
            var Hostile = enemyCount;
            return Hostile != null ? Hostile.Where(x => x.Location.DistanceSqr(fromLocation) < Range * Range) : null;
        }

        private static List<WoWUnit> enemyCount { get; set; }

        public static void enemyAnnex(double Range)
        {
            enemyCount.Clear();
            foreach (var u in surroundingEnemies())
            {
                if (u == null || !u.IsValid)
                    continue;
                if (!u.IsAlive || u.DistanceSqr > Range * Range)
                    continue;
                if (!u.Attackable || !u.CanSelect)
                    continue;
                if (u.IsFriendly)
                    continue;
                if (u.IsNonCombatPet && u.IsCritter)
                    continue;
                enemyCount.Add(u);
            }
        }

        private static IEnumerable<WoWUnit> surroundingEnemies() { return ObjectManager.GetObjectsOfTypeFast<WoWUnit>(); }

        static Unit() { enemyCount = new List<WoWUnit>(); }  
        #endregion

        #region [Method] - Aura Detection
        public static bool auraExists(this WoWUnit Unit, int auraID, bool isMyAura = false)
        {
            try
            {
                if (Unit == null || !Unit.IsValid)
                    return false;
                WoWAura Aura = isMyAura ? Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID && A.CreatorGuid == Me.Guid) : Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID);
                return Aura != null;
            }
            catch (Exception xException)
            {
                L.diagnosticLog("Exception in auraExists(): ", xException);
                return false;
            }
        }  

        public static uint auraStacks(this WoWUnit Unit, int auraID, bool isMyAura = false)
        {
            try
            {
                if (Unit == null || !Unit.IsValid)
                    return 0;                
                WoWAura Aura = isMyAura ? Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID && A.CreatorGuid == Me.Guid) : Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID);
                return Aura != null ? Aura.StackCount : 0;
            }
            catch (Exception xException)
            {
                L.diagnosticLog("Exception in auraStacks(): ", xException);
                return 0;
            }
        }

        public static double auraTimeLeft(this WoWUnit Unit, int auraID, bool isMyAura = false)
        {
            try
            {
                if (Unit == null || !Unit.IsValid)
                    return 0;
                WoWAura Aura = isMyAura ? Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID && A.CreatorGuid == Me.Guid) : Unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraID);
                return Aura != null ? Aura.TimeLeft.TotalMilliseconds : 0;
            }
            catch (Exception xException)
            {
                L.diagnosticLog("Exception in auraTimeLeft(): ", xException);
                return 9999;
            }
        }  
        #endregion

        #region [Method] - Cache
        public static bool sniperTraining;
        //public static bool auraOnTarget;
        //public static uint auraStackOnMe;
        //public static uint auraStackOnTarget;
        public static uint currentFocus;

        public static void Cache()
        {
            sniperTraining = auraExists(Me, SB.sniperTraining);
            //auraOnTarget = auraExists(currentTarget, SB.auraName, true);
            //auraStackOnMe = auraStacks(Me, SB.auraName);
            //auraStackOnTarget = auraStacks(currentTarget, SB.auraName, true);
            currentFocus = Me.CurrentFocus;
        }  
        #endregion

        #region [Method] - Unit Evaluation
        public static bool isUnitValid(this WoWUnit Unit, double Range)
        {
            return Unit != null && Unit.IsValid && Unit.IsAlive && Unit.Attackable && Unit.DistanceSqr <= Range * Range;
        }  
        #endregion
    }
}
