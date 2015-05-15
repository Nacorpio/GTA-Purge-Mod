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

        public static Viewport MainViewport;
        public static UIText DebugText;

        public static Player Player;
        public static readonly List<Team.Team> Teams = new List<Team.Team>();

        public Main() {

            Ticks = 0;

            Player = Game.Player;

            MainViewport = View;
            DebugText = new UIText("DEBUG!", new Point(500, 500), 0.35f, Color.Black, 4, false);

            _teamHampurgers = new TeamHampurgers();
            Teams.Add(_teamHampurgers);

            Tick += OnTick;
            KeyDown += OnKeyDown;

        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs) {
            Menus.ProcessKey(keyEventArgs);
        }

        private int Ticks { get; set; }

        private void OnTick(object sender, EventArgs e) {

            var playerPed = Player.Character;
            var pos = playerPed.Position;

            foreach (var team in Teams) {
                team.DoTick(Ticks);
            }

            if (Ticks == 1) {

            }

            DebugText.Draw();
            Ticks++;

        }

    }

}