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

        private readonly Player _player;
        private readonly List<Team.Team> _teams = new List<Team.Team>();

        public Main() {

            Ticks = 0;

            _player = Game.Player;
            DebugText = new UIText("DEBUG!", new Point(500, 500), 0.35f, Color.Black, 4, false);

            _teamHampurgers = new TeamHampurgers();
            _teams.Add(_teamHampurgers);

            Tick += Main_Tick;

        }

        private int Ticks { get; set; }

        private void Main_Tick(object sender, EventArgs e) {

            var playerPed = _player.Character;
            var pos = playerPed.Position;

            foreach (var team in _teams) {
                team.DoTick(Ticks);
            }

            if (Ticks == 1) {

                var vehicle = _teamHampurgers.SpawnVehicleWithMembers(0, new[] {

                    TeamMember.TeamMemberPosition.Engineer,
                    TeamMember.TeamMemberPosition.Gunman,
                    TeamMember.TeamMemberPosition.Gunman,
                    TeamMember.TeamMemberPosition.Tank

                }, pos, true);

            }

            DebugText.Draw();
            Ticks++;

        }
    }
}