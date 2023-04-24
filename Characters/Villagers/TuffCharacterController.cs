using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using NewHorizons.Handlers;

namespace ChristmasStory.Characters.Villagers
{	
	internal class TuffCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.TUFF_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Tuff_Dialogue").GetComponent<CharacterDialogueTree>();
			
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Gossan_Radio/Props_HEA_WoodenCrate_Radio").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Tuff_Pickaxe").SetActive(false);
			SearchUtilities.Find("Elevator_Dialogue").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/ConversationZone_Tuff").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/ConversationZone_Tuff").DestroyAllComponents<CharacterDialogueTree>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone_Tuff").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone_Tuff").DestroyAllComponents<CharacterDialogueTree>();
			SearchUtilities.Find("Signal_Radio").SetActive(false);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff").GetComponent<FacePlayerWhenTalking>()._dialogueTree = dialogue;

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			if (Conditions.Get(Conditions.PERSISTENT.TUFF_DONE))
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff").SetActive(true);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan").SetActive(true);
			}
			else
            {
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan").SetActive(false);
			}

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.PERSISTENT.TUFF_DONE))
			{
				PlayerEffectController.CloseEyes(1f);
				PlayerEffectController.AddLock(2f);
				SearchUtilities.Find("Elevator_Dialogue").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Tuff_Dialogue").SetActive(false);				

				Invoke("AfterDialogue", 2f);
				Invoke("OpenEyesDelayed", 4f);
				Invoke("RunElevator", 12f);
				Invoke("DelayRadio", 35f);
			}
			if (Conditions.Get(Conditions.CONDITION.ELEVATOR_DONE))
			{
				SearchUtilities.Find("Elevator_Dialogue").SetActive(false);	
			}
		}
        private void AfterDialogue()
        {
			var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/tuff_go.mp3");
			PlayerEffectController.PlayAudioExternalOneShot(sfx, 1f);
			
			var pickAxe = SearchUtilities.Find("Tuff_Pickaxe");
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Tuff_Radio/Elevator_Dialogue").GetComponent<InteractReceiver>().ChangePrompt(TranslationHandler.GetTranslation("RADIO_PROMT", TranslationHandler.TextType.UI));
			pickAxe.AddComponent<NewHorizons.Components.AddPhysics>();			
			pickAxe.SetActive(true);						
		}

		private void RunElevator() => SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator").GetComponent<Elevator>().ReturnToStart();
		private void OpenEyesDelayed() => PlayerEffectController.OpenEyes(1f);

		private void DelayRadio()
        {
			SearchUtilities.Find("Elevator_Dialogue").SetActive(true);
			SearchUtilities.Find("Signal_Radio").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan").SetActive(true);
		}


		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
