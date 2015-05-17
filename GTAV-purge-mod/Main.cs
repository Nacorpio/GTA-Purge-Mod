using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTAV_purge_mod.Team;
using GTAV_purge_mod.Teams;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod {

    public class Main : Script {

        public static TeamHampurgers TeamHampurgers;
        public static TeamNoPurgeIntended TeamNoPurgeIntended;

        public static Team.Team TeamPurgeCops;

        public static UIText DebugText;
        public static Viewport MainViewport;

        public static Player Player;
        public static readonly List<Team.Team> Teams = new List<Team.Team>();

        public Main() {

            Ticks = 0;

            Tick += OnTick;
            KeyDown += OnKeyDown;

        }

        private static void OnKeyDown(object sender, KeyEventArgs keyEventArgs) {
            Menus.ProcessKey(keyEventArgs);
        }

        private int Ticks { get; set; }

        private void OnTick(object sender, EventArgs e) {

            // First tick.
            if (Ticks == 0) {

                MainViewport = View;
                DebugText = new UIText("DEBUG!", new Point(100, 100), 1f);

                Player = Game.Player;

                TeamHampurgers = new TeamHampurgers();
                TeamNoPurgeIntended = new TeamNoPurgeIntended();

                TeamHampurgers.Color = BlipColor.Green;
                TeamNoPurgeIntended.Color = BlipColor.Red;

                Teams.Add(TeamHampurgers);
                Teams.Add(TeamNoPurgeIntended);

            }

            foreach (var team in Teams) {
                team.OnTick();
            }
   
            Ticks++;

        }

        public static Team.Team ToTeam(Ped ped) {
            return Teams.Where(t => t.IsTeamMember(ped)).ToArray()[0];
        }

        public static TeamMember[] GetMembers(params Team.Team[] teams) {
            
            List<TeamMember> members = new List<TeamMember>();
            foreach (var team in teams) {
                members.AddRange(team.Members);
            }

            return members.ToArray();

        }

        public static TeamMember[] GetActiveMembers(Team.Team[] teams, TeamMember.TeamMemberPosition position) {
            return GetMembers(teams, position).Where(t => t.IsActive).ToArray();
        }

        public static TeamMember[] GetInactiveMembers(Team.Team[] teams, TeamMember.TeamMemberPosition position) {
            return GetMembers(teams, position).Where(t => !t.IsActive).ToArray();
        }

        public static TeamMember[] GetMembers(Team.Team[] teams, TeamMember.TeamMemberPosition position) {
            return GetMembers(teams).Where(t => t.Position == position).ToArray();
        }

        public static TeamMember[] GetMembers(Team.Team team) {
            return team.Members;
        }

        public static TeamMember[] GetMembers(Team.Team team, TeamMember.TeamMemberPosition position) {
            return GetMembers(team).Where(t => t.Position == position).ToArray();
        }

        public static TeamMember[] GetMembers(Team.Team team, TeamMember.TeamMemberPosition position,
            TeamMember.TeamMemberRank rank) {
            return GetMembers(team, position).Where(t => t.Rank == rank).ToArray();
        }

        public static TeamMember[] GetInactiveMembers(Team.Team team, TeamMember.TeamMemberPosition position) {
            return GetInactiveMembers(team).Where(m => m.Position == position).ToArray();
        }

        public static TeamMember[] GetInactiveMembers(Team.Team team, TeamMember.TeamMemberRank rank) {
            return GetInactiveMembers(team).Where(t => t.Rank == rank).ToArray();
        }

        public static TeamMember[] GetInactiveMembers(Team.Team team) {
            return GetMembers(team).Where(m => !m.IsActive).ToArray();
        }

        public static TeamMember[] GetInactiveMembers(Team.Team team, TeamMember.TeamMemberPosition position,
            TeamMember.TeamMemberRank rank) {
            return GetInactiveMembers(team, position).Where(t => t.Rank == rank).ToArray();
        }

        public static TeamMember[] GetActiveMembers(Team.Team team, TeamMember.TeamMemberPosition position,
            TeamMember.TeamMemberRank rank) {
            return GetActiveMembers(team, position).Where(t => t.Rank == rank).ToArray();
        }

        public static TeamMember[] GetActiveMembers(Team.Team team, TeamMember.TeamMemberRank rank) {
            return GetActiveMembers(team).Where(t => t.Rank == rank).ToArray();
        }

        public static TeamMember[] GetActiveMembers(Team.Team team, TeamMember.TeamMemberPosition position) {
            return GetActiveMembers(team).Where(t => t.Position == position).ToArray();
        }

        public static TeamMember[] GetActiveMembers(Team.Team team) {
            return GetMembers(team).Where(m => m.IsActive).ToArray();
        }

    }

}