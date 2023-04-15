using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Travelers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class EskerCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.ESKER_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/Esker_Start_Dialogue").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking");

			originalCharacter.SetActive(true);

			base.Start();

			if (State == STATE.AT_TREE && !Conditions.Get(Conditions.PERSISTENT.ESKER_LOOP_DIALOGUE_COMPLETE))
			{
				Conditions.Set(Conditions.CONDITION.ESKER_SHOW_LOOP_DIALOGUE, true);
			}
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearEsker = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsShipNearVillage(100f) && !shipDestroyed;
			var shipFar = !shipNearEsker && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.ESKER_SHIP_NEAR, shipNearEsker);
			Conditions.Set(Conditions.CONDITION.ESKER_SHIP_FAR, shipFar);
			Conditions.Set(Conditions.CONDITION.SHIP_NEAR_VILLAGE, shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.ESKER_START_DONE))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.ESKER_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);
					}
					break;
			}

			Conditions.Set(Conditions.CONDITION.ESKER_SHOW_LOOP_DIALOGUE, false);
		}

		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
