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

        public static TeamHampurgers TeamHampurgers;
        public static Team.Team TeamNoPurgeIntended;
        public static Team.Team TeamPurgeCops;

        private readonly UIText _debugText;
        private readonly Player _player;
        private readonly List<Team.Team> _teams = new List<Team.Team>();

        public Main() {
            Ticks = 0;

            _player = Game.Player;
            _debugText = new UIText("DEBUG!", new Point(500, 500), 0.35f, Color.Black, 4, false);

            TeamHampurgers = new TeamHampurgers();

            Tick += Main_Tick;
            KeyDown += OnKeyDown;
        }

        public int Ticks { get; private set; }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs) {
            if (keyEventArgs.KeyCode == Keys.F3) {
                View.AddMenu(new Menu("The Purge", new MenuItem[] {
                    new MenuButton("Teams", "The teams participating on\nthis years purge", OpenTeamsMenu)
                }));
            }
        }

        private void OpenTeamsMenu() {
            var buttons = new MenuItem[_teams.Count];

            for (var i = 0; i < _teams.Count; i++) {
                var e = _teams[i];
                MenuButton btn;

                Action teamAction = () => { OpenTeamMenu(e); };

                btn = new MenuButton(e.Name, teamAction);
                buttons[i] = btn;
            }

            var menu = new Menu("Teams", buttons);
            menu.HasFooter = false;

            View.AddMenu(menu);
        }

        private void OpenTeamMenu(Team.Team t) {
            var menu = new Menu(t.Name, new MenuItem[] {
                new MenuButton("Members (" + t.Members.Length + ")", OpenTeamMembers),
                new MenuButton("Vehicles (" + t.Vehicles.Length + ")", OpenTeamVehicles),
                new MenuButton("Weapons", OpenTeamWeapons)
            });

            View.AddMenu(menu);
        }

        private void OpenTeamMembers() {}

        private void OpenTeamWeapons() {}

        private void OpenTeamVehicles() {}

        private void Main_Tick(object sender, EventArgs e) {

            var playerPed = _player.Character;
            var pos = playerPed.Position;

            for (var i = 0; i < _teams.Count; i++) {
                _teams[i].DoTick(Ticks);
            }

            if (Ticks == 1) {
                // var member = TeamHampurgers.SpawnMember(TeamMember.TeamMemberPosition.Gunman, pos, true);
            }

            _debugText.Draw();
            Ticks++;

        }
    }
}