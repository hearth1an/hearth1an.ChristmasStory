using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

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

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff/Villager_HEA_Tuff_ANIM_Mine").AddComponent<FacePlayerWhenTalking>();

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
				PlayerEffectController.CloseEyes(3f);				
				SearchUtilities.Find("Elevator_Dialogue").SetActive(false);
				Invoke("AfterDialogue", 1f);
				Invoke("DelayRadio", 30f);
			}
			if (Conditions.Get(Conditions.CONDITION.ELEVATOR_DONE))
			{
				SearchUtilities.Find("Elevator_Dialogue").SetActive(false);
				// For lift down
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator").GetComponent<Elevator>().AttachPlayerAndStartLift();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator/AttachPoint_MineElevator").GetComponent<PlayerAttachPoint>().DetachPlayer();
			}

		}

        private void AfterDialogue()
        {
			PlayerEffectController.OpenEyes(3f);
			var pickAxe = SearchUtilities.Find("Tuff_Pickaxe");
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ZeroGCave/Characters_ZeroGCave/Villager_HEA_Tuff").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_UpperVillage/Characters_UpperVillage/Villager_HEA_Gossan").SetActive(false);

			/*
            pickAxe.AddComponent<OWRigidbody>();
            var field = SearchUtilities.Find("TimberHearth_Body/FieldDetector_TH").GetComponent<ConstantForceDetector>();
            var gravity = SearchUtilities.Find("TimberHearth_Body/GravityWell_TH").GetComponent<GravityVolume>();
			pickAxe.GetComponent<OWRigidbody>().gra

			pickAxe.GetComponent<OWRigidbody>()._attachedForceDetector = field;
            pickAxe.GetComponent<OWRigidbody>()._attachedGravityVolume = gravity;
			*/

			
			pickAxe.AddComponent<NewHorizons.Components.AddPhysics>();
			pickAxe.SetActive(true);
			PlayerEffectController.PlayAudioOneShot(AudioType.CageElevator_Start);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator").GetComponent<Elevator>().ReturnToStart();			
		}

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
