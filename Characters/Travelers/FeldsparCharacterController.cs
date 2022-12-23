using ChrismasStory.Components;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
    internal class FeldsparCharacterController : TravelerCharacterController
	{
        /* Find Feldspar's note > Change some signals destinations > Find him in Dark Bramble, talk to him > Сlose eyes > 
		He disappears, signal too > activating Feldspar in ship > Escort him to TH carefully > Check if we near the Village > 
		Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And sure he should be the only one Feldspar and signal (!!! There's a lot of signals). 
		*/

        public override void Start()
        {
			dialogue = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar");

            base.Start();

			if (!PlayerData.PersistentConditionExists("FELDSPAR_START_ENTRY"))
			{
				ChangeState(STATE.NONE);
			}
			if (PlayerData.PersistentConditionExists("FELDSPAR_SHIP_DONE"))
			{
				ChangeState(STATE.AT_TREE);
			}
			else
			{
				ChangeState(STATE.ORIGINAL);
			}
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipNearFeldspar = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f);
			var shipNearVillage = ShipHandler.IsCharacterNearVillage(shipCharacter.gameObject, 100f);
			var shipDestroyed = ShipHandler.HasShipExploded();
			var shipFarNotDestroyed = !shipNearFeldspar && !shipDestroyed;

			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_FELDSPAR", shipNearFeldspar);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_FELDSPAR", shipFarNotDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_VILLAGE", shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (DialogueConditionManager.SharedInstance.GetConditionState("FELDSPAR_START_DONE"))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (DialogueConditionManager.SharedInstance.GetConditionState("FELDSPAR_SHIP_DONE"))
					{
						ChangeState(STATE.AT_TREE);
					}
					break;
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{
			// We can remove the signals here when it changes state
		}
	}
}
