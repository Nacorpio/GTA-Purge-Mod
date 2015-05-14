using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Position {

    public class PositionMedic : MemberPosition {

        public PositionMedic(TeamMember member) : base(member, Spawn, Heal) {
        }

        private static void Spawn() {
            
        }

        private static void Heal(object[] pars) {

            // Check if the first parameter is a target to heal.
            if (pars[0] is TeamMember) {

                // Convert the argument to a TeamMember instance.
                TeamMember member = (TeamMember) pars[0];

                // The member must be alive and active.
                if (member.IsActive) {

                    member.Health += 25;
                    member.OnHealFromMedic(null);

                }

            }

        }

    }

}
