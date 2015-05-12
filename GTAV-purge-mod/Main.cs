using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAV_purge_mod
{

    public class Main : Script {

        private int ticks = 0;

        public Main() {
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
