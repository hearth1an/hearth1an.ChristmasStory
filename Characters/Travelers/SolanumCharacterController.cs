using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
	/* 
	At the start of the game when player will have a conversation with Gal. He will say that he happy about translator and he also can do nomai writings on stones and he 
	wish if someone could read them one day. > Player will need to ask him if he could write something like "Merry Christmas, Solanum! Join us to celebration on Timber Hearth!" > closing eyes, stone with writing appears >
	player brings it to Solanum > Drop it near her > Script checking the distance > she replying that she probably will be able to appear on TH, we just need to take Nomai rock to TH > player takes it and brings to TH.

	> she appears there

	*/
	internal class SolanumCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.SOLANUM_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/Interactables_EYEState/ConversationPivot/Character_NOM_Solanum/Nomai_ANIM_SkyWatching_Idle/ConversationZone").GetComponent<CharacterDialogueTree>();
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle");

			base.Start();

			HeldItemHandler.Instance.BringItem.AddListener(HeldItemHandler_BringItemDone);
		}

		public override void OnDestroy()
		{
			ChangeState(STATE.AT_TREE);
			base.OnDestroy();
			HeldItemHandler.Instance?.BringItem?.RemoveListener(HeldItemHandler_BringItemDone);
		}

		protected override void Dialogue_OnStartConversation()
		{
			var holdingInviteStone = HeldItemHandler.IsPlayerHoldingInviteStone();
			// var inviteStoneNearTheVillage = HeldItemHandler.IsCharacterNearVillage();
			DialogueConditionManager.SharedInstance.SetConditionState("HOLDING_INVITE_STONE", holdingInviteStone);
		}

		protected override void Dialogue_OnEndConversation()
		{
			if (DialogueConditionManager.SharedInstance.GetConditionState("SOLANUM_START"))
			{
				SolanumAnimationController.Instance.SolanumAnimEvent();

				DialogueConditionManager.SharedInstance.SetConditionState("SOLANUM_START", false);
				DialogueConditionManager.SharedInstance.SetConditionState("SOLANUM_START_DONE", true);

			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}

		private void HeldItemHandler_BringItemDone()
		{
			if (HeldItemHandler.IsCharacterNearVillage(50f))
			{
				PlayerData.SetPersistentCondition("SOLANUM_DONE", true);
			}
		}
	}
}
