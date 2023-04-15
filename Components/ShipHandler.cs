using UnityEngine;
using NewHorizons.Utility;
using HarmonyLib;
using UnityEngine.Events;
using ChristmasStory.Utility;

namespace ChristmasStory.Components
{
	[HarmonyPatch]
	internal class ShipHandler : MonoBehaviour
	{
		private ShipCockpitController _shipCockpitController;
		private GameObject _shipBody;
		private ShipDamageController _shipDamageController;
		private GameObject _villageSector;
		private bool shipExplodedByProbe = false;
		public UnityEvent ShipExplosion { get; private set; }

		public static ShipHandler Instance;

		public void Start()
		{
			Instance = this;

			ShipExplosion = new();

			_shipCockpitController = FindObjectOfType<ShipCockpitController>();
			_shipDamageController = gameObject.GetComponent<ShipDamageController>();
			_shipBody = Locator.GetShipBody().gameObject;
			_villageSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking");

			SearchUtilities.Find("ProbeBroken").SetActive(false);		
			
		}

		

		/// <summary>
		/// Checks if a character (give their game object) is within a specific distance of the ship
		/// </summary>
		/// <param name="character"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static bool IsCharacterNearShip(GameObject character, float distance)
		{
			return (character.transform.position - Instance._shipBody.transform.position).sqrMagnitude < distance * distance;
		}

		public static bool IsShipNearVillage(float distance)
		{
			return (Instance._shipBody.transform.position - Instance._villageSector.transform.position).sqrMagnitude < distance * distance;
		}

		public static bool HasShipExploded() => Instance._shipDamageController._hullBreach;


		/// <summary>
		/// For testing!
		/// </summary>
		public static void BlowUpShip()
		{
			Instance._shipDamageController.TriggerSystemFailure(true);
			Instance._shipDamageController.TriggerElectricalFailure(true);
			Instance._shipDamageController.TriggerReactorCritical(true);
			Instance._shipDamageController.TriggerHullBreach(true);
			Instance._shipDamageController.Explode(true);
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(ShipDamageController), nameof(ShipDamageController.Explode))]
		private static void ShipDamageController_Explode()
		{
			WriteUtil.WriteDebug("Explosion event");
			Conditions.Set(Conditions.CONDITION.SHIP_DESTROYED, true);
			Instance.ShipExplosion?.Invoke();
			SearchUtilities.Find("Ship_Body/ShipSector").SetActive(false);			
		}

		private void OnSectorOccupantsUpdated()
		{
			var probeGrav = SearchUtilities.Find("Probe_Body/ProbeGravity/CapsuleVolume_NOM_GravityCrystal");
			var shipSector = SearchUtilities.Find("Ship_Body/ShipSector").GetComponent<Sector>();

			if (PlayerState._insideShip && probeGrav.activeSelf)
            {
				probeGrav.SetActive(false);
			}
            if (!PlayerState._insideShip && !shipExplodedByProbe && !probeGrav.activeSelf )
            {
				probeGrav.SetActive(true);
            }

			if (shipSector.ContainsAnyOccupants(DynamicOccupant.Probe) && !Instance._shipDamageController._exploded)
			{
				probeGrav.SetActive(false);
				SearchUtilities.Find("ProbeBroken").SetActive(true);
				SearchUtilities.Find("Probe_Body/ProbeGravity/AudioSource_GravityCrystal").SetActive(false);
				shipExplodedByProbe = true;
				BlowUpShip();
			};
		}


		public void Update()
		{
			OnSectorOccupantsUpdated();			
		}


	}
}
