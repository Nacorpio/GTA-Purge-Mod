using GTA;
using GTA.Math;
using GTA.Native;
using GTAV_purge_mod.Position;

namespace GTAV_purge_mod.Team {

    public class TeamMember {

        public enum TeamMemberPosition {
            Gunman = 0,
            Medic = 1,
            Engineer = 2,
            Tank = 3,
            Ninja = 4
        }

        public enum TeamMemberRank {
            StreetRat = 0,
            Thug = 1,
            Soldier = 2,
            MadeMan = 3,
            Lieutenant = 4,
            Boss = 5
        }

        private readonly PedHash _modelHash;
        private TeamMemberPosition _position = TeamMemberPosition.Gunman;
        private VehicleSeat _preferredSeat = VehicleSeat.Any;
        private TeamMemberRank _rank = TeamMemberRank.StreetRat;

        public TeamMember(Ped ped, bool changes) {
            Ped = ped;
            _modelHash = (PedHash) ped.Model.Hash;

            if (changes) {
                ApplyChanges();
            }
        }

        public void OnHealFromMedic(PositionMedic medic) {}

        public TeamMember(PedHash modelHash) {
            _modelHash = modelHash;
        }

        public VehicleSeat PreferredSeat {
            get { return _preferredSeat; }
            set { _preferredSeat = value; }
        }

        public WeaponHash[] Weapons { get; set; }

        public TeamMemberPosition Position {
            get { return _position; }
            set { _position = value; }
        }

        public TeamMemberRank Rank {
            get { return _rank; }
            set { _rank = value; }
        }

        private int Accuracy {
            get { return (IsActive ? Ped.Accuracy : Accuracy); }
            set {
                if (IsActive) {
                    Ped.Accuracy = value;
                } else {
                    Accuracy = value;
                }
            }
        }

        public int Armor {
            get { return (IsActive ? Ped.Health : Armor); }
            set {
                if (IsActive) {
                    Ped.Armor = value;
            } else {
                    Armor = value;
                }
            }
        }

        public int Money {
            get { return (IsActive ? Ped.Money : Money); }
            set {
                if (IsActive) {
                    Ped.Money = value;
                } else {
                    Money = value;
                }
            }
        }

        public float Heading {
            get { return (IsActive ? Ped.Heading : Heading); }
            set {
                if (IsActive) {
                    Ped.Heading = value;
                } else {
                    Heading = value;
                }
            }
        }

        public int Health {
            get { return (IsActive ? Ped.Health : Health); }
            set {
                if (IsActive) {
                    Ped.Health = value;
                } else {
                    Health = value;
                }
            }
        }

        public bool IsActive { get; private set; }

        public Ped Ped {
            get;
            private set;
        }

        public PedHash ModelHash {
            get { return _modelHash; }
        }

        public void Update(int tick) {
            if (Ped != null && Ped.IsAlive) {
                IsActive = true;
            }
            else {
                IsActive = false;
            }
        }

        private void ApplyChanges() {
            if (Ped != null) {
                // void GIVE_WEAPON_TO_PED(int pedHandle, Hash weaponAssetHash, int ammoCount, BOOL equipNow, BOOL isAmmoLoaded)
                for (var i = 0; i < Weapons.Length; i++) {
                    var e = Weapons[i];
                    Function.Call(Hash.GIVE_WEAPON_TO_PED, Ped, (int) e, 666, true, true);
                }

                Ped.Accuracy = Accuracy;
                Ped.Armor = Armor;
                Ped.Money = Money;
                Ped.Heading = Heading;
                Ped.Health = Health;
            }
        }

        public TeamMember Create(Vector3 pos, bool changes) {
            var ped = World.CreatePed(_modelHash, pos);

            Ped = ped;
            IsActive = true;

            if (changes) {
                ApplyChanges();
            }

            return this;
        }
    }

}