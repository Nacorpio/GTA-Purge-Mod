using GTA;
using GTA.Native;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Teams {

    public class TeamNoPurgeIntended : Team.Team {

        public TeamNoPurgeIntended() : base("No Purge Intended",

            new [] {
                
                new TeamVehicle(VehicleHash.CogCabrio) {

                    PrimaryColor = VehicleColor.MetallicBlack,
                    SecondaryColor = VehicleColor.MetallicBlack,
                    WindowTint = VehicleWindowTint.PureBlack,

                }
                
                .AddModMax(VehicleMod.Engine)
                .AddModMax(VehicleMod.Suspension)
                .AddModMax(VehicleMod.Armor),

                new TeamVehicle(VehicleHash.Washington) {
                    PrimaryColor = VehicleColor.MetallicBlack,
                    SecondaryColor = VehicleColor.MetallicBlack,
                    WindowTint = VehicleWindowTint.PureBlack,
                }
                
                .AddModMax(VehicleMod.Engine)
                .AddModMax(VehicleMod.Armor),

                new TeamVehicle(VehicleHash.Patriot) {
                    PrimaryColor = VehicleColor.MetallicBlack,
                    SecondaryColor = VehicleColor.MetallicBlack,
                    WindowTint = VehicleWindowTint.PureBlack,
                }

                .AddModMax(VehicleMod.Engine)
                .AddModMax(VehicleMod.Armor)
                .AddModMax(VehicleMod.FrontBumper)
                .AddModMax(VehicleMod.RearBumper),

                new TeamVehicle(VehicleHash.CarbonRS) {
                    PrimaryColor = VehicleColor.MetallicBlack,
                    SecondaryColor = VehicleColor.MetallicBlack,
                } 

                .AddModMax(VehicleMod.Engine)
                .AddModMax(VehicleMod.Armor)

            }, new TeamMember[] {
                
                new TeamMember(PedHash.Business01AMM) {
                    Position = TeamMember.TeamMemberPosition.Gunman,
                    Weapons = new [] {
                        WeaponHash.AssaultRifle, 
                        WeaponHash.CombatPistol
                    },
                    PreferredWeapon = WeaponHash.AssaultRifle,
                    Armor = 100,
                    PreferredSeat = VehicleSeat.RightRear
                },

                new TeamMember(PedHash.Scientist01SMM) {
                    Position = TeamMember.TeamMemberPosition.Medic,
                    Weapons = new [] {
                        WeaponHash.HeavyPistol,
                        WeaponHash.MicroSMG
                    },
                    PreferredWeapon = WeaponHash.CarbineRifle,
                    Armor = 100,
                    PreferredSeat = VehicleSeat.LeftRear
                },

                new TeamMember(PedHash.Business02AMY) {
                    Position = TeamMember.TeamMemberPosition.Engineer,
                    Weapons = new [] {
                        WeaponHash.SMG,
                        WeaponHash.Pistol50
                    },
                    PreferredWeapon = WeaponHash.SMG,
                    Armor = 100,
                    PreferredSeat = VehicleSeat.Driver
                },

                new TeamMember(PedHash.Bouncer01SMM) {
                    Position = TeamMember.TeamMemberPosition.Tank,
                    Weapons = new [] {
                        WeaponHash.Gusenberg,
                        WeaponHash.VintagePistol,
                        WeaponHash.GrenadeLauncher
                    },
                    PreferredWeapon = WeaponHash.Gusenberg,
                    Armor = 150,
                    PreferredSeat = VehicleSeat.Passenger
                }

            })
        
        {
            
        }

    }

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

                    MaxHealth = 1000,
                    Health = 1000,

                    Position = TeamMember.TeamMemberPosition.Tank,

                    Weapons = new[] {
                        WeaponHash.HeavyShotgun,
                        WeaponHash.GrenadeLauncher
                    },

                    PreferredSeat = VehicleSeat.Passenger,
                    PreferredWeapon = WeaponHash.HeavyShotgun

                }

            })
        {
                
        }

    }

}