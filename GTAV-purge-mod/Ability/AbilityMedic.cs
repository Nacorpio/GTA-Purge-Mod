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

        public void Perform(params object[] args) {

            if (args.Length == 2) {

                var target = (TeamMember) args[0];
                var increment = (int) args[1];

                if (target != null && increment != 0) {
                    if (AbilityPoints - increment >= 0) {

                        if (target.Health < target.MaxHealth) {

                            if (target.Health + increment <= target.MaxHealth) {

                                target.Health += increment;
                                DecrementAbilityPoints(increment);

                            } else {
                                // The target can't have its health incremented by the specified amount.
                            }

                        } else {
                            // The target doesn't need healing.  
                        }

                    } else {
                        // There's not enough ability points to spend.
                    }
                } else {
                    // There's no valid target.  
                }

            }

        }

        public void ShowMenu(TeamMember member) {
            
            MenuItem[] buttons = new MenuItem[member.Team.Members.Length];
            TeamMember[] members = member.Team.Members;

            for (var i = 0; i < members.Length; i++) {

                var e = members[i];
                MenuButton btn = null;

                if (e.Health <= e.MaxHealth/2) {
                    btn = new MenuButton(e.Position.ToString() + "(Low Health)", () => Perform(e));
                }
                else {
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
