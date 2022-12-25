using NewHorizons.Utility;
using ChrismasStory.Components;

namespace ChrismasStory.Characters.Travelers
{
    internal class PlayerNPCCharacterController : TravelerCharacterController
	{
        /* 
            Same thing, you can ask yourself if you can go to TH just for fun. > closing eyes > he appears in ship > flight to TH > talk, closing eyes > He appears near the Christmas tree

            */

        public override void Start()
        {
            dialogue = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player").GetComponent<CharacterDialogueTree>();
            dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<CharacterDialogueTree>();
            dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player/ConversationZone").GetComponent<CharacterDialogueTree>();

            originalCharacter = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player");

			base.Start();			
        }

        protected override void Dialogue_OnStartConversation()
        {
            var shipDestroyed = ShipHandler.HasShipExploded();

            var shipNearNPCPlayer = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 150f) && !shipDestroyed;
            var shipNearVillage = ShipHandler.IsCharacterNearVillage(shipCharacter.gameObject, 100f) && !shipDestroyed;
            var shipFar = !shipNearNPCPlayer && !shipDestroyed;

            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_NPC_PLAYER", shipNearNPCPlayer);
            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_NPC_PLAYER", shipFar);
            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_VILLAGE", shipNearVillage);
        }

        protected override void Dialogue_OnEndConversation()
        {
            switch (State)
            {
                case STATE.ORIGINAL:
                    if (DialogueConditionManager.SharedInstance.GetConditionState("NPC_PLAYER_START_DONE"))
                    {
                        ChangeState(STATE.ON_SHIP);
                    }
                    break;
                case STATE.ON_SHIP:
                    if (DialogueConditionManager.SharedInstance.GetConditionState("NPC_PLAYER_SHIP_DONE"))
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
