using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTAV_purge_mod.Ability;
using GTAV_purge_mod.Team;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod {

    public static class MemberMenuFunctions {

        public static void SpawnOrDespawn(TeamMember member) {
            if (!member.IsActive) {
                member.Create(Main.Player.Character.Position, true);
            } else {
                member.Ped.Delete();
                Main.MainViewport.HandleBack();
            }
        }

        public static void Kill(TeamMember member) {
            if (member.IsActive) {
                member.Ped.Kill();
                Main.MainViewport.HandleBack();
            }
        }

        public static void TeleportTo(Ped ped, TeamMember member) {
            if (member.IsActive) {
                ped.Position = member.Ped.Position.Around(2f);
            }
        }

        public static void Teleport(Ped ped, TeamMember member) {
            if (member.IsActive) {
                member.Ped.Position = ped.Position;
            }
        }

        public static void WarpIntoVehicle(TeamVehicle vehicle, TeamMember member) {

            if (member.IsActive) {

                if (!vehicle.IsActive) {
                    vehicle.Create(member.Ped.Position, true);
                }

                string[] seats = Enum.GetNames(typeof(VehicleSeat));
                foreach (var s in seats) {

                    var seat = (VehicleSeat)Enum.Parse(typeof(VehicleSeat), s);
                    var ped = vehicle.Vehicle.GetPedOnSeat(seat);

                    if (ped == null)
                        member.Ped.Task.WarpIntoVehicle(vehicle.Vehicle, seat);

                }

                Main.MainViewport.HandleBack();

            }

        }

    }

    public static class VehicleMenuFunctions {

        public static void SpawnOrDespawn(Ped ped, TeamVehicle vehicle) {
            if (!vehicle.IsActive) {
                vehicle.Create(ped.Position, true);
                Main.MainViewport.HandleBack();
            } else {
                vehicle.Vehicle.Delete();
                Main.MainViewport.HandleBack();
            }
        }

        public static void KillAllPassengers(TeamVehicle vehicle) {

            if (vehicle.IsActive) {

                string[] seats = Enum.GetNames(typeof (VehicleSeat));
                foreach (var s in seats) {

                    var seat = (VehicleSeat) Enum.Parse(typeof (VehicleSeat), s);
                    var ped = vehicle.Vehicle.GetPedOnSeat(seat);

                    if (ped != null)
                        ped.Kill();

                }

            }

        } 

        public static void WarpInto(Ped ped, TeamVehicle vehicle) {

            if (vehicle.IsActive) {

                string[] seats = Enum.GetNames(typeof (VehicleSeat));
                foreach (var s in seats) {

                    var seat = (VehicleSeat) Enum.Parse(typeof (VehicleSeat), s);
                    if (vehicle.Vehicle.GetPedOnSeat(seat) == null)
                        ped.Task.WarpIntoVehicle(vehicle.Vehicle, seat);

                }

            }

        }

        public static void Teleport(Ped ped, TeamVehicle vehicle) {
            if (vehicle.IsActive) {
                vehicle.Vehicle.Position = ped.Position.Around(5f);
            }
        }

        public static void Explode(TeamVehicle vehicle) {
            if (vehicle.IsActive) {
                vehicle.Vehicle.Explode();
                Main.MainViewport.HandleBack();
            }
        }

    }

    public static class Menus {

        private const string MethodInactive = "This method is inactive and can not be used";

        private static Menu _participatingTeamsMenu = null;

        private static Menu _teamMenu = null;
        private static Menu _teamVehicleMenu = null;
        private static Menu _teamMemberMenu = null;

        private static Menu _teamShowMemberMenu = null;
        private static Menu _teamShowVehicleMenu = null;

        private static Menu _teamMemberWarpIntoVehicleMenu = null;

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

            ThemeMenu(MainMenu, false);
            Main.MainViewport.AddMenu(MainMenu);

        }

        public static void ThemeMenu(Menu menu, bool footer) {

            if (footer) {

                menu.HasFooter = true;
                menu.FooterTextScale = .35f;
                menu.FooterCentered = true;
                menu.FooterHeight = 20;

                menu.FooterTextColor = Color.DarkSlateGray;
                menu.FooterColor = Color.Gray;

            }

            menu.HeaderColor = Color.DarkSlateGray;
            menu.HeaderTextColor = Color.White;

            menu.ItemTextCentered = true;
            menu.ItemTextScale = .35f;
            menu.ItemHeight = 20;

            menu.UnselectedItemColor = Color.LightGray;
            menu.SelectedItemColor = Color.DarkGray;

            menu.Width = 500;

        }

        private static void ShowParticipatingTeams() {

            MenuItem[] buttons = new MenuItem[Main.Teams.Count];

            for (var i = 0; i < Main.Teams.Count; i++) {

                var e = Main.Teams[i];
                buttons[i] = new MenuButton(e.Name, String.Format(e.Name + " is participating with {0} vehicles & {1} members.", e.Vehicles.Length, e.Members.Length), () => ShowTeamMenu(e));

            }

            _participatingTeamsMenu = new Menu("Participating teams", buttons);

            ThemeMenu(_participatingTeamsMenu, true);
            Main.MainViewport.AddMenu(_participatingTeamsMenu);

        }

        private static void ShowTeamMenu(Team.Team team) {

            MenuItem[] buttons = new MenuItem[4];

            buttons[0] = new MenuButton("Members (" + team.Members.Length + ")", () => ShowTeamMembers(team));
            buttons[1] = new MenuButton("Vehicles (" + team.Vehicles.Length + ")", () => ShowTeamVehicles(team));
            buttons[2] = new MenuButton("Statistics", () => ShowTeamStatistics(team));

            if (team.IsTeamMember(Main.Player.Character)) {

                var playerMember = team.ToTeamMember(Main.Player.Character);
                if (playerMember == null) {

                    // Member doesn't exist for some reason.
                    return;

                }

                var ability = playerMember.Ability;
                if (ability == null) {
                    // The player has no ability.
                    return;
                }

                // If we're at this point, we know the player has an ability.
                // We can now use the ability.

                buttons[3] = new MenuButton("Use Ability (" + ability.Position.ToString() + ")",
                    () => ability.ShowMenu(playerMember));

            } else {

                buttons[3] = new MenuButton("There is no ability available for use!", () => { });

            }

            _teamMenu = new Menu("Team (" + team.Name + ")", buttons);

            ThemeMenu(_teamMenu, false);
            Main.MainViewport.AddMenu(_teamMenu);

        }

        private static void ShowTeamMember(TeamMember member) {

            var active = member.IsActive;
            _teamShowMemberMenu = new Menu("Member (" + member.Position.ToString() + ")", new MenuItem[] {
                
                new MenuButton((member.IsActive ? "Despawn" : "Spawn"), (active ? "Despawns the member" : "Spawns the member"), () => MemberMenuFunctions.SpawnOrDespawn(member)),
                new MenuButton((member.IsActive ? "Teleport To" : "Teleport To (inactive)"), (active ? "Teleport to the member" : MethodInactive), () => MemberMenuFunctions.TeleportTo(Main.Player.Character, member)),
                new MenuButton((member.IsActive ? "Teleport" : "Teleport (inactive)"), (active ? "Teleport the member to your location" : MethodInactive), () => MemberMenuFunctions.Teleport(Main.Player.Character, member)),
                new MenuButton((member.IsActive ? "Kill" : "Kill (inactive)"), (active ? "Kill the member" : MethodInactive), () => MemberMenuFunctions.Kill(member)),
                new MenuButton((member.IsActive ? "Warp Into Vehicle" : "Warp Into Vehicle (inactive)"), (active ? "Warp the member into a team vehicle" : MethodInactive), () => ShowVehiclesMenuAsMember(member)), 
                new MenuButton((member.IsActive ? "Reset" : "Reset (inactive)"), (active ? "Reset the member" : MethodInactive), () => { })

            });

            ThemeMenu(_teamShowMemberMenu, true);
            Main.MainViewport.AddMenu(_teamShowMemberMenu);

        }

        private static void ShowVehiclesMenuAsMember(TeamMember member) {

            Team.Team team = member.Team;
            MenuItem[] buttons = new MenuItem[team.Vehicles.Length];

            for (var i = 0; i < team.Vehicles.Length; i++) {

                var e = team.Vehicles[i];
                var cap = (e.IsActive ? e.Vehicle.DisplayName : e.ModelHash.ToString()) + " (" + (e.IsActive ? "Active" : "Inactive") + ")";

                buttons[i] = new MenuButton(cap, () => MemberMenuFunctions.WarpIntoVehicle(e, member));

            }

            _teamMemberWarpIntoVehicleMenu = new Menu("Warp Into Vehicle", buttons);

            ThemeMenu(_teamMemberWarpIntoVehicleMenu, false);
            Main.MainViewport.AddMenu(_teamMemberWarpIntoVehicleMenu);

        }

        private static void ShowTeamVehicle(TeamVehicle vehicle) {

            _teamShowVehicleMenu = new Menu("Vehicle (" + (vehicle.IsActive ? vehicle.Vehicle.DisplayName : vehicle.ModelHash.ToString()) + ")", new MenuItem[] {
                
                new MenuButton((vehicle.IsActive ? "Despawn" : "Spawn"), (vehicle.IsActive ? "Despawns the vehicle" : "Spawns the vehicle"), () => VehicleMenuFunctions.SpawnOrDespawn(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Warp Into" : "Warp Into (inactive)"), (vehicle.IsActive ? "Warp into the vehicle" : MethodInactive), () => VehicleMenuFunctions.WarpInto(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Teleport" : "Teleport (inactive)"), (vehicle.IsActive ? "Teleport the vehicle to your location" : MethodInactive), () => VehicleMenuFunctions.Teleport(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Explode" : "Explode (inactive)"), (vehicle.IsActive ? "Explode the vehicle" : MethodInactive), () => VehicleMenuFunctions.Explode(vehicle)),
                new MenuButton((vehicle.IsActive ? "Kill Passengers" : "Kill Passengers (inactive)"), (vehicle.IsActive ? "Kill the passengers of the vehicle" : MethodInactive), () => VehicleMenuFunctions.KillAllPassengers(vehicle))

            });

            ThemeMenu(_teamShowVehicleMenu, true);
            Main.MainViewport.AddMenu(_teamShowVehicleMenu);

        }

        private static void ShowTeamStatistics(Team.Team team) {
            
            MenuItem[] buttons = new MenuItem[team.Vehicles.Length];

            for (var i = 0; i < team.Vehicles.Length; i++) {

                var e = team.Vehicles[i];
                var cap = (e.IsActive ? e.Vehicle.DisplayName : e.ModelHash.ToString()) + " (" + (e.IsActive ? "Active" : "Inactive") + ")";

                buttons[i] = new MenuButton(cap, () => ShowTeamVehicle(e));

            }

        }

        private static void ShowTeamVehicles(Team.Team team) {
            
            MenuItem[] buttons = new MenuItem[team.Vehicles.Length];

            for (var i = 0; i < team.Vehicles.Length; i++) {

                var e = team.Vehicles[i];
                var cap = (e.IsActive ? e.Vehicle.DisplayName : e.ModelHash.ToString()) + " (" + (e.IsActive ? "Active" : "Inactive") + ")";

                buttons[i] = new MenuButton(cap, () => ShowTeamVehicle(e));

            }

            _teamVehicleMenu = new Menu("Vehicles", buttons);

            ThemeMenu(_teamVehicleMenu, false);
            Main.MainViewport.AddMenu(_teamVehicleMenu);

        }

        private static void ShowTeamMembers(Team.Team team) {
         
            MenuItem[] buttons = new MenuItem[team.Members.Length];

            for (var i = 0; i < team.Members.Length; i++) {

                var e = team.Members[i];
                buttons[i] = new MenuButton(e.Position.ToString() + " (" + (e.IsActive ? "Active" : "Inactive") + ")", () => ShowTeamMember(e));

            }

            _teamMemberMenu = new Menu("Members", buttons);

            ThemeMenu(_teamMemberMenu, false);
            Main.MainViewport.AddMenu(_teamMemberMenu);
   
        }

    }

}
