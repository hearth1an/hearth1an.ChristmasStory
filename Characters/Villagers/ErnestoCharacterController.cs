using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class ErnestoCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.ERNESTO_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/Ernesto_Dialogue").GetComponent<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto").SetActive(false);
			//originalCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village");			

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.PERSISTENT.ERNESTO_DONE))
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/Ernesto_Dialogue").SetActive(false);
				PlayerEffectController.Blink(4f);
				Invoke("EnableErnesto", 2f);

			}

		}

		private void EnableErnesto()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Exhibits/Exhibits_Glass").SetActive(false);
			PlayerEffectController.PlayAudioOneShot(AudioType.ShipDamageCockpitGlassCrack, 1f);

			var ernestoLight = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Props_HEA_WallLamp_Pulsing 1/Ernesto_Light").GetComponent<PulsingLight>();
			ernestoLight._initLightRange = 20f;


			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto").SetActive(true);


		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
