using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Ability {

    public interface IAbility {

        void Perform(params object[] args);

        void ShowMenu(TeamMember member);

        TeamMember.TeamMemberPosition Position { get; }

        int AbilityPoints { get; }

        int MaxAbilityPoints { get; }

    }

}
