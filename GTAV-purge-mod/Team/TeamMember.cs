using System.Diagnostics;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTAV_purge_mod.Ability;

namespace GTAV_purge_mod.Team {

    public class TeamMember : Updater {

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
        private IAbility _ability;

        private WeaponHash[] _weapons;
        private VehicleSeat _preferredSeat = VehicleSeat.Any;
        private WeaponHash _preferredWeapon;
        private Blip _blip;

        private Team _team;

        private TeamMemberRank _rank = TeamMemberRank.StreetRat;

        private int _accuracy;
        private int _armor;
        private int _money;
        private int _health = 100;
        private int _maxHealth = 100;

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

        protected override void OnUpdate(int tick) {

            if (Ped != null && Ped.IsAlive) {
                IsActive = true;
            } else {
                IsActive = false;
            }

            if (Ped != null && Ped.IsPlayer) {

            }
            
        }

        protected override void OnFirstUpdate() {}

        protected override void OnActiveUpdate(int activeTick, int tick) {}

        protected override void OnInactiveUpdate(int inactiveTick, int tick) {

            // HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY

            if (Ped != null && Ped.IsDead) {

                Team.OnMemberKilled(this);

            }

            if (_blip != null) {
                _blip.Remove();
                _blip = null;
            }

        }

        protected override void OnFirstActiveUpdate(int tick) {

            Ped.RelationshipGroup = Team.Group;

            if (_blip == null && !Ped.IsPlayer) {
                _blip = Ped.AddBlip();
                _blip.Color = Team.Color;
            }

        }

        protected override void OnFirstInactiveUpdate(int tick) {}

        #region "Properties"

        public IAbility Ability {
            get {
                return _ability;
            }
        }

        public WeaponHash PreferredWeapon {
            private get {
                return _preferredWeapon;
            }
            set {
                _preferredWeapon = value;
            }
        }

        public VehicleSeat PreferredSeat {
            get {
                return _preferredSeat;
            }
            set {
                _preferredSeat = value;
            }
        }

        public WeaponHash[] Weapons {
            private get {
                return _weapons;
            }
            set {
                _weapons = value;
            }
        }

        public TeamMemberPosition Position {
            get {
                return _position;
            }
            set {
                _position = value;
                UpdateForPosition(_position);
            }
        }

        public TeamMemberRank Rank {
            get {
                return _rank;
            }
            set {
                _rank = value;
            }
        }

        public Blip Blip {
            get {
                return _blip;
            }
        }

        public Team Team {
            get {
                return _team;
            }
            set {
                _team = value;
            }
        }

        private int Accuracy {
            get {
                return (IsActive ? Ped.Accuracy : _accuracy);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.Accuracy = value;
                    return;
                }
                _accuracy = value;
            }
        }

        public int Armor {
            get {
                return (IsActive ? Ped.Health : _armor);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.Armor = value;
                } else {
                    _armor = value;   
                }
            }
        }

        public int Money {
            get {
                return (IsActive ? Ped.Money : _money);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.Money = value;
                } else {
                    _money = value;
                }
                
            }
        }

        public float Heading {
            get {
                return (IsActive ? Ped.Heading : _heading);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.Heading = value;
                } else {
                    _heading = value;
                }
            }
        }

        public int Health {
            get {
                return (IsActive ? Ped.Health : _health);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.Health = value;
                } else {
                    _health = value;
                }
            }
        }

        public int MaxHealth {
            get {
                return (IsActive ? Ped.MaxHealth : _maxHealth);
            }
            set {
                if (Ped != null && IsActive) {
                    Ped.MaxHealth = value;
                } else {
                    _maxHealth = value;
                }
            }
        }

        public Ped Ped {
            get;
            private set;
        }

        public PedHash ModelHash {
            get {
                return _modelHash;
            }
        }

        #endregion

        #region "Methods"

        private void UpdateForPosition(TeamMemberPosition position) {
            switch (position) {
                case TeamMemberPosition.Medic:
                    _ability = new AbilityMedic();
                    break;
            }
        }

        private void ApplyChanges() {

            if (Ped != null && IsActive && !Ped.IsPlayer) {

                Ped.Weapons.RemoveAll();

                foreach (var weapon in Weapons) {
                    Ped.Weapons.Give(weapon, 100, true, true);
                }

                if (Ped.Weapons[PreferredWeapon] != null) {
                    Ped.Weapons.Select(Ped.Weapons[PreferredWeapon]);
                }

                Ped.Accuracy = _accuracy;
                Ped.Armor = _armor;
                Ped.Money = _money;
                Ped.Heading = _heading;

                Ped.MaxHealth = _maxHealth;
                Ped.Health = _health;

            }

        }

        public TeamMember[] NearbyMembers(float dist) {
            return Team.Members.Where(t => Ped.IsNearEntity(t.Ped, new Vector3(dist, dist, dist))).ToArray();
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

        #endregion
       
    }

}