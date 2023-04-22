using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{	
	internal class MarlCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.MARL_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").GetComponent<CharacterDialogueTree>();
			
            base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.TEKTITE_ASK_MARL))
			{				
				PlayerEffectController.Blink(4f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(false);
				Invoke("SwapCharacter", 2f);
			}
		}

		private void SwapCharacter()
		{
			var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/marl_go.mp3");
			PlayerEffectController.PlayAudioExternalOneShot(sfx, 3f);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2").transform.localRotation = new UnityEngine.Quaternion(0.05f, 0.1109f, 0.0867f, -0.9888f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Marl").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Base").SetActive(false);
		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
