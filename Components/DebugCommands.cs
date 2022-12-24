using ChrismasStory.Components;
using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChristmasStory.Components
{
	internal class DebugCommands : MonoBehaviour
	{
		private readonly List<(Key key, Action action)> _debugCommands = new();
		private readonly List<(Key key, Action action)> _debugWarpCommands = new();

		public void Start()
		{
			RegisterDebugCommand(Key.Numpad1, ShipHandler.BlowUpShip);
			RegisterDebugCommand(Key.Numpad2, HeldItemHandler.GivePlayerWarpCore);
			RegisterDebugCommand(Key.Numpad3, HeldItemHandler.GivePlayerLantern);

			RegisterDebugWarpCommand(Key.Numpad1, () => WarpToPlanet(AstroObject.Name.TimberMoon, 100f)); // Esker
			RegisterDebugWarpCommand(Key.Numpad2, () => WarpToPlanet(AstroObject.Name.CaveTwin, 180)); // Chert
			RegisterDebugWarpCommand(Key.Numpad3, () => WarpToPlanet(AstroObject.Name.BrittleHollow, 350f)); // Riebeck
			RegisterDebugWarpCommand(Key.Numpad4, WarpToGabbroIsland);
			RegisterDebugWarpCommand(Key.Numpad5, WarpToFeldspar);
		}

		public void Update()
		{
			if (Keyboard.current[Key.P].isPressed)
			{
				foreach (var (key, action) in _debugCommands)
				{
					if (Keyboard.current[key].wasReleasedThisFrame)
					{
						action.Invoke();
					}
				}
			}

			if (Keyboard.current[Key.O].isPressed)
			{
				foreach (var (key, action) in _debugWarpCommands)
				{
					if (Keyboard.current[key].wasReleasedThisFrame)
					{
						action.Invoke();
					}
				}
			}
		}

		private void RegisterDebugCommand(Key key, Action action) => _debugCommands.Add((key, action));
		private void RegisterDebugWarpCommand(Key key, Action action) => _debugWarpCommands.Add((key, action));

		private static void WarpToPlanet(AstroObject.Name planetName, float offset)
		{
			var planet = Locator.GetAstroObject(planetName);

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = planet.transform.position + Vector3.up * offset;

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}

		private static void WarpToGabbroIsland()
		{
			var island = SearchUtilities.Find("GabbroIsland_Body");

			var planet = Locator.GetAstroObject(AstroObject.Name.GiantsDeep);

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = island.transform.TransformPoint(Vector3.up * 30f);

			body.WarpToPositionRotation(newWorldPos, island.transform.rotation);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}

		private static void WarpToFeldspar()
		{
			var dimension = SearchUtilities.Find("DB_AnglerNestDimension_Body");

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = dimension.transform.TransformPoint(new Vector3(10, 10, 25));

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(dimension.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));

			GlobalMessenger.FireEvent("PlayerFogWarp");
		}
	}
}
