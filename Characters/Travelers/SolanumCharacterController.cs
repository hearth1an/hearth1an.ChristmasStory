using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace ChrismasStory.Characters.Travelers
{
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
