using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Ability {

    public interface IAbility {

        /// <summary>
        /// Perform this ability with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments to pass through the ability.</param>
        /// <returns></returns>
        bool Perform(params object[] args);

        /// <summary>
        /// Shows the ability-specific menu with the specified member taken into consideration.
        /// </summary>
        /// <param name="member">The TeamMember to take into consideration.</param>
        void ShowMenu(TeamMember member);

        /// <summary>
        /// The TeamMemberPosition that is required to perform this ability.
        /// </summary>
        TeamMember.TeamMemberPosition Position { get; }

        /// <summary>
        /// Returns the amount of ability points that are remaining for use.
        /// </summary>
        int AbilityPoints { get; }

        /// <summary>
        /// Returns the maximum amount of ability points that can be stored.
        /// </summary>
        int MaxAbilityPoints { get; }

    }

}
