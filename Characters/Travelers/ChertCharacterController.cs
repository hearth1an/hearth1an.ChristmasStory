using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using OWML.ModHelper;
using UnityEngine;

namespace ChrismasStory.Characters.Travelers
{
	/* 
	 * Find Chert > talk with him (he don't want to leave and you telling him that you know everything about nomai, supernova, stranger, time loop etc. He will laugh and ask to proof 3 things. 
	 * 1st thing is timeloop. He will say the information only he knows, you will need to tell him it next loop. 
	 * 2nd bring unknown nomai tech (Warp Core) and 3rd - strangers artifact. Since you have one loop to tell him everything, you need to bring both things. > 
	 * There should be a script that will check distance between Chert and Warp Core, Chert and artifact. If it worked => RunWhen thing to explore the fact that will open the dialogue node to make Chert disappear
	 * Closing eyes > he appears near the Christmas tree always. And he should be the only one Chert and signal. 
	 */

	internal class ChertCharacterController : TravelerCharacterController
	{
		private GameObject _emberTwinShip, _timberHearthShip, _originalDrums;

		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.CHERT_DONE;
		public bool isArtifactDestroyed;

		public override void Start()
		{
			_emberTwinShip = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Geometry_CaveTwin/OtherComponentsGroup/Prefab_HEA_ChertShip");
			_timberHearthShip = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip");

			_originalDrums = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Volumes_Lakebed/Signal_Drums");

			dialogue = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper");

            dialogue.OnSelectDialogueOption += Dialogue_OnSelectDialogueOption;			

			base.Start();			
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			if (dialogue) dialogue.OnSelectDialogueOption -= Dialogue_OnSelectDialogueOption;
		}

		private void Dialogue_OnSelectDialogueOption()
		{
			ValidateAllDone();
        }

        protected override void Dialogue_OnStartConversation()
        {
            var holdingWarpCore = HeldItemHandler.IsPlayerHoldingWarpCore();
            var holdingStrangerArtifact = HeldItemHandler.IsPlayerHoldingStrangerArtifact();
            var holdingJunkItem = HeldItemHandler.IsPlayerHoldingJunk();

            var shipDestroyed = ShipHandler.HasShipExploded();

            var shipNearChert = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 100f) && !shipDestroyed;
            var shipFar = !shipNearChert && !shipDestroyed;

			

            Conditions.Set(Conditions.CONDITION.CHERT_SHIP_NEAR, shipNearChert);
            Conditions.Set(Conditions.CONDITION.CHERT_SHIP_FAR, shipFar);

            Conditions.Set(Conditions.CONDITION.HOLDING_CORE, holdingWarpCore);
			Conditions.Set(Conditions.CONDITION.HOLDING_DLC_ITEM, holdingStrangerArtifact);

            Conditions.Set(Conditions.CONDITION.HOLDING_JUNK_ITEM, holdingJunkItem);

			ValidateAllDone();

			// Delay.FireOnNextUpdate(ValidateAllDone);			
		}

		private void ValidateAllDone()
		{
			var phraseTold = Conditions.Get(Conditions.CONDITION.CHERT_PHRASE_TOLD);
			var coreDone = Conditions.Get(Conditions.CONDITION.CHERT_CORE_DONE);
			var dlcDone = Conditions.Get(Conditions.CONDITION.CHERT_DLC_ITEM_DONE);

			if (phraseTold && coreDone && dlcDone)
			{
				Conditions.Set(Conditions.CONDITION.CHERT_ALL_DONE, true);
				ChristmasStory.Instance.ModHelper.Console.WriteLine("Chert conditions done!");
			}
		}

		protected override void Dialogue_OnEndConversation()
        {
			var toDestroy = SearchUtilities.Find("Player_Body/PlayerCamera/ItemCarryTool/DreamLanternSocket").GetAllChildren();

			if (Conditions.Get(Conditions.CONDITION.CHERT_DLC_ITEM_DONE) && !isArtifactDestroyed)
            {
				GameObject.Destroy(toDestroy[0]);
				isArtifactDestroyed = true;
			}
                //SearchUtilities.Find("Player_Body/PlayerCamera/ItemCarryTool").GetComponent<ItemTool>().enabled = true;

                switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.CHERT_START_DONE))
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

			// The drum signal is separate from chert's game object
			_originalDrums?.SetActive(newState == STATE.ORIGINAL);
		}

		
    }
}
