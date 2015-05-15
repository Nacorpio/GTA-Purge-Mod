using GTA;
using GTA.Native;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Teams {

    public class TeamHampurgers : Team.Team {

        public TeamHampurgers() : base("Hampurgers",

            new[] {

                new TeamVehicle(VehicleHash.Baller2) {
                    
                },

                new TeamVehicle(VehicleHash.Sandking2),
                new TeamVehicle(VehicleHash.DLoader),
                new TeamVehicle(VehicleHash.Technical)

            },

            new[] {

                new TeamMember(PedHash.Blackops01SMY) {
                    
                    Position = TeamMember.TeamMemberPosition.Gunman,
                    Armor = 100,

                    Weapons = new [] {
                        WeaponHash.CarbineRifle,
                        WeaponHash.CombatPistol,
                    },

                    PreferredSeat = VehicleSeat.LeftRear,
                    PreferredWeapon = WeaponHash.CarbineRifle

                },

                new TeamMember(PedHash.Armymech01SMY) {

                    Position = TeamMember.TeamMemberPosition.Engineer,
                    Health = 100,

                    Weapons = new[] {
                        WeaponHash.AssaultShotgun,
                        WeaponHash.HeavyPistol
                    },

                    PreferredSeat = VehicleSeat.Driver,
                    PreferredWeapon = WeaponHash.AssaultRifle

                },

                new TeamMember(PedHash.Blackops02SMY) {

                    Position = TeamMember.TeamMemberPosition.Gunman,
                    Armor = 100,

                    Weapons = new [] {
                        WeaponHash.CarbineRifle,
                        WeaponHash.RPG
                    },

                    PreferredSeat = VehicleSeat.RightRear,
                    PreferredWeapon = WeaponHash.CarbineRifle

                },

                new TeamMember(Main.Player.Character, true) {

                    Position = TeamMember.TeamMemberPosition.Medic,
                    Armor = 25,

                    Weapons = new [] {
                        WeaponHash.SNSPistol,
                    },

                    PreferredWeapon = WeaponHash.SNSPistol

                },

                new TeamMember(PedHash.TaoCheng) {

                    Position = TeamMember.TeamMemberPosition.Ninja,
                    Armor = 25,

                    Weapons = new [] {
                        WeaponHash.Dagger,
                        WeaponHash.MicroSMG
                    },

                    PreferredSeat = VehicleSeat.Any,
                    PreferredWeapon = WeaponHash.Dagger

                },

                new TeamMember(PedHash.Bouncer01SMM) {

                    Money = 0,
                    Health = 1000,
                    Position = TeamMember.TeamMemberPosition.Tank,

                    Weapons = new[] {
                        WeaponHash.HeavyShotgun,
                        WeaponHash.GrenadeLauncher
                    },

                    PreferredSeat = VehicleSeat.Passenger,
                    PreferredWeapon = WeaponHash.HeavyShotgun

                }

            }) {
            
        }

    }

}