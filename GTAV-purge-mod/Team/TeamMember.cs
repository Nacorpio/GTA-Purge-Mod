using GTA;
using GTA.Math;
using GTA.Native;
using GTAV_purge_mod.Ability;

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

        private int _activeTicks = 0;
        private int _inactiveTicks = 0;

        public void Update(int tick) {

            // Update with the current tick and whether it's active.
            OnUpdate(tick, IsActive);

            if (Ped != null && Ped.IsAlive) {

                IsActive = true;
                _inactiveTicks = 0;

                // Update the member with a new active update.
                OnActiveUpdate(_activeTicks, tick);

                // Update the active lifespan.
                _activeTicks++;

            } else {

                IsActive = false;
                _activeTicks = 0;

                // Update the member with a new inactive update.
                OnInactiveUpdate(_inactiveTicks, tick);

                // Update the inactive lifespan.
                _inactiveTicks++;

            }

        }

        private static void OnUpdate(int gameTick, bool alive) {
            
        }
            
        private void OnActiveUpdate(int activeTick, int gameTick) {

            // First tick of the active update.
            if (activeTick == 0)
                Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, Ped, false);

            if (_blip == null && !Ped.IsPlayer) {
                _blip = Ped.AddBlip();
                _blip.Color = BlipColor.Green;
            }

        }

        private void OnInactiveUpdate(int inactiveTick, int gameTick) {

            if (_blip != null) {
                _blip.Remove();
            }

        }

        /// <summary>
        /// Returns for how many ticks this member has been active this active session.
        /// 0 represents the first tick of the active session.
        /// </summary>
        public int ActiveTicks { get { return _activeTicks; } }

        /// <summary>
        /// Returns for how many ticks this member has been inactive inactive session.
        /// 0 represents the first tick of the inactive session.
        /// </summary>
        public int InactiveTicks { get { return _inactiveTicks; }}

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
                if (IsActive) {
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
                if (IsActive) {
                    Ped.Armor = value;
                    return;
                }
                _armor = value;
            }
        }

        public int Money {
            get {
                return (IsActive ? Ped.Money : _money);
            }
            set {
                if (IsActive) {
                    Ped.Money = value;
                    return;
                }
                _money = value;
            }
        }

        public float Heading {
            get {
                return (IsActive ? Ped.Heading : _heading);
            }
            set {
                _heading = value;
            }
        }

        public int Health {
            get {
                return (IsActive ? Ped.Health : _health);
            }
            set {
                if (IsActive) {
                    Ped.Health = value;
                    return;
                }
                _health = value;
            }
        }

        public int MaxHealth {
            get {
                return (IsActive ? Ped.MaxHealth : _maxHealth);
            }
            set {
                if (IsActive) {
                    Ped.MaxHealth = value;
                    return;
                }
                _maxHealth = value;
            }
        }

        public bool IsActive {
            get;
            private set;
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

            if (Ped != null && IsActive) {

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

            TeamMember[] result = new TeamMember[] { };
            var location = Ped.Position;

            foreach (var member in Team.Members) {

                if (Ped.IsNearEntity(member.Ped, new Vector3(dist, dist, dist)))
                    result[result.Length] = member;

            }

            return result;

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