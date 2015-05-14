using GTA;
using GTA.Math;
using GTA.Native;

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

        private WeaponHash[] _weapons = new WeaponHash[] {WeaponHash.Knife};
        private VehicleSeat _preferredSeat = VehicleSeat.Any;
        private WeaponHash _preferredWeapon;

        private TeamMemberRank _rank = TeamMemberRank.StreetRat;

        private int _accuracy;
        private int _armor;
        private int _money;
        private int _health;

        private float _heading;

        public TeamMember(Ped ped, bool changes) {

            Ped = ped;

            if (changes) {
                ApplyChanges();
            }

        }

        public TeamMember(PedHash modelHash) {

            _modelHash = modelHash;

        }

        public WeaponHash PreferredWeapon {
            private get { return _preferredWeapon; }
            set { _preferredWeapon = value; }
        }

        public VehicleSeat PreferredSeat {
            get { return _preferredSeat; }
            set { _preferredSeat = value; }
        }

        public WeaponHash[] Weapons {
            private get { return _weapons; }
            set { _weapons = value; }
        }

        public TeamMemberPosition Position {
            get { return _position; }
            set { _position = value; }
        }

        public TeamMemberRank Rank {
            get { return _rank; }
            set { _rank = value; }
        }

        private int Accuracy {
            get {
                return (IsActive ? Ped.Accuracy : _accuracy);
            }
            set { _accuracy = value; }
        }

        public int Armor {
            get {
                return (IsActive ? Ped.Health : _armor);
            }
            set { _armor = value; }
        }

        public int Money {
            get {
                return (IsActive ? Ped.Money : _money);
            }
            set { _money = value; }
        }

        public float Heading {
            get {
                return (IsActive ? Ped.Heading : _heading);
            }
            set { _heading = value; }
        }

        public int Health {
            get {
                return (IsActive ? Ped.Health : _health);
            }
            set { _health = value; }
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

            } else {

                IsActive = false;

            }

        }

        private void ApplyChanges() {

            if (Ped != null && IsActive) {

                // void GIVE_WEAPON_TO_PED(int pedHandle, Hash weaponAssetHash, int ammoCount, BOOL equipNow, BOOL isAmmoLoaded)

                foreach (WeaponHash weapon in Weapons) {
                    Ped.Weapons.Give(weapon, 100, true, true);
                }

                if (Ped.Weapons[PreferredWeapon] != null) {
                    Ped.Weapons.Select(Ped.Weapons[PreferredWeapon]);
                }

                Ped.Accuracy = _accuracy;
                Ped.Armor = _armor;
                Ped.Money = _money;
                Ped.Heading = _heading;
                Ped.Health = _health;

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