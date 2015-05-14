using System;
using System.Windows.Forms;
using GTA;
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
            }
        }

        public static void Kill(TeamMember member) {
            if (member.IsActive) {
                member.Ped.Kill();
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

            }

        }

    }

    public static class VehicleMenuFunctions {

        public static void SpawnOrDespawn(Ped ped, TeamVehicle vehicle) {
            if (!vehicle.IsActive) {

                if (vehicle != null) {
                    vehicle.Create(ped.Position, true);
                } else {
                    Main.DebugText.Caption = "Vehicle is null?!";
                }
                    
            } else {
                vehicle.Vehicle.Delete();
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
            }
        }

    }

    public static class Menus {

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
            
            _teamShowMemberMenu = new Menu("Member (" + member.Position.ToString() + ")", new MenuItem[] {
                
                new MenuButton((member.IsActive ? "Despawn" : "Spawn"), () => MemberMenuFunctions.SpawnOrDespawn(member)),
                new MenuButton((member.IsActive ? "Teleport To" : "Teleport To (inactive)"), () => MemberMenuFunctions.TeleportTo(Main.Player.Character, member)),
                new MenuButton((member.IsActive ? "Teleport" : "Teleport (inactive)"), () => MemberMenuFunctions.Teleport(Main.Player.Character, member)),
                new MenuButton((member.IsActive ? "Kill" : "Kill (inactive)"), () => MemberMenuFunctions.Kill(member)),
                new MenuButton((member.IsActive ? "Warp Into Vehicle" : "Warp Into Vehicle (inactive)"), () => ShowVehiclesMenuAsMember(member)), 
                new MenuButton((member.IsActive ? "Reset" : "Reset (inactive)"), null)

            });

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
            Main.MainViewport.AddMenu(_teamMemberWarpIntoVehicleMenu);

        }

        private static void ShowTeamVehicle(TeamVehicle vehicle) {

            _teamShowVehicleMenu = new Menu("Vehicle (" + (vehicle.IsActive ? vehicle.Vehicle.DisplayName : vehicle.ModelHash.ToString()) + ")", new MenuItem[] {
                
                new MenuButton((vehicle.IsActive ? "Despawn" : "Spawn"), () => VehicleMenuFunctions.SpawnOrDespawn(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Warp Into" : "Warp Into (inactive)"), () => VehicleMenuFunctions.WarpInto(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Teleport" : "Teleport (inactive)"), () => VehicleMenuFunctions.Teleport(Main.Player.Character, vehicle)),
                new MenuButton((vehicle.IsActive ? "Explode" : "Explode (inactive)"), () => VehicleMenuFunctions.Explode(vehicle)),
                new MenuButton((vehicle.IsActive ? "Kill Passengers" : "Kill Passengers (inactive)"), () => VehicleMenuFunctions.KillAllPassengers(vehicle))

            });

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
