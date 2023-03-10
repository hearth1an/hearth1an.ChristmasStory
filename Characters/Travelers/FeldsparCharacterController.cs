using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
	/* Find Feldspar's note > Change some signals destinations > Find him in Dark Bramble, talk to him > Сlose eyes > 
	 * He disappears, signal too > activating Feldspar in ship > Escort him to TH carefully > Check if we near the Village > 
	 * Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And sure he should be the only one Feldspar and signal (!!! There's a lot of signals). 
	 */

	internal class FeldsparCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.FELDSPAR_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar");

			

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearFeldspar = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsShipNearVillage(100f) && !shipDestroyed;
			var shipFar = !shipNearFeldspar && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.FELDSPAR_SHIP_NEAR, shipNearFeldspar);
			Conditions.Set(Conditions.CONDITION.FELDSPAR_SHIP_FAR, shipFar);
			Conditions.Set(Conditions.CONDITION.SHIP_NEAR_VILLAGE, shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.FELDSPAR_START_DONE))
					{
						ChangeState(STATE.ON_SHIP);
						Invoke("SpawnAnglers", 4f);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);
					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.FELDSPAR_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);
					}
					break;
			}
		}
		private void SpawnAnglers()
        {
			SearchUtilities.Find("Ernando").transform.localPosition = new UnityEngine.Vector3(5f, -5f, -30f);
			SearchUtilities.Find("Ernando").SetActive(true);

			SearchUtilities.Find("Gustavo").SetActive(true);
			SearchUtilities.Find("Gustavo").transform.localPosition = new UnityEngine.Vector3(9f, -3f, -20f);

			SearchUtilities.Find("Rudolfo").transform.localPosition = new UnityEngine.Vector3(3f, -7f, -19f);
			SearchUtilities.Find("Rudolfo").SetActive(true);
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}
