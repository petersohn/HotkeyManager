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
	internal class Persistence
	{
		static readonly string saveFileName = KSPUtil.ApplicationRootPath + "GameData/HotkeyManager_settings.cfg";

		public static void Load(HotkeyManager hotkeyManager, ref Vector2 settingsWindowPosition)
		{
			Debug.Log (Constants.logPrefix + "Loading from file.");
			ConfigNode configFileNode = ConfigNode.Load (saveFileName);
			if (configFileNode == null) {
				Debug.LogWarning (Constants.logPrefix + "Config file does not exist, using empty config.");
				configFileNode = new ConfigNode ();
			}

			ConfigNode hotkeysNode = GetOrCreateNode (configFileNode, "hotkeys");
			hotkeyManager.Load (hotkeysNode);

			ConfigNode settingsWindowNode = configFileNode.GetNode("settingsWindow");
			if (settingsWindowNode != null) {
				settingsWindowPosition.x = Convert.ToSingle(settingsWindowNode.GetValue ("x"));
				settingsWindowPosition.y = Convert.ToSingle(settingsWindowNode.GetValue ("y"));
			}
		}

		public static void Save(HotkeyManager hotkeyManager, Vector2 settingsWindowPosition)
		{
			Debug.Log (Constants.logPrefix + "Saving to file.");

			ConfigNode baseNode = new ConfigNode();
			ConfigNode hotkeysNode = baseNode.AddNode ("hotkeys");
			hotkeyManager.Save (hotkeysNode);

			ConfigNode settingsWindowNode = baseNode.AddNode ("settingsWindow");
			settingsWindowNode.AddValue ("x", settingsWindowPosition.x);
			settingsWindowNode.AddValue ("y", settingsWindowPosition.y);

			baseNode.Save (saveFileName);
		}

		private static ConfigNode GetOrCreateNode(ConfigNode node, string name)
		{
			ConfigNode result = node.GetNode (name);
			if (result == null) {
				Debug.Log (Constants.logPrefix + "Node not found: " + name);
				result = new ConfigNode ();
				node.AddNode (name, result);
			} else {
				Debug.Log (Constants.logPrefix + "Node found: " + name);
			}
			return result;
		}
	}
}

