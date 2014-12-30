using Styx;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using CommonBehaviors.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region [Method] - Class Redundancy
using C = ElJefeMM.Rotation.Conditions;
using HkM = ElJefeMM.Core.Managers.Hotkey_Manager;
using SB = ElJefeMM.Core.Helpers.Spell_Book;
using TM = ElJefeMM.Core.Managers.Talent_Manager;
using U = ElJefeMM.Core.Unit;
using S = ElJefeMM.Core.Spell;
using L = ElJefeMM.Core.Utilities.Log;
#endregion  

namespace ElJefeMM.Rotation
{
    class Rotation
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit currentTarget { get { return StyxWoW.Me.CurrentTarget; } }

        #region [Method] - Combat Buffing
        public static Composite combatBuffing()
        {
            return new PrioritySelector(
                new Decorator(ret => HkM.manualOn || !Me.IsAlive || !Me.GotTarget, new ActionAlwaysSucceed())
                );
        } 
        #endregion

        #region [Method] - Precombat Buffing
        public static Composite preCombatBuffing()
        {
            return new PrioritySelector(
                new Decorator(ret => HkM.manualOn || !Me.IsAlive || Me.IsCasting || Me.IsChanneling || Me.Mounted || Me.OnTaxi, new ActionAlwaysSucceed()),
                S.Buff(SB.trapLauncher, ret => !U.auraExists(Me, SB.trapLauncher))
//                actions.precombat=flask,type=greater_draenic_agility_flask
//                actions.precombat+=/food,type=calamari_crepes
//                actions.precombat+=/summon_pet
//                actions.precombat+=/snapshot_stats
//                actions.precombat+=/exotic_munitions,ammo_type=poisoned,if=active_enemies<3
//                actions.precombat+=/exotic_munitions,ammo_type=incendiary,if=active_enemies>=3
//                actions.precombat+=/potion,name=draenic_agility
//                actions.precombat+=/glaive_toss
//                actions.precombat+=/focusing_shot,if=!talent.glaive_toss.enabled
                );
        }  
        #endregion

        #region [Method] - Rotation Selector
        public static Composite rotationSelector()
        {
            return new PrioritySelector(
                new Decorator(ret => HkM.manualOn || !Me.IsAlive || !Me.GotTarget, new ActionAlwaysSucceed()),
                new Switch<bool>(ret => HkM.aoeOn,
                    new SwitchArgument<bool>(true,
                        new PrioritySelector(
                            )),
                    new SwitchArgument<bool>(false,
                        new PrioritySelector(
                            S.Cast(SB.spellChimaeraShot, ret => U.isUnitValid(currentTarget, 42) && Me.CurrentFocus >= 35),
                            S.Cast(SB.spellKillShot, ret => U.isUnitValid(currentTarget, 48)),
                            S.Cast(SB.spellRapidFire, ret => true),
                            S.dropCast(SB.spellExplosiveTrap, ret => currentTarget, ret => U.isUnitValid(currentTarget, 42)),
                            S.Cast(SB.spellCrows, ret => U.isUnitValid(currentTarget, 42) && Me.CurrentFocus >= 30),
                            S.Cast(SB.spellBarrage, ret => U.isUnitValid(currentTarget, 42) && Me.CurrentFocus >= 60),
                            S.Cast(SB.spellAimedShot, ret => U.isUnitValid(currentTarget, 42) && Me.CurrentFocus >= 50),
                            S.Cast(SB.spellSteadyShot, ret => U.isUnitValid(currentTarget, 42) && Me.CurrentFocus < 35)
//                            actions=auto_shot
//                            actions+=/use_item,name=beating_heart_of_the_mountain
//                            actions+=/arcane_torrent,if=focus.deficit>=30
//                            actions+=/blood_fury
//                            actions+=/berserking
//                            actions+=/potion,name=draenic_agility,if=((buff.rapid_fire.up|buff.bloodlust.up)&(cooldown.stampede.remains<1))|target.time_to_die<=25
//                            actions+=/chimaera_shot
//                            actions+=/kill_shot
//                            actions+=/rapid_fire
//                            actions+=/stampede,if=buff.rapid_fire.up|buff.bloodlust.up|target.time_to_die<=25
//                            actions+=/call_action_list,name=careful_aim,if=buff.careful_aim.up
//                            actions+=/explosive_trap,if=active_enemies>1
//                            actions+=/a_murder_of_crows
//                            actions+=/dire_beast,if=cast_regen+action.aimed_shot.cast_regen<focus.deficit
//                            actions+=/glaive_toss
//                            actions+=/powershot,if=cast_regen<focus.deficit
//                            actions+=/barrage
//                            actions+=/steady_shot,if=focus.deficit*cast_time%(14+cast_regen)>cooldown.rapid_fire.remains
//                            actions+=/focusing_shot,if=focus.deficit*cast_time%(50+cast_regen)>cooldown.rapid_fire.remains&focus<100
//                            actions+=/steady_shot,if=buff.pre_steady_focus.up&(14+cast_regen+action.aimed_shot.cast_regen)<=focus.deficit
//                            actions+=/multishot,if=active_enemies>6
//                            actions+=/aimed_shot,if=talent.focusing_shot.enabled
//                            actions+=/aimed_shot,if=focus+cast_regen>=85
//                            actions+=/aimed_shot,if=buff.thrill_of_the_hunt.react&focus+cast_regen>=65
//                            actions+=/focusing_shot,if=50+cast_regen-10<focus.deficit
//                            actions+=/steady_shot
                            ))));
        } 
        #endregion
    }
}
