using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HotkeyManager
{
	using HotkeyActions = List<KeyValuePair<String, List<KeyValuePair<String, HotkeyAction>>>>;

	public class HotkeyManager
	{
		static public HotkeyManager MainManager {
			get {
				if (mainInstance == null) {
					mainInstance = new HotkeyManager ();
				}
				return mainInstance;
			}	
		}

		static private HotkeyManager mainInstance;

		public HotkeyManager ()
		{
		}

		public void Fire()
		{
			foreach (var group in hotkeyActions) {
				foreach (var element in group.Value) {
					element.Value.Fire ();
				}
			}
		}

		public void Add(String group, String name, HotkeyAction hotkeyAction)
		{
			int groupIndex = hotkeyActions.FindIndex (element => element.Key == group);
			if (groupIndex == -1) {
				groupIndex = hotkeyActions.Count;
				hotkeyActions.Add(new KeyValuePair<string, List<KeyValuePair<string, HotkeyAction>>>(
					group, new List<KeyValuePair<string, HotkeyAction>>()));
			}
			int index = hotkeyActions [groupIndex].Value.FindIndex (element => element.Key == name);
			if (index == -1) {
				hotkeyActions [groupIndex].Value.Add (new KeyValuePair<string, HotkeyAction> (name, hotkeyAction));
			}
		}

		public void Remove(String group, String name)
		{
			int index = hotkeyActions.FindIndex (element => element.Key == group);
			if (index == -1) {
				return;
			}
			hotkeyActions[index].Value.RemoveAt (
				hotkeyActions[index].Value.FindIndex (element => element.Key == name));
		}

		public void RemoveGroup(String group)
		{
			hotkeyActions.RemoveAt (hotkeyActions.FindIndex (element => element.Key == group));
		}

		public IEnumerator<KeyValuePair<String, List<KeyValuePair<String, HotkeyAction>>>> GetEnumerator()
		{
			return hotkeyActions.GetEnumerator ();
		}

		public int Count { get { return hotkeyActions.Count; } }

		public void Save(ConfigNode node)
		{
			foreach (var group in hotkeyActions) {
				Debug.Log (Constants.logPrefix + "Saving hotkey group: " + group.Key);
				ConfigNode groupNode = new ConfigNode ();
				foreach (var element in group.Value) {
					Debug.Log (Constants.logPrefix + "Saving hotkey: " + element.Key + ": " +
						element.Value.KeyBinding.name);
					ConfigNode keyNode = new ConfigNode ();
					element.Value.Save (keyNode);
					groupNode.AddNode (GetNodeName(element.Key), keyNode);
				}
				node.AddNode (GetNodeName(group.Key), groupNode);
			}
		}

		public void Load(ConfigNode node)
		{
			foreach (var group in hotkeyActions) {
				Debug.Log (Constants.logPrefix + "Loading hotkey group: " + group.Key);
				ConfigNode groupNode = node.GetNode (GetNodeName(group.Key));
				if (groupNode == null) {
					continue;
				}
				foreach (var element in group.Value) {
					Debug.Log (Constants.logPrefix + "Loading hotkey: " + element.Key);
					ConfigNode keyNode = groupNode.GetNode (GetNodeName(element.Key));
					if (keyNode != null) {
						element.Value.Load (keyNode);
					}
					Debug.Log (Constants.logPrefix + "Loaded hotkey: " + element.Value.KeyBinding.name);
				}
			}
		}

		private static String GetNodeName(String name)
		{
			return Regex.Replace (name, "[^a-zA-Z0-9]", _ => "_");
		}

		private HotkeyActions hotkeyActions = new HotkeyActions();
	}
}

