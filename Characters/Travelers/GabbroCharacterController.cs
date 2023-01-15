using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace ChrismasStory.Characters.Travelers
{
	/*
	 * Visit Gabbro
	 * Gabbro asks the player to blow them up with the ship
	 * Gabbro dies
	 * Next loop Gabbro is at TH with their ship
	 */

	internal class GabbroCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.GABBRO_DONE;

		private GameObject _gdShip, _thShip;
		private GameObject _signal;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/ConversationZone").GetComponent<CharacterDialogueTree>();
			_signal = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Signal_Flute");

			originalCharacter = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute");
			
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro"); // Will need to change the model since he don't have any static animation. Probably the best way is to rip from Eye Scene

			_gdShip = SearchUtilities.Find("GabbroShip_Body");
			_thShip = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GabbroShip");

			base.Start();

			ShipHandler.Instance.ShipExplosion.AddListener(ShipHandler_ShipExplosion);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			ShipHandler.Instance?.ShipExplosion?.RemoveListener(ShipHandler_ShipExplosion);
		}

		private void ShipHandler_ShipExplosion()
		{
			if (ShipHandler.IsCharacterNearShip(originalCharacter, 30f))
			{
				ChristmasStory.WriteDebug("Kaboom!");
				Conditions.Set(DoneCondition, true);
				StartCoroutine(KillGabbro());
			}
		}

		private IEnumerator KillGabbro()
		{
			// Disappear after 1.5 seconds

			yield return 1.5f;
			originalCharacter.SetActive(false);
			_signal?.SetActive(false);
			dialogue?.gameObject?.SetActive(false);
		}

		protected override void Dialogue_OnStartConversation() { }
		protected override void Dialogue_OnEndConversation() { }
		protected override void OnChangeState(STATE oldState, STATE newState) 
		{
			_gdShip?.SetActive(newState == STATE.ORIGINAL);
			_thShip?.SetActive(newState == STATE.AT_TREE);

			_signal?.SetActive(newState == STATE.ORIGINAL);
			dialogue?.gameObject?.SetActive(newState == STATE.ORIGINAL);
		}
	}
}
