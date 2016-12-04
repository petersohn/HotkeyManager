/*
Copyright (C) 2016  Péter Szabados

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using UnityEngine;

namespace HotkeyManager
{
	public class HotkeyAction : IConfigNode {
		public delegate void Action();

		public HotkeyAction(Action action, bool edgeTrigger = true, KeyBinding keyBinding = null)
		{
			this.keyBinding = keyBinding;
			this.edgeTrigger = edgeTrigger;
			this.action = action;
		}

		public KeyBinding KeyBinding
		{
			get { return keyBinding; }
			set { keyBinding = value; }
		}

		internal void Fire() {
			bool state = keyBinding.GetKey ();
			if (state && (!edgeTrigger || !lastState)) {
				action ();
			}
			lastState = state;
		}

		public void Load(ConfigNode node) {
			if (keyBinding == null) {
				keyBinding = new KeyBinding ();
			}
			keyBinding.Load (node);
		}

		public void Save(ConfigNode node) {
			keyBinding.Save (node);
		}

		private KeyBinding keyBinding;
		private bool edgeTrigger;
		private bool lastState = false;
		private Action action;
	}
}

