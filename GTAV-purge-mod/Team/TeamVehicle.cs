using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAV_purge_mod.Team {

    public class TeamVehicle : Updater {

        private readonly List<TeamMember> _membersInVehicle = new List<TeamMember>();
        private readonly Dictionary<VehicleMod, int> _mods = new Dictionary<VehicleMod, int>();
        private readonly VehicleHash _vehicleHash;

        private VehicleColor _primaryColor = VehicleColor.MetallicWhite;
        private VehicleColor _secondaryColor = VehicleColor.MetallicWhite;
        private VehicleWindowTint _windowTint = VehicleWindowTint.Stock;

        private Blip _blip;
        private Team _team;

        public TeamVehicle(VehicleHash modelHash) {
            _vehicleHash = modelHash;
        }

        protected override void OnUpdate(int tick) {
            IsActive = Vehicle != null && !Vehicle.IsDead;
        }

        protected override void OnFirstUpdate() {}

        protected override void OnActiveUpdate(int activeTick, int tick) {}

        protected override void OnInactiveUpdate(int activeTick, int tick) {
            if (_blip != null) {
                _blip.Remove();
            }
        }

        protected override void OnFirstActiveUpdate(int tick) {
            if (_blip == null) {
                _blip = Vehicle.AddBlip();
                _blip.Color = BlipColor.Blue;
            }
        }

        protected override void OnFirstInactiveUpdate(int tick) {}

        #region "Properties"

        public List<TeamMember> MembersInVehicle {
            get {
                return _membersInVehicle;
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

        public VehicleHash ModelHash {
            get {
                return _vehicleHash;
            }
        }

        public VehicleWindowTint WindowTint {
            get {
                return _windowTint;
            }
            set {
                _windowTint = value;
            }
        }

        public VehicleColor PrimaryColor {
            get {
                return _primaryColor;
            }
            set {
                _primaryColor = value;
            }
        }

        public VehicleColor SecondaryColor {
            get {
                return _secondaryColor;
            }
            set {
                _secondaryColor = value;
            }
        }

        public Dictionary<VehicleMod, int> Mods {
            get {
                return _mods;
            }
        }

        public Vehicle Vehicle {
            get;
            private set;
        }

        public bool HasWeapons() {
            return Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_WEAPONS, Vehicle);
        }

        #endregion

        #region "Methods"

        public TeamVehicle Create(Vector3 pos, bool mods) {

            var vehicle = World.CreateVehicle(_vehicleHash, pos);

            Vehicle = vehicle;
            IsActive = true;

            if (mods) {
                EnableMods();
            }

            return this;

        }

        private void EnableMods() {

            Function.Call(Hash.SET_VEHICLE_MOD_KIT, Vehicle.Handle, 0);

            Vehicle.PrimaryColor = _primaryColor;
            Vehicle.SecondaryColor = _secondaryColor;
            Vehicle.WindowTint = _windowTint;

            for (var i = 0; i < _mods.Count; i++) {

                var mod = _mods.Keys.ToList()[i];
                var value = _mods[mod];

                Vehicle.SetMod(mod, value, true);

            }

        }

        private void UpdateMembers() {

            if (Team != null && Vehicle != null) {

                _membersInVehicle.Clear();

                for (var i = 0; i < Vehicle.PassengerSeats; i++) {

                    // Gets the enum with the specified value.
                    var e = Vehicle.GetPedOnSeat((VehicleSeat)Enum.Parse(typeof(VehicleSeat), i.ToString()));

                    // Checks if the ped is a team member.
                    if (Team.IsTeamMember(e)) {

                        _membersInVehicle.Add(Team.ToTeamMember(e));

                    }

                }

            }

        }

        public TeamVehicle AddMember(TeamMember member, VehicleSeat seat) {

            if (Team != null && Vehicle != null) {

                if (Vehicle.GetPedOnSeat(seat) == null) {
                    member.Ped.Task.WarpIntoVehicle(Vehicle, seat);
                } else {
                    // Set has already been taken.
                    return this;
                }

            } else {
                // There is no vehicle.
                return this;
            }

            return this;

        }

        public TeamVehicle AddMember(TeamMember member) {
            return AddMember(member, member.PreferredSeat);
        }

        public TeamVehicle AddMember(TeamMember.TeamMemberPosition position, VehicleSeat seat) {

            if (Team != null && Vehicle != null) {

                var member = Team.MembersOfPosition(position, true);
                TeamMember result = null;

                if (Vehicle.GetPedOnSeat(seat) != null) {
                    // There's already a ped.
                    return this;
                }

                foreach (var e in member) {

                    if (!e.Ped.IsInVehicle(Vehicle) && e.Position == position) {
                        result = e;
                    }

                }

                if (result == null) {
                    // The result is null.
                    return this;
                }

                result.Ped.Task.WarpIntoVehicle(Vehicle, seat);
                UpdateMembers();

            }

            return this;

        }

        public TeamVehicle AddMember(TeamMember.TeamMemberPosition position) {

            if (Team != null && Vehicle != null) {

                var member = Team.MembersOfPosition(position, true)[0];
                member.Ped.Task.WarpIntoVehicle(Vehicle, member.PreferredSeat);
                UpdateMembers();

            }

            return this;
        }

        private TeamVehicle AddMod(VehicleMod mod, int index) {

            if (Vehicle != null) {

                Vehicle.SetMod(mod, index, true);

            }

            return this;

        }

        public TeamVehicle AddAllModsMax() {

            var modStrings = Enum.GetNames(typeof(VehicleMod));

            foreach (string s in modStrings) {

                var mod = (VehicleMod)Enum.Parse(typeof(VehicleMod), s);
                AddModMax(mod);

            }

            return this;

        }

        private TeamVehicle AddModMax(VehicleMod mod) {

            var max = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, Vehicle, (int)mod);

            if (max != 0) {

                return AddMod(mod, max - 1);

            }

            return this;

        }

        #endregion

    }
}