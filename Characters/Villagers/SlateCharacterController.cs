using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using UnityEngine;

namespace ChristmasStory.Characters.Villagers
{
	internal class SlateCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.SLATE_START_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("TimberHearth_Body/Slate_Village");
			originalCharacter.SetActive(true);
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Slate_Village_Final");
			base.Start();

			if (Conditions.Get(Conditions.PERSISTENT.SLATE_START_DONE))
			{
				SpawnEndGameProps();
			}
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.PERSISTENT.SLATE_START_DONE))
			{
				PlayerEffectController.Blink(5f);
				PlayerEffectController.AddLock(5f);
				PlayerEffectController.PlayAudioOneShot(AudioType.PlayerGasp_Light, 1f);
				Invoke("SpawnEndGameProps", 2f);
			}
		}

		public void SpawnEndGameProps()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village").SetActive(false);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final").SetActive(true);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").transform.localRotation = new Quaternion(-0.0104f, -0.0329f, 0.0209f, 0.9992f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(false);
		}
		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}
