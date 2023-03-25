using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class HalCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.HAL_ROCK_TOLD;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").GetComponent<CharacterDialogueTree>();			

			//originalCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village");			

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{
			
		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.HAL_ROCK_DONE)) // && PlayerData.GetPersistentCondition("LOOP_COUNT_GOE_2"))
			{
				PlayerEffectController.Blink(2f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").SetActive(false);
				Invoke("SwapDialogue", 2f);
				ChristmasStory.WriteLine("Hal dialogue swapping!");
			}			
		}

		private void SwapDialogue()
        {
			PlayerEffectController.PlayAudioOneShot(AudioType.ToolItemSharedStonePickUp, 1f);
			HeldItemHandler.GivePlayerInviteStone();			
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_2_ConversationTrigger").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_2").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_2_ConversationTrigger").transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
			
		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
