using NewHorizons.Utility;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ChristmasStory.Utility
{
	internal static class WarpUtils
	{
		public static void WarpToSixthLocation()
		{
			var quantumMoon = GameObject.FindObjectOfType<QuantumMoon>();

			WarpToPlanet(AstroObject.Name.QuantumMoon, new Vector3(-6.2f, -68.2f, 9.75f));

			quantumMoon.SetSurfaceState(5);
		}

		public static void WarpToPlanet(AstroObject.Name planetName, Vector3 position)
		{
			var planet = Locator.GetAstroObject(planetName);

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = planet.transform.TransformPoint(position);

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}

		public static void WarpToPlanet(AstroObject.Name planetName, float offset)
		{
			var planet = Locator.GetAstroObject(planetName);

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = planet.transform.position + Vector3.up * offset;

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}

		public static void WarpToGabbroIsland()
		{
			var island = SearchUtilities.Find("GabbroIsland_Body");

			var planet = Locator.GetAstroObject(AstroObject.Name.GiantsDeep);

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = island.transform.TransformPoint(Vector3.up * 30f);

			body.WarpToPositionRotation(newWorldPos, island.transform.rotation);
			body.SetVelocity(planet.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));
		}

		public static void WarpToFeldspar()
		{
			var dimension = SearchUtilities.Find("DB_AnglerNestDimension_Body");

			var body = PlayerState.AtFlightConsole() ? Locator.GetShipBody() : Locator.GetPlayerBody();

			var newWorldPos = dimension.transform.TransformPoint(new Vector3(10, 10, 25));

			body.WarpToPositionRotation(newWorldPos, Quaternion.identity);
			body.SetVelocity(dimension.GetAttachedOWRigidbody().GetPointVelocity(newWorldPos));

			GlobalMessenger.FireEvent("PlayerFogWarp");
		}

		public static void WarpToLetranger()
		{
			var ringworldSector = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior").GetComponent<Sector>();

			WarpToPlanet(AstroObject.Name.RingWorld, PlayerState.AtFlightConsole() ? Vector3.zero : new Vector3(194f, -80f, -137f));
			Locator.GetPlayerSectorDetector().RemoveFromAllSectors();
			ringworldSector.GetTriggerVolume().AddObjectToVolume(Locator.GetPlayerDetector());
			Locator.GetRingWorldController()._insideRingWorldVolume.AddObjectToVolume(Locator.GetPlayerDetector());
		}

		public static void WarpToDreamTown() => ChristmasStory.Instance.StartCoroutine(WarpToDreamTown_Routine());

		private static IEnumerator WarpToDreamTown_Routine()
		{
			// Stolen from QSB who stole it from DayDream
			var relativeLocation = new RelativeLocationData(Vector3.up * 2 + Vector3.forward * 2, Quaternion.identity, Vector3.zero);

			var location = DreamArrivalPoint.Location.Zone4;
			var arrivalPoint = Locator.GetDreamArrivalPoint(location);
			var dreamCampfire = Locator.GetDreamCampfire(location);
			if (Locator.GetToolModeSwapper().GetItemCarryTool().GetHeldItemType() != ItemType.DreamLantern)
			{
				var dreamLanternItem = GameObject.FindObjectsOfType<DreamLanternItem>().First(x => x._lanternType == DreamLanternType.Functioning && !x._lanternController.IsLit());
				Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(dreamLanternItem);
			}

			Locator.GetDreamWorldController().EnterDreamWorld(dreamCampfire, arrivalPoint, relativeLocation);

			yield return new WaitForSeconds(0.5f);

			WarpToPlanet(AstroObject.Name.DreamWorld, new Vector3(-2.5f, -290.5f, 685.8f));
		}
	}
}
