using System.Collections.Generic;
using GTA;
using GTA.Math;

namespace GTAV_purge_mod.Team {
    public class Team {

        private readonly string _name;

        protected Team(string name, TeamVehicle[] vehicles, TeamMember[] members) {

            _name = name;

            Vehicles = vehicles;
            Members = members;

            Group = World.AddRelationShipGroup("group." + name.ToLower());

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

        #endregion

        #region "Functions"

        public void DoTick(int tick) {
            foreach (var member in Members) {
                member.Update(tick);
            }
        }

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
            vehicle.OwnerTeam = this;

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

        public Team SpawnMembers(Vector3 vect, bool changes) {
            for (var i = 0; i < Members.Length; i++) {
                SpawnMember(i, vect, changes);
            }
            return this;
        }

        #endregion

        #region "Utilities"

        public TeamMember ToTeamMember(Ped ped) {
            if (IsTeamMember(ped)) {
                for (var i = 0; i < Members.Length; i++) {
                    var e = Members[i];
                    if (e.Ped.Handle == ped.Handle) {
                        return e;
                    }
                }
            }
            return null;
        }

        public bool IsTeamMember(Ped ped) {
            for (var i = 0; i < Members.Length; i++) {
                var e = Members[i];
                if (e.Ped.Handle == ped.Handle) {
                    return true;
                }
            }
            return false;
        }

        public List<TeamMember> MembersOfPosition(TeamMember.TeamMemberPosition position, bool active) {

            var result = new List<TeamMember>();

            for (var i = 0; i < Members.Length; i++) {

                var e = Members[i];

                if (e.Position == position && e.IsActive == active) {

                    

                    result.Add(e);

                }

            }

            return result;

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

                // MemberPosition memberPos = null;

                // Add a new element to the list, depending on the specified position.
                //switch (position) {

                //    case TeamMember.TeamMemberPosition.Medic:

                //        memberPos = new PositionMedic(member);
                //        // positions.Add(memberPos);

                //        break;

                //}

                //if (memberPos != null)
                //    memberPos.OnSpawn();

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
        ///     Returns how many members that are available of the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int MemberCount(TeamMember.TeamMemberPosition position) {
            var result = 0;
            for (var i = 0; i < Members.Length; i++) {
                var e = Members[i];
                if (e.Position == position) {
                    result++;
                }
            }
            return result;
        }


        /// <summary>
        ///     Returns how many members that are active of the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        private int ActiveOf(TeamMember.TeamMemberPosition position) {
            var result = 0;
            foreach (TeamMember e in Members) {
                if (e.Position == position && e.IsActive) {
                    result++;
                }
            }
            return result;
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
            for (var i = 0; i < Members.Length; i++) {
                var e = Members[i];
                if (e.Position == position && e.IsActive) {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}