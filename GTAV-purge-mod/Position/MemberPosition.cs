﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTAV_purge_mod.Team;

namespace GTAV_purge_mod.Position {

    public abstract class MemberPosition {

        private readonly TeamMember _member;

        private readonly Action<object[]>[] _actions;
        private readonly Action _spawnAction;

        protected MemberPosition(TeamMember member, Action spawn, params Action<object[]>[] action) {
            _member = member;
            _actions = action;
            _spawnAction = spawn;
        }

        public TeamMember Member {
            get {
                return _member;
            }
        }

        public Action SpawnAction {
            get {
                return _spawnAction;
            }
        }

        public Action<object[]>[] Action {
            get {
                return _actions;
            }
        }

        public void OnSpawn() {
            _spawnAction.Invoke();
        }

        public abstract void OnOpenMenu(Viewport view);

        public void Use(int index, object[] args) {
            _actions[index].Invoke(args);
        }

    }

}
