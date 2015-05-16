using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTAV_purge_mod.Team;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod.Ability {

    public class AbilityMedic : IAbility {

        private Menu _abilityMenu;
        private int _abilityPoints = 10;

        public bool Perform(params object[] args) {

            if (args.Length == 2) {

                var target = (TeamMember) args[0];
                var increment = (int) args[1];

                if (target != null && increment != 0) {
                    if (AbilityPoints - increment >= 0) {

                        if (target.Health < target.MaxHealth) {

                            if (target.Health + increment <= target.MaxHealth) {

                                target.Health += increment;
                                DecrementAbilityPoints(increment);
                                return true;

                            } else {

                                // The target can't have its health incremented by the specified amount.
                                return false;

                            }

                        } else {

                            // The target doesn't need healing.  
                            return false;

                        }

                    } else {

                        // There's not enough ability points to spend.
                        return false;

                    }
                } else {

                    // There's no valid target.  
                    return false;

                }

            }

            return false;

        }

        public void ShowMenu(TeamMember member) {
            
            MenuItem[] buttons = new MenuItem[member.Team.Members.Length];
            TeamMember[] members = member.Team.Members;

            for (var i = 0; i < members.Length; i++) {

                var e = members[i];
                MenuButton btn = null;

                int health = e.Health;
                int max = e.MaxHealth;

                if (health <= max / 2) {

                    // The target has less than half its life left.
                    btn = new MenuButton(e.Position.ToString() + "(Medium health)", () => Perform(e));

                } else if (health <= max / 4) {

                    // The target has less than 1/4 of its life left.
                    btn = new MenuButton(e.Position.ToString() + "(Low health)", () => Perform(e));

                } else {

                    // The target's health is too great to heal it.
                    btn = new MenuButton(e.Position.ToString(), () => { });

                }

                buttons[i] = btn;

            }

            _abilityMenu = new Menu("Ability Menu", buttons);

            Menus.ThemeMenu(_abilityMenu, false);
            Main.MainViewport.AddMenu(_abilityMenu);

        }

        public TeamMember.TeamMemberPosition Position {
            get { return TeamMember.TeamMemberPosition.Medic; }
        }

        private AbilityMedic IncrementAbilityPoints(int value) {
            if (AbilityPoints + value <= MaxAbilityPoints)
                AbilityPoints += value;
            return this;
        }

        private AbilityMedic DecrementAbilityPoints(int value) {
            if (AbilityPoints - value >= 0)
                AbilityPoints -= value;
            return this;
        }

        public int MaxAbilityPoints {
            get { return 100;  }
        }

        public int AbilityPoints {
            get { return _abilityPoints; }
            private set { _abilityPoints = value; }
        }

    }

}
