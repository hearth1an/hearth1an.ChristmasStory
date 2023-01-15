using ChrismasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
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
				PlayerEffectController.CloseEyes(2f);
				Invoke("AddShine_1", 10f);
				Invoke("AddShine_2", 19f);
				Invoke("AddShine_3", 35f);
			}
		}

		private void AddShine_1()
        {
			var ernestoLight = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Props_HEA_WallLamp_Pulsing 1/Ernesto_Light").GetComponent<PulsingLight>();
			ernestoLight._initLightRange = 100f;
			ChristmasStory.Instance.ModHelper.Console.WriteLine("Increase 1");
		}
		private void AddShine_2()
		{
			var ernestoLight = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Props_HEA_WallLamp_Pulsing 1/Ernesto_Light").GetComponent<PulsingLight>();
			ernestoLight._initLightRange = 200f;
			ChristmasStory.Instance.ModHelper.Console.WriteLine("Increase 2");
		}

		private void AddShine_3()
		{
			var ernestoLight = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Props_HEA_WallLamp_Pulsing 1/Ernesto_Light").GetComponent<PulsingLight>();
			ernestoLight._initLightRange = 300f;
			SearchUtilities.Find("Sun_Body/Sector_SUN/Effects_SUN/Supernova").GetComponent<SupernovaEffectController>().enabled = true;
			ChristmasStory.Instance.ModHelper.Console.WriteLine("Starting supernova");
		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
