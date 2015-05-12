using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace GTAV_purge_mod
{

    public class Main : Script {

        private Player player;
        private int ticks = 0;

        public Main() {

            player = Game.Player;
            Tick += Main_Tick;

        }

        private void Main_Tick(object sender, EventArgs e) {

            if (ticks == 0) {

                // During the first tick, initializations can be made.
                FirstTick();

            } else if (ticks > 0) {

                tick(ticks);

            }

            ticks++;

        }

        private void tick(int tick) {

            Ped ped = player.Character;
            Team t = new Team("Team1", VehicleHash.Schafter2, VehicleHash.Sultan);

        }

        private void FirstTick() {

        }

        public int Ticks {
            get {
                return ticks;
            }
        }

    }

}
