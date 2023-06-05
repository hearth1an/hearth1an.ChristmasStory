using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Travelers
{
	
	internal class PlayerNPCCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.SELF_DONE;

		PlayerNPCCharacterController instance;
		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player/Player_Dialogue").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player");

			instance = this;
			
            SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player/Traveller_HEA_Player_v2").DestroyAllComponents<FacePlayerWhenTalking>();

            var loopCoreController = SearchUtilities.Find("TowerTwin_Body/Sector_TowerTwin/Sector_TimeLoopInterior/Interactables_TimeLoopInterior/CoreCasingController").GetComponent<TimeLoopCoreController>();

			base.Start();

			ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				if (loopCoreController._playerEnteredCoreLastLoop && !Conditions.Get(Conditions.PERSISTENT.SELF_DONE))
				{
					originalCharacter.SetActive(true);
				}
				else
					originalCharacter.SetActive(false);
			});
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearNPCPlayer = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 150f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsShipNearVillage(100f) && !shipDestroyed;
			var shipFar = !shipNearNPCPlayer && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.NPC_PLAYER_SHIP_NEAR, shipNearNPCPlayer);
			Conditions.Set(Conditions.CONDITION.NPC_PLAYER_SHIP_FAR, shipFar);
			Conditions.Set(Conditions.CONDITION.SHIP_NEAR_VILLAGE, shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.NPC_PLAYER_START_DONE))
					{
						ChangeState(STATE.ON_SHIP);
					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.NPC_PLAYER_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);
						SearchUtilities.Find("TowerTwin_Body/Sector_TowerTwin/Sector_TimeLoopInterior/Interactables_TimeLoopInterior/CoreCasingController").GetComponent<TimeLoopCoreController>()._playerEnteredCoreLastLoop = false;
					}
					break;
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
