using ChrismasStory.Components;
using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrismasStory.Characters.Travelers
{
    internal class RiebeckCharacterController : TravelerCharacterController
	{
		/* Find Riebec > talk with him > check if ship is near (write a script that will check distance between ship and Riebec) > Сlose eyes > 
		Riebec disappears, signal too > activating Riebec in ship > Escort him to TH > Check if we near the Village > 
		Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And he should be the only one Riebec and signal.
		*/

        public override void Start()
        {
			dialogue = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Traveller_HEA_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();
            dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();

            originalCharacter = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Traveller_HEA_Riebeck");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck");

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearRiebeck = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 100f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsCharacterNearVillage(shipCharacter.gameObject, 100f) && !shipDestroyed;

			var shipFarNotDestroyed = !shipNearRiebeck && !shipDestroyed;

			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_RIEBECK", shipNearRiebeck);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_RIEBECK", shipFarNotDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_VILLAGE", shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (DialogueConditionManager.SharedInstance.GetConditionState("RIEBECK_START_DONE"))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (DialogueConditionManager.SharedInstance.GetConditionState("RIEBECK_SHIP_DONE"))
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
