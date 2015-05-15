using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Position {

    public class PositionMedic : MemberPosition {

        private static int _healPoints = 0;

        private Menu _medicMenu;
        private Menu _medicHealMembersMenu;

        public PositionMedic(TeamMember member)
            : base(member, Spawn, Heal) {
        }

        private static void Spawn() {
            _healPoints = 5;
        }

        private static void Heal(object[] pars) {

            // Check if the first parameter is a target to heal.
            if (pars[0] is TeamMember) {

                // Convert the argument to a TeamMember instance.
                TeamMember member = (TeamMember)pars[0];

                // The member must be alive and active.
                if (member.IsActive) {

                    if (_healPoints - 1 >= 0) {

                        member.Health = member.MaxHealth - 25;
                        _healPoints--;

                    } else {

                        Main.DebugText.Caption = "";

                    }

                }

            }

        }

        private void ShowMedicHealMemberMenu() {
         
            MenuItem[] buttons = new MenuItem[(Member.Team.Members.Length > 0 ? Member.Team.Members.Length : 1)];

            if (Member.Team.Members.Length > 0) {
                for (var i = 0; i < Member.Team.Members.Length; i++) {

                    var e = Member.Team.Members[i];

                    if (e.IsActive) {
                        if (e.Ped.Health < e.Ped.MaxHealth) {

                            string caption = e.Position.ToString() + " (" + e.Ped.Health + "/" + e.Ped.MaxHealth + "hp)";
                            buttons[i] = new MenuButton(caption, () => Heal(new object[] {e}));

                        }
                    }

                }
            } else {
                buttons[0] = new MenuButton("No members to heal", null);
            }
            
            _medicHealMembersMenu = new Menu("Heal Member", buttons);
            Main.MainViewport.AddMenu(_medicHealMembersMenu);

        }

        public override void OpenMenu(Viewport view) {
            
            _medicMenu = new Menu("Medic", new MenuItem[] {
                new MenuButton("Heal Member", ShowMedicHealMemberMenu), 
            });

            Main.MainViewport.AddMenu(_medicMenu);

        }
    }

}
