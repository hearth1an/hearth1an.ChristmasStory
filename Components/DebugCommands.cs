using ChrismasStory.Components;
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
				if (Keyboard.current[Key.Numpad2].wasReleasedThisFrame)
				{
					HeldItemHandler.GivePlayerWarpCore();
				}
				if (Keyboard.current[Key.Numpad3].wasReleasedThisFrame)
				{
					HeldItemHandler.GivePlayerLantern();
				}
			}

			if (Keyboard.current[Key.O].isPressed)
			{
				if (Keyboard.current[Key.Numpad1].wasReleasedThisFrame)
				{
					WarpToPlanet(Locator.GetAstroObject(AstroObject.Name.TimberMoon), 100f);
				}
				if (Keyboard.current[Key.Numpad2].wasReleasedThisFrame)
				{
					WarpToPlanet(Locator.GetAstroObject(AstroObject.Name.CaveTwin), 180);
				}
				if (Keyboard.current[Key.Numpad3].wasReleasedThisFrame)
				{
					WarpToPlanet(Locator.GetAstroObject(AstroObject.Name.BrittleHollow), 300f);
				}
			}
		}

		private static void WarpToPlanet(AstroObject planet, float offset = 100f)
		{
			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = planet.transform.position + Vector3.up * offset;

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}
	}
}
