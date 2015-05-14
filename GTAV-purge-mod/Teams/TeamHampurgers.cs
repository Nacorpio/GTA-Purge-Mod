using GTA.Native;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Teams {
    public class TeamHampurgers : Team.Team {
        public TeamHampurgers() : base("Hampurgers",
            new[] {

                new TeamVehicle(VehicleHash.JB700),
                new TeamVehicle(VehicleHash.Sandking2),
                new TeamVehicle(VehicleHash.DLoader),
                new TeamVehicle(VehicleHash.Technical)

            },
            new[] {

                new TeamMember(PedHash.OldMan1a) {
                    Health = 100,
                    Position = TeamMember.TeamMemberPosition.Engineer
                },

                new TeamMember(PedHash.OldMan2) {
                    Health = 100,
                    Position = TeamMember.TeamMemberPosition.Gunman
                }

            }) {}
    }
}