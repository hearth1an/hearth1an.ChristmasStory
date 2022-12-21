using ChrismasStory.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristmasStory.Components
{
	internal class DebugCommands : MonoBehaviour
	{
		public void Update()
		{
			if (Keyboard.current[Key.P].isPressed)
			{
				if (Keyboard.current[Key.Numpad1].wasReleasedThisFrame)
				{
					ShipHandler.BlowUpShip();
				}
			}

		}
	}
}
