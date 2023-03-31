using ChristmasStory.Characters;
using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Travelers
{
	/* Find Riebec > talk with him > check if ship is near (write a script that will check distance between ship and Riebec) > Сlose eyes > 
	 * Riebec disappears, signal too > activating Riebec in ship > Escort him to TH > Check if we near the Village > 
	 * Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And he should be the only one Riebec and signal.
	 */
	internal class RiebeckCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.RIEBECK_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Traveller_HEA_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Traveller_HEA_Riebeck");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck");
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/Signal_Banjo").transform.localPosition = new UnityEngine.Vector3(0, 2f, 0);

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearRiebeck = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 100f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsShipNearVillage(100f) && !shipDestroyed;

			var shipFarNotDestroyed = !shipNearRiebeck && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.RIEBECK_SHIP_NEAR, shipNearRiebeck);
			Conditions.Set(Conditions.CONDITION.RIEBECK_SHIP_FAR, shipFarNotDestroyed);
			Conditions.Set(Conditions.CONDITION.SHIP_NEAR_VILLAGE, shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.RIEBECK_START_DONE))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.RIEBECK_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);
					}
					break;
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
