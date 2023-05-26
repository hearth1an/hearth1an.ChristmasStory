using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using NewHorizons.Components;
using UnityEngine;

namespace ChristmasStory.Characters.Villagers
{
	
	internal class HalCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.HAL_ROCK_TOLD;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").GetComponent<CharacterDialogueTree>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/ConversationZone").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_2").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger_2").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_3").SetActive(false);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/ConversationZone").DestroyAllComponents<InteractReceiver>(); 						
					
			base.Start();

			ChangeStoneItem();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.HAL_ROCK_DONE)) // && PlayerData.GetPersistentCondition("LOOP_COUNT_GOE_2"))
			{
				PlayerEffectController.Blink(2f);
				PlayerEffectController.AddLock(2f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").SetActive(false);
				Invoke("SwapDialogue", 2f);
				WriteUtil.WriteLine("Hal dialogue swapping!");
			}
		}

		private void ChangeStoneItem()
        {
			var inviteStone = SearchUtilities.Find("Invite_Stone");
			inviteStone.DestroyAllComponents<SharedStone>();
			inviteStone.AddComponent<InviteStone>();
			

			SearchUtilities.Find("Hal_Text").transform.parent = inviteStone.transform;
			SearchUtilities.Find("Hal_Text").transform.localPosition = new Vector3(0, 0, 0);
			var textArc = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Invite_Stone/Hal_Text/Arc 1 - Child of -1");
			textArc.transform.localPosition = new Vector3(0.1f, -0.1f, 0.08f);
			textArc.transform.localRotation = new Quaternion(0, 0, 0.6279f, 0.7783f);
			textArc.GetComponent<NomaiTextLine>()._radius = 2;
			textArc.GetComponent<NomaiTextLine>()._totalLength = 0.5f;
			textArc.GetComponent<MeshRenderer>().forceRenderingOff = true;
		}

		private void SwapDialogue()
		{
			PlayerEffectController.PlayAudioOneShot(AudioType.ToolItemSharedStonePickUp, 1f);
			HeldItemHandler.GivePlayerInviteStone();			
			Invoke("EnableDialogue3", 1f);
		}

		private void EnableDialogue3()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_3").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_3").GetComponent<CharacterDialogueTree>().StartConversation();
		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
