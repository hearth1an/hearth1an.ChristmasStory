using ChrismasStory.Components;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
	internal class EskerCharacterController : TravelerCharacterController
	{
		/* Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
		close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
		*/

		public override void Start()
		{
			dialogue = SearchUtilities.Find("Moon_Body/Sector_THM/Esker_Start_Dialogue").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking");

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipNearEsker = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f);
			var shipNearVillage = ShipHandler.IsCharacterNearVillage(shipCharacter.gameObject, 100f);
			var shipDestroyed = ShipHandler.HasShipExploded();
			var shipFarNotDestroyed = !shipNearEsker && !shipDestroyed;

			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_ESKER", shipNearEsker);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_ESKER", shipFarNotDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_VILLAGE", shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (DialogueConditionManager.SharedInstance.GetConditionState("ESKER_START_DONE"))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (DialogueConditionManager.SharedInstance.GetConditionState("ESKER_SHIP_DONE"))
					{
						ChangeState(STATE.AT_TREE);
					}
					break;
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}
