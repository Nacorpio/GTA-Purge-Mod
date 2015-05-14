using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTAV_purge_mod.Team;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod {

    public static class Menus {

        private static Menu _participatingTeamsMenu = null;

        private static Menu _teamMenu = null;
        private static Menu _teamVehicleMenu = null;
        private static Menu _teamMemberMenu = null;

        private static readonly Menu MainMenu = new Menu("Main-menu", new MenuItem [] {
            new MenuButton("Participating teams", ShowParticipatingTeams)
        });

        /// <summary>
        /// Process the key being pressed by the user.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public static void ProcessKey(KeyEventArgs e) {
            if (e.KeyCode == Keys.F3) {
                ShowMainMenu();
            }
        }

        /// <summary>
        /// Shows the main-menu.
        /// </summary>
        private static void ShowMainMenu() {
            Main.MainViewport.AddMenu(MainMenu);
        }

        private static void ShowParticipatingTeams() {

            MenuItem[] buttons = new MenuItem[Main.Teams.Count];

            for (var i = 0; i < Main.Teams.Count; i++) {

                var e = Main.Teams[i];
                buttons[i] = new MenuButton(e.Name, () => ShowTeamMenu(e));

            }

            _participatingTeamsMenu = new Menu("Participating teams", buttons);
            Main.MainViewport.AddMenu(_participatingTeamsMenu);

        }

        private static void ShowTeamMenu(Team.Team team) {
            
            _teamMenu = new Menu("Team (" + team.Name + ")", new MenuItem[] {
                
                new MenuButton("Members (" + team.Members.Length + ")", () => ShowTeamMembers(team)), 
                new MenuButton("Vehicles (" + team.Vehicles.Length + ")", () => ShowTeamVehicles(team)),
                new MenuButton("Statistics", () => ShowTeamStatistics(team))

            });

            Main.MainViewport.AddMenu(_teamMenu);

        }

        private static void ShowTeamMember(TeamMember member) {
            
        }

        private static void ShowTeamVehicle(TeamVehicle vehicle) {
            
        }

        private static void ShowTeamStatistics(Team.Team team) {
            
        }

        private static void ShowTeamVehicles(Team.Team team) {
            
            MenuItem[] buttons = new MenuItem[team.Vehicles.Length];

            for (var i = 0; i < team.Vehicles.Length; i++) {

                var e = team.Vehicles[i];
                var cap = (e.IsActive ? e.Vehicle.DisplayName : e.ModelHash.ToString()) + " (" + (e.IsActive ? "Active" : "Inactive") + ")";

                buttons[i] = new MenuButton(cap, () => ShowTeamVehicle(e));

            }

            _teamVehicleMenu = new Menu("Vehicles", buttons);
            Main.MainViewport.AddMenu(_teamVehicleMenu);

        }

        private static void ShowTeamMembers(Team.Team team) {
         
            MenuItem[] buttons = new MenuItem[team.Members.Length];

            for (var i = 0; i < team.Members.Length; i++) {

                var e = team.Members[i];
                buttons[i] = new MenuButton(e.Position.ToString() + " (" + (e.IsActive ? "Active" : "Inactive") + ")", () => ShowTeamMember(e));

            }

            _teamMemberMenu = new Menu("Members", buttons);
            Main.MainViewport.AddMenu(_teamMemberMenu);
   
        }

    }

}
