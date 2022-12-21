using UnityEngine;

namespace ChrismasStory.Components
{
	internal class ShipHandler : MonoBehaviour
	{
		private ShipCockpitController _shipCockpitController;
		private GameObject _shipBody;
		private ShipDamageController _shipDamageController;

		private static ShipHandler _instance;

		private void Start()
		{
			_shipCockpitController = GameObject.FindObjectOfType<ShipCockpitController>();
			_shipDamageController = gameObject.GetComponent<ShipDamageController>();
			_shipBody = Locator.GetShipBody().gameObject;

			_instance = this;
		}

		/// <summary>
		/// Checks if a character (give their game object) is within a specific distance of the ship
		/// </summary>
		/// <param name="character"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static bool IsCharacterNearShip(GameObject character, float distance)
		{
			// Shouldn't count if the ship blew up
			if (HasShipExploded()) return false;

			return (character.transform.position - _instance._shipBody.transform.position).sqrMagnitude < distance * distance;
		}

		public static bool HasShipExploded() => _instance._shipDamageController._hullBreach;

		/// <summary>
		/// For testing!
		/// </summary>
		public static void BlowUpShip()
		{
			_instance._shipDamageController.TriggerHullBreach(true);
			_instance._shipDamageController.TriggerElectricalFailure(true);
			_instance._shipDamageController.TriggerReactorCritical(true);
		}

	}
}
