using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTAV_purge_mod.Team;
using GTAV_purge_mod.Teams;
using Menu = GTA.Menu;
using MenuItem = GTA.MenuItem;

namespace GTAV_purge_mod {

    public class Main : Script {

        private static TeamHampurgers _teamHampurgers;
        
        public static Team.Team TeamNoPurgeIntended;
        public static Team.Team TeamPurgeCops;

        public static UIText DebugText;
        public static Viewport MainViewport;

        public static Player Player;
        public static readonly List<Team.Team> Teams = new List<Team.Team>();

        public Main() {

            Ticks = 0;

            Tick += OnTick;
            KeyDown += OnKeyDown;

        }

        private static void OnKeyDown(object sender, KeyEventArgs keyEventArgs) {
            Menus.ProcessKey(keyEventArgs);
        }

        private int Ticks { get; set; }

        private void OnTick(object sender, EventArgs e) {

            // First tick.
            if (Ticks == 0) {

                MainViewport = View;
                DebugText = new UIText("DEBUG!", new Point(100, 100), 1f);

                Player = Game.Player;

                _teamHampurgers = new TeamHampurgers();
                Teams.Add(_teamHampurgers);

            }

            foreach (var team in Teams) {
                team.OnTick();
            }

            DebugText.Draw();
            Ticks++;

        }

    }

}