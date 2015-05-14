using GTA;
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
                    Position = TeamMember.TeamMemberPosition.Engineer,

                    Weapons = new[] {
                        WeaponHash.CombatPistol,
                        WeaponHash.AssaultRifle
                    },

                    PreferredWeapon = WeaponHash.CombatPistol

                },

                new TeamMember(PedHash.OldMan2) {

                    Position = TeamMember.TeamMemberPosition.Gunman

                },

                new TeamMember(PedHash.Bouncer01SMM) {

                    Money = 0,
                    Health = 100,
                    Position = TeamMember.TeamMemberPosition.Tank,

                    Weapons = new[] {
                        WeaponHash.GrenadeLauncher,
                        WeaponHash.SawnOffShotgun
                    },

                    PreferredWeapon = WeaponHash.SawnOffShotgun

                }

            }) {
            
        }

    }

}