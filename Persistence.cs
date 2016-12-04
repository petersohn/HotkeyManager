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

