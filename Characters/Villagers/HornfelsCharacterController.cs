using ChristmasStory.Utility;
using NewHorizons.Utility;
using ChristmasStory.Components;


namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class HornfelsCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.HORNFELS_FISH_TOLD;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final/Hornfels_Dialogue_Final").GetComponent<CharacterDialogueTree>();

			//originalCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village");			

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.HORNFELS_FISH_TOLD))
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/Ernesto_Dialogue").SetActive(true);

			}
			if (Conditions.Get(Conditions.CONDITION.START_END_EVENT))
			{		
				PlayerEffectController.CloseEyes(3);
				PlayerEffectController.OpenEyes(4);

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Signal_Prisoner").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/Signal_Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/Signal_Banjo").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Esker").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Signal_Drums").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro/Signal_Flute").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle/Signal_Nomai").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final/Hornfels_Dialogue_Final").SetActive(false);

				EndGameController.Instance.Invoke("StartErnestoShine", 2f);
			}
		}		
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
