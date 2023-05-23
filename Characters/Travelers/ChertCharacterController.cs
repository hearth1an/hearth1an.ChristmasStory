using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using UnityEngine;

namespace ChristmasStory.Characters.Travelers
{	
	internal class ChertCharacterController : TravelerCharacterController
	{
		private GameObject _emberTwinShip, _timberHearthShip, _originalDrums;

		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.CHERT_DONE;

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

			if (State == STATE.AT_TREE && !Conditions.Get(Conditions.PERSISTENT.CHERT_LOOP_DIALOGUE_COMPLETE))
			{
				Conditions.Set(Conditions.CONDITION.CHERT_SHOW_LOOP_DIALOGUE, true);
			}
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
		//	var holdingItem = FindObjectOfType<ToolModeSwapper>()._currentToolMode == ToolMode.Item && FindObjectOfType<ToolModeSwapper>()._currentToolMode == ToolMode.None;

			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearChert = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 100f) && !shipDestroyed;
			var shipFar = !shipNearChert && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.CHERT_SHIP_NEAR, shipNearChert);
			Conditions.Set(Conditions.CONDITION.CHERT_SHIP_FAR, shipFar);

			Conditions.Set(Conditions.CONDITION.HOLDING_JUNK_ITEM, holdingJunkItem);			

			Conditions.Set(Conditions.CONDITION.HOLDING_CORE, holdingWarpCore);
			Conditions.Set(Conditions.CONDITION.HOLDING_DLC_ITEM, holdingStrangerArtifact);
						

			ValidateAllDone();			
		}

		private void ValidateAllDone()
		{
			var phraseTold = Conditions.Get(Conditions.CONDITION.CHERT_PHRASE_TOLD);
			var coreDone = Conditions.Get(Conditions.CONDITION.CHERT_CORE_DONE);
			var dlcDone = Conditions.Get(Conditions.CONDITION.CHERT_DLC_ITEM_DONE);

			var holdingSomething = HeldItemHandler.IsPlayerHoldingItem();

			if (phraseTold && coreDone && dlcDone)
			{
				Conditions.Set(Conditions.CONDITION.CHERT_ALL_DONE, true);
				ChristmasStory.Instance.ModHelper.Console.WriteLine("Chert conditions done!");
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_CORE, false);
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_DLC_ITEM, false);
			}
			if (phraseTold && coreDone && !dlcDone && !holdingSomething)
			{
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_CORE, true);
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_DLC_ITEM, false);
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_SOLO, false);
			}
			if (phraseTold && dlcDone && !coreDone && !holdingSomething)
			{
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_DLC_ITEM, true);
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_AND_CORE, false);
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_SOLO, false);
			}
			if (phraseTold && !holdingSomething && !dlcDone && !coreDone )
			{
				Conditions.Set(Conditions.CONDITION.CHERT_PHRASE_SOLO, true);
			}
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.CHERT_START_DONE))
					{
						ChangeState(STATE.AT_TREE);						
					}
					break;
			}

			Conditions.Set(Conditions.CONDITION.CHERT_SHOW_LOOP_DIALOGUE, false);
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
