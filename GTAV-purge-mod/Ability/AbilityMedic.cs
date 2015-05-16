using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTAV_purge_mod.Team;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod.Ability {

    public class AbilityMedic : IAbility {

        private Menu _abilityMenu;

        public bool Perform(params object[] args) {

            if (args.Length == 1) {

                var target = (TeamMember) args[0];

                if (target != null) {

                    target.Health = target.MaxHealth;

                    if (target.Ped.IsInjured)
                        Function.Call(Hash.REVIVE_INJURED_PED, target.Ped);

                    return true;

                }

            }

            return false;

        }

        public int AbilityPoints { get; private set; }

        public void ShowMenu(TeamMember member) {

            TeamMember[] members = member.Team.Members;
            MenuItem[] buttons = new MenuItem[members.Length];
            
            for (var i = 0; i < members.Length; i++) {

                var e = members[i];
                MenuButton btn = null;
                var desc = "";

                if (!e.IsActive) {

                    desc = "This member is inactive, and can not be healed!";
                    btn = new MenuButton(e.Position.ToString() + " (Inactive)", desc, () => {});
                    buttons[i] = btn;

                    continue;

                }

                if (e.Ped.IsPlayer) {

                    desc = "You can not heal yourself!";
                    btn = new MenuButton("You (" + e.Position.ToString() + ")", desc, () => {});
                    buttons[i] = btn;

                    continue;

                }

                var health = e.Health;
                var max = e.MaxHealth;

                desc = (health == max ? "This member has full health!" : 
                String.Format("This member has {0} HP and needs {1} HP to have full health!", health, max - health));

                btn = new MenuButton(e.Position.ToString() + " (" + health + " / " + max + " HP)", desc, () => Perform(e));
                buttons[i] = btn;

            }

            _abilityMenu = new Menu("Use Ability (Heal member)", buttons);

            Menus.ThemeMenu(_abilityMenu, true);
            Main.MainViewport.AddMenu(_abilityMenu);

        }

        public TeamMember.TeamMemberPosition Position {
            get { return TeamMember.TeamMemberPosition.Medic; }
        }

        public int MaxAbilityPoints {
            get { return 100;  }
        }

    }

}
