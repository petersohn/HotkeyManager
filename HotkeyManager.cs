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
			Debug.Log (Constants.logPrefix + "Removing hotkey: " + group + "." + name);
			int groupIndex = hotkeyActions.FindIndex (element => element.Key == group);
			if (groupIndex == -1) {
				Debug.Log (Constants.logPrefix + "Group not found.");
				return;
			}
			int index = hotkeyActions [groupIndex].Value.FindIndex (element => element.Key == name);
			if (index == -1) {
				Debug.Log (Constants.logPrefix + "Group not hotkey.");
				return;
			}
			hotkeyActions[groupIndex].Value.RemoveAt (index);
		}

		public void RemoveGroup(String group)
		{
			Debug.Log (Constants.logPrefix + "Removing hotkey group: " + group);
			int index = hotkeyActions.FindIndex (element => element.Key == group);
			if (index == -1) {
				Debug.Log (Constants.logPrefix + "Group not found.");
				return;
			}
			hotkeyActions.RemoveAt (index);
		}

		public IEnumerator<KeyValuePair<String, List<KeyValuePair<String, HotkeyAction>>>> GetEnumerator()
		{
			return hotkeyActions.GetEnumerator ();
		}

		public struct GroupList
		{
			internal GroupList(List<KeyValuePair<String, HotkeyAction>> list)
			{
				this.list = list;
			}

			public IEnumerator<KeyValuePair<String, HotkeyAction>> GetEnumerator()
			{
				if (list == null) {
					return null;
				}
				return list.GetEnumerator();
			}

			private List<KeyValuePair<String, HotkeyAction>> list;
		}

		public GroupList GetGroup(String group)
		{
			int index = hotkeyActions.FindIndex (element => element.Key == group);
			if (index == -1) {
				return new GroupList(null);
			}
			return new GroupList(hotkeyActions[index].Value);
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

