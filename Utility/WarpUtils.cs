using NewHorizons.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChristmasStory.Utility
{
	internal static class WarpUtils
	{
		public static void WarpToSixthLocation()
		{
			var quantumMoon = GameObject.FindObjectOfType<QuantumMoon>();

			WarpToPlanet(AstroObject.Name.QuantumMoon, new Vector3(5f, 70f, 0f));

			//SetQuantumMoonState(quantumMoon, 5);

			quantumMoon.SetSurfaceState(5);
		}

		private static void SetQuantumMoonState(QuantumMoon quantumMoon, int num)
		{
			int num2 = -1;
			for (int j = 0; j < quantumMoon._orbits.Length; j++)
			{
				if (quantumMoon._orbits[j].GetStateIndex() == num)
				{
					num2 = j;
					break;
				}
			}
			if (num2 == -1)
			{
				Debug.LogError("QUANTUM MOON FAILED TO FIND ORBIT FOR STATE " + num);
			}
			float d = (num2 != -1) ? quantumMoon._orbits[num2].GetOrbitRadius() : 10000f;
			OWRigidbody owrigidbody = (num2 != -1) ? quantumMoon._orbits[num2].GetAttachedOWRigidbody() : Locator.GetAstroObject(AstroObject.Name.Sun).GetOWRigidbody();
			Vector3 onUnitSphere = Random.onUnitSphere;
			if (num == 5)
			{
				onUnitSphere.y = 0f;
				onUnitSphere.Normalize();
			}
			Vector3 position = onUnitSphere * d + owrigidbody.GetWorldCenterOfMass();
			if (!Physics.CheckSphere(position, quantumMoon._sphereCheckRadius, OWLayerMask.physicalMask) || quantumMoon._collapseToIndex != -1)
			{
				quantumMoon._visibilityTracker.transform.position = position;
				if (!Physics.autoSyncTransforms)
				{
					Physics.SyncTransforms();
				}

				quantumMoon._moonBody.transform.position = position;
				if (!Physics.autoSyncTransforms)
				{
					Physics.SyncTransforms();
				}
				quantumMoon._visibilityTracker.transform.localPosition = Vector3.zero;
				quantumMoon._constantForceDetector.AddConstantVolume(owrigidbody.GetAttachedGravityVolume(), true, true);
				Vector3 b = owrigidbody.GetVelocity();
				if (quantumMoon._useInitialMotion)
				{
					InitialMotion component = owrigidbody.GetComponent<InitialMotion>();
					b = ((component != null) ? component.GetInitVelocity() : Vector3.zero);
					quantumMoon._useInitialMotion = false;
				}
				quantumMoon._moonBody.SetVelocity(OWPhysics.CalculateOrbitVelocity(owrigidbody, quantumMoon._moonBody, (float)Random.Range(0, 360)) + b);
				quantumMoon._useInitialMotion = false;
				quantumMoon._lastStateIndex = quantumMoon._stateIndex;
				quantumMoon._stateIndex = num;
				quantumMoon._collapseToIndex = -1;

				for (int k = 0; k < quantumMoon._stateSkipCounts.Length; k++)
				{
					quantumMoon._stateSkipCounts[k] = ((k == quantumMoon._stateIndex) ? 0 : (quantumMoon._stateSkipCounts[k] + 1));
				}

				quantumMoon._visibilityTracker.transform.localPosition = Vector3.zero;
			}
			else
			{
				Debug.LogError("Quantum moon orbit position occupied! Aborting collapse.");
			}

			if (quantumMoon._isPlayerInside)
			{
				quantumMoon.SetSurfaceState(quantumMoon._stateIndex);
			}
			else
			{
				quantumMoon.SetSurfaceState(-1);
				quantumMoon._quantumSignal.SetSignalActivation(quantumMoon._stateIndex != 5, 2f);
			}
			quantumMoon._referenceFrameVolume.gameObject.SetActive(quantumMoon._stateIndex != 5);
			quantumMoon._moonBody.SetIsTargetable(quantumMoon._stateIndex != 5);
			for (int l = 0; l < quantumMoon._deactivateAtEye.Length; l++)
			{
				quantumMoon._deactivateAtEye[l].SetActive(quantumMoon._stateIndex != 5);
			}
			GlobalMessenger<OWRigidbody>.FireEvent("QuantumMoonChangeState", quantumMoon._moonBody);
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
	}
}
