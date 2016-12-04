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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;

namespace HotkeyManager
{
	[KSPAddon (KSPAddon.Startup.SpaceCentre, false)]
	public class SpaceCentreBehaviour : MonoBehaviour
	{
		public SpaceCentreBehaviour ()
		{
		}

		public void Awake()
		{
		}

		public void Start()
		{
			Load ();
			if (toolbarButton == null) {
				toolbarButton = ApplicationLauncher.Instance.AddModApplication (
					OnToolbarOn, OnToolbarOff, null, null, null, null,
					ApplicationLauncher.AppScenes.SPACECENTER,
					GameDatabase.Instance.GetTexture ("HotkeyManager/toolbar", false));
			}
		}

		public void OnDestroy()
		{
			ApplicationLauncher.Instance.RemoveModApplication (toolbarButton);
			Save ();
		}

		public void OnGUI()
		{
			if (settingsWindowVisible) {
				settingsWindow.Draw ();
			}
		}

		private void OnToolbarOn()
		{
			Debug.Log (Constants.logPrefix + "Toolbar switched on.");
			settingsWindowVisible = true;
		}

		private void OnToolbarOff()
		{
			Debug.Log (Constants.logPrefix + "Toolbar switched off.");
			Save ();
			settingsWindowVisible = false;
		}

		private void Load()
		{
			Vector2 settingsWindowPosition = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
			Persistence.Load (HotkeyManager.MainManager, ref settingsWindowPosition);
			settingsWindow = new SettingsWindow (HotkeyManager.MainManager);
			settingsWindow.WindowPosition = settingsWindowPosition;
		}

		private void Save()
		{
			Persistence.Save (HotkeyManager.MainManager, settingsWindow.WindowPosition);
		}

		private bool settingsWindowVisible = false;
		private ApplicationLauncherButton toolbarButton;
		private SettingsWindow settingsWindow;


	}
}

