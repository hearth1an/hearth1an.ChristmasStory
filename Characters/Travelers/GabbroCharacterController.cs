using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
	internal class GabbroCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.GABBRO_DONE;

		/* Visit Gabbro > He will ask you to start the new loop with exploding your ship near him > There should be a script that will check distance between Gabbro and ship and track the explosion > 
Gabbro should disappear, player should die too.> Next loop he will appear Feldspar and signal. 
*/

		public override void Start()
		{
			dialogue = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute");

			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro"); // Will need to change the model since he don't have any static animation. Probably the best way is to rip from Eye Scene

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
				Conditions.Set(Conditions.PERSISTENT.GABBRO_DIED_EXPLOSION, true);
				originalCharacter.SetActive(false);
			}
		}

		protected override void Dialogue_OnStartConversation() { }
		protected override void Dialogue_OnEndConversation() { }
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
