using NewHorizons.Utility;
using UnityEngine;
using ChrismasStory.Components;

namespace ChrismasStory.Characters.Travelers
{
    internal class ChertCharacterController : TravelerCharacterController
	{
        /* Find Chert > talk with him (he don't want to leave and you telling him that you know everything about nomai, supernova, stranger, time loop etc. He will laugh and ask to proof 3 things. 
		> 1st thing is timeloop. He will say the information only he knows, you will need to tell him it next loop. 
		> 2nd bring unknown nomai tech (Warp Core) and 3rd - strangers artifact. Since you have one loop to tell him everything, you need to bring both things. > 
		There should be a script that will check distance between Chert and Warp Core, Chert and artifact. If it worked => RunWhen thing to explore the fact that will open the dialogue node to make Chert disappear
		> Closing eyes > he appears near the Christmas tree always. And he should be the only one Chert and signal. 
		*/

        private GameObject _emberTwinShip, _timberHearthShip;

        public override void Start()
        {
            _emberTwinShip = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Geometry_CaveTwin/OtherComponentsGroup/Prefab_HEA_ChertShip");
            _timberHearthShip = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip");

            dialogue = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/ConversationZone").GetComponent<CharacterDialogueTree>();

            originalCharacter = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper");

			

			base.Start();

            if (PlayerData.PersistentConditionExists("CHERT_SHIP_DONE"))
            {
                ChangeState(STATE.AT_TREE);
                _emberTwinShip.SetActive(false);
            }           
            else
            {
                ChangeState(STATE.ORIGINAL);
                _timberHearthShip.SetActive(false);
            }
        }

        protected override void Dialogue_OnStartConversation()
        {
            var shipNearChert = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f);
            var holdingWarpCore = HeldItemHandler.IsPlayerHoldingWarpCore();
            var holdingStrangerArtifact = HeldItemHandler.IsPlayerHoldingStrangerArtifact();
            // var chertShipFly = _emberTwinShip.AddComponent<OWRigidbody>();


            var shipDestroyed = ShipHandler.HasShipExploded();
            var shipFarNotDestroyed = !shipNearChert && !shipDestroyed;

            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_CHERT", shipNearChert);
            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_CHERT", shipFarNotDestroyed);
            DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
            // DialogueConditionManager.SharedInstance.SetConditionState("CHERT_START_DONE", chertShipFly);
            DialogueConditionManager.SharedInstance.SetConditionState("HOLDING_CORE", holdingWarpCore);
            DialogueConditionManager.SharedInstance.SetConditionState("HOLDING_DLC_ITEM", holdingStrangerArtifact);
        }
           

        protected override void Dialogue_OnEndConversation()
        {
            switch (State)
            {
                case STATE.ORIGINAL:
                    if (DialogueConditionManager.SharedInstance.GetConditionState("CHERT_START_DONE"))
                    {
                        ChangeState(STATE.AT_TREE);
                    }
                    break;
                case STATE.ON_SHIP:
                    if (DialogueConditionManager.SharedInstance.GetConditionState("CHERT_SHIP_DONE"))
                    {
                        ChangeState(STATE.AT_TREE);
                    }
                    break;
            }
        }


        protected override void OnChangeState(STATE oldState, STATE newState)
        {
            // Here we have to update the visibility of the ship at ember twin and the ship at timber hearth
            _emberTwinShip?.SetActive(newState == STATE.ORIGINAL);
            _timberHearthShip?.SetActive(newState == STATE.AT_TREE);
        }

        public void DestroyChertShip()
        {
            _emberTwinShip.SetActive(false);
            ChangeState(STATE.ON_SHIP);
        }
    }
}
