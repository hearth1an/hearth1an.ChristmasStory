using ChrismasStory.Components;
using NewHorizons.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace ChrismasStory.Characters.Travelers
{
	internal class SolanumCharacterController : TravelerCharacterController
	{
		/* Find Chert > talk with him (he don't want to leave and you telling him that you know everything about nomai, supernova, stranger, time loop etc. He will laugh and ask to proof 3 things. 
		> 1st thing is timeloop. He will say the information only he knows, you will need to tell him it next loop. 
		> 2nd bring unknown nomai tech (Warp Core) and 3rd - strangers artifact. Since you have one loop to tell him everything, you need to bring both things. > 
		There should be a script that will check distance between Chert and Warp Core, Chert and artifact. If it worked => RunWhen thing to explore the fact that will open the dialogue node to make Chert disappear
		> Closing eyes > he appears near the Christmas tree always. And he should be the only one Chert and signal. 
		*/


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
