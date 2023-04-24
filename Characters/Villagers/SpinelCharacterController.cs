using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	
	internal class SpinelCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.ERNESTO_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Spinel_Dialogue").GetComponent<CharacterDialogueTree>();
				
			SearchUtilities.Find("New_Spinel").SetActive(false);

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.SPINEL_TOLD))
            {
                PlayerEffectController.CloseEyes(1f);
				PlayerEffectController.AddLock(2f);

				var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/jump.mp3");
                PlayerEffectController.PlayAudioExternalOneShot(sfx, 3f);

                Invoke("ChangePosition", 3f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Spinel_Dialogue").SetActive(false);
			}
		}

		private void ChangePosition()
        {
			PlayerEffectController.OpenEyes(1f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel").SetActive(false);
			SearchUtilities.Find("New_Spinel").SetActive(true);
		}

		
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
