using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAV_purge_mod {

    public class Team {

        public class Member {

            public enum MemberType {
                Member = 0,
                Leader = 1,
                Boss = 2
            }

            private Ped ped;

            private MemberType type;

            public Member(Ped ped) {
                this.ped = ped;
            }

            public Ped Ped {
                get { return ped; }
                set { ped = value; }
            }

            public MemberType Type {
                get { return type; }
                set { type = value; }
            }

        }
   
        private string name;

        private GTA.Native.VehicleHash[] vehicleModels = null;

        private Dictionary<VehicleMod, int> mods = 
            new Dictionary<VehicleMod,int>();

        private VehicleColor primaryColor = VehicleColor.MetallicWhite;
        private VehicleColor secondaryColor = VehicleColor.MetallicWhite;

        private List<Member> _members = new List<Member>();
        private int group = 0;
        private Member boss = null;

        public Team(string name, params GTA.Native.VehicleHash[] vmodels) {

            this.name = name;
            this.vehicleModels = vmodels;

            group = World.AddRelationShipGroup(name);
            ResetMods();

        }

        /// <summary>
        /// Returns the name of this team.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Sets all the mods in the dictionary to 0.
        /// </summary>
        public void ResetMods() {
            string[] names = Enum.GetNames(typeof(VehicleMod));

            for (int i = 0; i < names.Length; i++) {

                string e = names[i];
                VehicleMod mod = (VehicleMod)Enum.Parse(typeof(VehicleMod), e);

                mods.Add(mod, 0);

            }
        }

        /// <summary>
        /// Spawns a random vehicle on the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <param name="z">The Z-coordinate.</param>
        /// <returns></returns>
        public Vehicle SpawnRandom(int x, int y, int z) {
            Random rand = new Random(DateTime.Today.Second);
            return Spawn(rand.Next(vehicleModels.Length) - 1, x, y, z);
        }

        /// <summary>
        /// Spawns a vehicle on the specified index at the specified coordinates.
        /// </summary>
        /// <param name="index">The index of the vehicle to spawn.</param>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <param name="z">The Z-coordinate.</param>
        /// <returns></returns>
        public Vehicle Spawn(int index, int x, int y, int z) {

            Vehicle veh = World.CreateVehicle(vehicleModels[index], new GTA.Math.Vector3(x, y, z));

            for (int i = 0; i < mods.Count; i++) {

                VehicleMod mod = mods.Keys.ToList<VehicleMod>()[i];
                int value = mods.Values.ToList<int>()[i];

                veh.SetMod(mod, value, true);

            }

            veh.PrimaryColor = primaryColor;
            veh.SecondaryColor = secondaryColor;

            return veh;

        }

        /// <summary>
        /// Spawns a vehicle on the specified index at the specified Vector3.
        /// </summary>
        /// <param name="index">The index of the vehicle to spawn.</param>
        /// <param name="vect">The Vector3 where to spawn the vehicle.</param>
        /// <returns></returns>
        public Vehicle Spawn(int index, GTA.Math.Vector3 vect) {
            return Spawn(index, (int) vect.X, (int) vect.Y, (int) vect.Z);
        }

        /// <summary>
        /// Returns the default configuration of this team vehicle.
        /// </summary>
        public Dictionary<VehicleMod, int> VehicleMods {
            get { return mods; }
            set { mods = value; }
        }

        /// <summary>
        /// Returns the vehicle models that are part of this team.
        /// </summary>
        public GTA.Native.VehicleHash[] VehicleModels {
            get { return vehicleModels; }
            set { vehicleModels = value; }
        }

        /// <summary>
        /// Returns the group handle of this team.
        /// </summary>
        public int Group {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// Returns the leader of this team.
        /// </summary>
        public Ped Leader {
            get;
            set;
        }

        /// <summary>
        /// Returns the members of this team.
        /// </summary>
        public List<Member> Members {
            get { return _members; }
        }

        public Team SetRelationshipTo(Team t1, Relationship r1) {
            World.SetRelationshipBetweenGroups(r1, t1.group, group);
            return this;
        }

        public Relationship GetRelationshipTo(Team t1) {
            return World.GetRelationshipBetweenGroups(group, t1.Group);
        }

        public Member Boss {
            get { return boss; }
            set { boss = value; boss.Type = Member.MemberType.Boss }
        }

        public Team AddMember(Ped par1) {
            return AddMember(new Member(par1));
        }

        public Team AddMember(Member par1) {
            if (!HasMember(par1.Ped)) {
                par1.Ped.RelationshipGroup = group;
                par1.Type = Member.MemberType.Member;
                _members.Add(par1);
            }
            return this;
        }

        public Member GetMember(Ped par1) {
            for (int i = 0; i < _members.Count; i++) {
                Member e = _members[i];
                if (e.Ped.Handle == par1.Handle)
                    return e;
            }
            return null;
        }

        public bool HasMember(Ped par1) {
            return GetMember(par1) != null;
        } 
        
    }

}
