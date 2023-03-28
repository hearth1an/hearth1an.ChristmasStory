using UnityEngine;
using NewHorizons.Utility;
using HarmonyLib;
using UnityEngine.Events;
using ChristmasStory.Utility;

namespace ChrismasStory.Components
{
	[HarmonyPatch]
	internal class ShipHandler : MonoBehaviour
	{
		private ShipCockpitController _shipCockpitController;
		private GameObject _shipBody;
		private ShipDamageController _shipDamageController;
		private GameObject _villageSector;

		public UnityEvent ShipExplosion { get; private set; }

		public static ShipHandler Instance;

		public void Start()
		{
			Instance = this;

			ShipExplosion = new();

			_shipCockpitController = GameObject.FindObjectOfType<ShipCockpitController>();
			_shipDamageController = gameObject.GetComponent<ShipDamageController>();
			_shipBody = Locator.GetShipBody().gameObject;
			_villageSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking");

			
			
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
			ChristmasStory.WriteDebug("Explosion event");
			Conditions.Set(Conditions.CONDITION.SHIP_DESTROYED, true);
			Instance.ShipExplosion?.Invoke();
        }

        public void Update()
        {
			var playerState = Locator.GetPlayerController()._groundBody;
			var probeGrav = SearchUtilities.Find("Probe_Body/ProbeGravity/CapsuleVolume_NOM_GravityCrystal");
			var probeLaunch = Locator.GetProbe();
			var shipBody = Locator.GetShipBody();
			var shipModule = shipBody.GetComponent<ShipThrusterController>();

            if (IsCharacterNearShip(probeLaunch.gameObject, 10) && !Instance._shipDamageController._exploded && probeGrav.activeSelf && shipModule.isActiveAndEnabled == true)
            {
                probeGrav.SetActive(false);
            }
			if (IsCharacterNearShip(probeLaunch.gameObject, 10) && !Instance._shipDamageController._exploded && !probeGrav.activeSelf && !shipModule.isActiveAndEnabled && probeLaunch.IsLaunched())
			{
				BlowUpShip();
			}

			if (playerState == shipBody && Instance._shipDamageController._exploded != true && probeGrav.activeSelf)
			{
				probeGrav.SetActive(false);
			}
			else if (playerState == shipBody && probeLaunch.IsLaunched() && !probeGrav.activeSelf)
			{
				BlowUpShip();				
			}
			else if (playerState != shipBody && !probeGrav.activeSelf)
			{
				probeGrav.SetActive(true);
			}
			else return;
		}


    }
}
