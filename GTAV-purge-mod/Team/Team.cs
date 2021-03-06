﻿using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;

namespace GTAV_purge_mod.Team {

    public class Team : Updater {

        private readonly string _name;

        protected Team(string name, TeamVehicle[] vehicles, TeamMember[] members) {

            _name = name;

            Vehicles = vehicles;
            Members = members;

            // Set the TeamMember.Team variable to this team.
            foreach (var member in members) {
                member.Team = this;
            }

            // Set the TeamVehicle.Team variable to this team.
            foreach (var vehicle in vehicles) {
                vehicle.Team = this;
            }

            // Assign a new relationship group to the team.
            Group = World.AddRelationShipGroup("group." + name.ToLower());

        }

        protected override void OnUpdate(int tick) {

            foreach (var member in Members) {
                if (member.IsActive)
                    member.OnTick();
            }

            foreach (var vehicle in Vehicles) {
                vehicle.OnTick();
            }

        }

        public void OnMemberKilled(TeamMember member) {

            UI.Notify(member.Position + " died.");

            if (ActiveMembers().Length == 0) {
                UI.Notify("All members in " + Name + " have been killed.");
            }

        }

        protected override void OnFirstActiveUpdate(int tick) {}

        protected override void OnFirstInactiveUpdate(int tick) {}

        protected override void OnFirstUpdate() {

            foreach (var t in Main.Teams) {

                if (t.Name != Name)
                    World.SetRelationshipBetweenGroups(Relationship.Hate, Group, t.Group);

            }

        }

        protected override void OnActiveUpdate(int activeTick, int tick) {
        }

        protected override void OnInactiveUpdate(int inactiveTick, int tick) {
        }

        #region "Properties"

        /// <summary>
        ///     Returns the name of this team.
        /// </summary>
        public string Name {
            get { return _name; }
        }

        /// <summary>
        ///     Returns the members of this team.
        /// </summary>
        public TeamMember[] Members { get; private set; }

        /// <summary>
        ///     Returns the vehicle models that are part of this team.
        /// </summary>
        public TeamVehicle[] Vehicles { get; private set; }

        /// <summary>
        ///     Returns the group handle of this team.
        /// </summary>
        public int Group { get; private set; }

        public BlipColor Color { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        ///     Spawns a vehicle on the specified index at the specified coordinates.
        /// </summary>
        /// <param name="index">The index of the vehicle to spawn.</param>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <param name="z">The Z-coordinate.</param>
        /// <param name="mods">Whether mods should be applied to this vehicle.</param>
        /// <returns></returns>
        public TeamVehicle SpawnVehicle(int index, float x, float y, float z, bool mods) {
            var vehicle = Vehicles[index];
            vehicle.Team = this;

            if (!vehicle.IsActive) {
                vehicle.Create(new Vector3(x, y, z), mods);
            }

            return vehicle;
        }

        /// <summary>
        ///     Spawns a vehicle on the specified index at the specified Vector3.
        /// </summary>
        /// <param name="index">The index of the vehicle to spawn.</param>
        /// <param name="vect">The Vector3 where to spawn the vehicle.</param>
        /// <param name="mods">Whether mods should be applied to this vehicle.</param>
        /// <returns></returns>
        public TeamVehicle SpawnVehicle(int index, Vector3 vect, bool mods) {
            return SpawnVehicle(index, vect.X, vect.Y, vect.Z, mods);
        }

        /// <summary>
        /// Spawns a vehicle of the specified type with members of the specified positions.
        /// </summary>
        /// <param name="index">The index of the vehicle to spawn.</param>
        /// <param name="positions">The positions to spawn within the vehicle.</param>
        /// <param name="vect">A Vector3 of the position of where to spawn the vehicle.</param>
        /// <param name="mods">Whether to install mods on the newly spawned vehicle.</param>
        /// <returns></returns>
        public TeamVehicle SpawnVehicleWithMembers(int index, TeamMember.TeamMemberPosition[] positions, Vector3 vect, bool mods) {

            TeamVehicle vehicle = SpawnVehicle(index, vect, mods);

            foreach (TeamMember.TeamMemberPosition pos in positions.Where(p => InactiveCountOf(p) >= 1)) {

                TeamMember member = SpawnMember(pos, vect, true);
                vehicle.AddMember(member);
            
            }

            return vehicle;

        }

        /// <summary>
        ///     Spawns a member at the specified index on the specified Vector3.
        /// </summary>
        /// <param name="index">The index of the member to spawn.</param>
        /// <param name="vect">The Vector3 where to spawn the member.</param>
        /// <param name="changes">Whether changes should be applied to this member.</param>
        /// <returns></returns>
        public TeamMember SpawnMember(int index, Vector3 vect, bool changes) {

            var member = Members[index];

            if (!member.IsActive) {
                member.Create(vect, changes);
                member.Ped.RelationshipGroup = Group;
            }

            return member;

        }

        /// <summary>
        /// Spawns all the members in the team at the specified coordinates.
        /// </summary>
        /// <param name="vect">The coordinates of where to spawn the members.</param>
        /// <param name="changes">Whether to update the properties of the members.</param>
        /// <returns></returns>
        public Team SpawnMembers(Vector3 vect, bool changes) {
            for (var i = 0; i < Members.Length; i++) {
                SpawnMember(i, vect, changes);
            }
            return this;
        }

        #endregion

        #region "Utilities"

        /// <summary>
        /// Converts the specified ped to a TeamMember if the ped is a member of this team.
        /// </summary>
        /// <param name="ped">The ped to convert to a TeamMember.</param>
        /// <returns></returns>
        public TeamMember ToTeamMember(Ped ped) {
            return Members.Where(m => m.Ped.Handle == ped.Handle).ToList()[0];
        }

        /// <summary>
        /// Returns whether the specified ped is a member of this team.
        /// </summary>
        /// <param name="ped">The ped to check.</param>
        /// <returns></returns>
        public bool IsTeamMember(Ped ped) {
            return Members.Any(m => m.Ped != null && m.Ped.Handle == ped.Handle);
        }

        /// <summary>
        /// Returns a list of TeamMembers of the specified TeamMemberPosition.
        /// </summary>
        /// <param name="position">The position to sort out.</param>
        /// <param name="active">Whether the sorting is for active or inactive members.</param>
        /// <returns></returns>
        public List<TeamMember> MembersOfPosition(TeamMember.TeamMemberPosition position, bool active) {
            return Members.Where(m => m.Position == position && m.IsActive == active).ToList();
        }

        /// <summary>
        ///     Spawns a member of the specified position on the specified Vector3.
        /// </summary>
        /// <param name="position">The position of the new member.</param>
        /// <param name="vect">The Vector3 coordinates of the new member.</param>
        /// <param name="changes">Whether changes should be applied to this member.</param>
        /// <returns></returns>
        public TeamMember SpawnMember(TeamMember.TeamMemberPosition position, Vector3 vect, bool changes) {
            if (InactiveCountOf(position) >= 1) {
                var member = SpawnMember(InactiveOf(position), vect, changes);
                return member;
            }
            return null;
        }

        /// <summary>
        ///     Returns the index of an inactive member of the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int InactiveOf(TeamMember.TeamMemberPosition position) {
            for (var i = 0; i < Members.Length; i++) {
                var e = Members[i];
                if (e.Position == position && !e.IsActive) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Returns how many members that are available of the specified position (both active and inactive).
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int MemberCount(TeamMember.TeamMemberPosition position) {
            return Members.Where(m => m.Position == position).ToList().Count;
        }

        /// <summary>
        ///     Returns how many members that are active of the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int ActiveOf(TeamMember.TeamMemberPosition position) {
            return Members.Where(m => m.Position == position && m.IsActive).ToList().Count;
        }

        /// <summary>
        /// Returns all the active members of this team.
        /// </summary>
        /// <returns></returns>
        public TeamMember[] ActiveMembers() {
            return Members.Where(m => m.IsActive).ToArray();
        }

        /// <summary>
        ///     Returns how many members that are inactive of the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int InactiveCountOf(TeamMember.TeamMemberPosition position) {
            return MemberCount(position) - ActiveOf(position);
        }

        /// <summary>
        ///     Returns whether there is a member of the specified position alive.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        public bool IsPositionAlive(TeamMember.TeamMemberPosition position) {
            return Members.Where(m => m.Position == position).ToList().Count >= 1;
        }

        #endregion

    }
}