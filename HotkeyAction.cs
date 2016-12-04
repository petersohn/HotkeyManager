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

