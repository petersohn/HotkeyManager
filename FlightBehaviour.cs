using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotkeyManager
{
	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class FlightBehaviour : MonoBehaviour
	{
		public void Update ()
		{
			HotkeyManager.MainManager.Fire ();
		}
	}
}