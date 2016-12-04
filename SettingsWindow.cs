using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;

namespace HotkeyManager
{
	public class SettingsWindow
	{
		public Vector2 WindowPosition
		{
			get { return windowRect.center; }
			set { windowRect.center = value; }
		}

		public SettingsWindow(HotkeyManager hotkeyManager)
		{
			this.hotkeyManager = hotkeyManager;
		}

		public void Draw()
		{
			CalculateStyles ();
			windowRect = GUILayout.Window(1, windowRect, DrawSettingsWindow, "Hotkey Manager",
				HighLogic.Skin.window);
		}

		private void DrawSettingsWindow(int windowID)
		{
			if (currentAction != null) {
				foreach (KeyBinding keyBinding in AllKeyBindings) {
					if (keyBinding.GetKey ()) {
						currentAction.KeyBinding = keyBinding;
						currentAction = null;
						break;
					}
				}
			}
			scrollPosition = GUILayout.BeginScrollView (scrollPosition, false, true);
			foreach (var group in hotkeyManager) {
				GUILayout.Label (group.Key, groupTitleStyle);
				foreach (var element in group.Value) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (element.Key);
					DrawSelectorButton (element.Value);
					if (GUILayout.Button (clearButtonText, GUILayout.Width(ClearButtonWidth))) {
						element.Value.KeyBinding = new KeyBinding ();
					}
					GUILayout.EndHorizontal ();
				}
			}
			GUILayout.EndScrollView ();
			GUI.DragWindow ();
		}

		private void CalculateStyles()
		{
			if (groupTitleStyle == null) {
				groupTitleStyle = new GUIStyle (GUI.skin.label);
				groupTitleStyle.alignment = TextAnchor.MiddleCenter;
			}
		}

		private void DrawSelectorButton(HotkeyAction hotkeyAction)
		{
			if (currentAction == hotkeyAction) {
				if (GUILayout.Button ("...", GUILayout.Width(HotkeyButtonWidth))) {
					currentAction = null;
				}
			} else {
				if (GUILayout.Button (hotkeyAction.KeyBinding.name, GUILayout.Width(HotkeyButtonWidth))) {
					currentAction = hotkeyAction;
				}
			}
		}

		private void CalculateKeyBindings()
		{
			if (allKeyBindings != null) {
				return;
			}
			var values = Enum.GetValues (typeof(KeyCode)).Cast<KeyCode> ();
			allKeyBindings = new List<KeyBinding> ();
			float maxWidth = 0.0f;
			foreach (KeyCode key in values) {
				String keyName = key.ToString ();
				// JoystickButtonX refers to buttons for any joystick, so filter them out.
				if (!keyName.StartsWith ("JoystickButton") && !keyName.StartsWith("Mouse")) {
					allKeyBindings.Add (new KeyBinding (key));
					maxWidth = Math.Max(maxWidth, GUI.skin.button.CalcSize(new GUIContent(keyName)).x);
				}
			}
			hotkeyButtonWidth = maxWidth + buttonWidthThreshold;
			CalculateWindowSize ();
		}

		private void CalculateWindowSize()
		{
			float labelWidth = 0.0f;
			foreach (var group in hotkeyManager) {
				foreach (var element in group.Value) {
					labelWidth = Math.Max (labelWidth, GUI.skin.label.CalcSize (new GUIContent (element.Key)).x);
				}
			}
			float width = labelWidth + hotkeyButtonWidth + ClearButtonWidth + windowWidthThreshold;
			Debug.Log (Constants.logPrefix + "Old window width: " + windowRect.width + ". New window width: " + width);
			float halfDifference = (width - windowRect.width) / 2;
			windowRect.xMin -= halfDifference;
			windowRect.xMax += halfDifference;
		}

		private List<KeyBinding> AllKeyBindings
		{
			get {
				CalculateKeyBindings ();
				return allKeyBindings;
			}
		}

		private float HotkeyButtonWidth
		{
			get {
				CalculateKeyBindings ();
				return hotkeyButtonWidth;
			}
		}

		private float ClearButtonWidth
		{
			get {
				if (clearButtonWidth == 0.0f) {
					clearButtonWidth = GUI.skin.button.CalcSize (
						new GUIContent (clearButtonText)).x + buttonWidthThreshold;
				}
				return clearButtonWidth;
			}
		}

		private const float buttonWidthThreshold = 10.0f;
		private const float windowWidthThreshold = 80.0f;
		private const String clearButtonText = "Clear";
		private const int defaultWindowWidth = 400;
		private const int defaultWindowHeight = 500;

		private HotkeyManager hotkeyManager;
		private GUIStyle groupTitleStyle;
		private List<KeyBinding> allKeyBindings;
		private float hotkeyButtonWidth = 0.0f;
		private float clearButtonWidth = 0.0f;
		private Vector2 scrollPosition = new Vector2 (0, 0);
		public static Rect windowRect = new Rect (0, 0,	defaultWindowWidth, defaultWindowHeight);
		private HotkeyAction currentAction;
	}
}

