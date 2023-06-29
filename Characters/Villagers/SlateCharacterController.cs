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
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/Slate_Dialogue_Main").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village");
			originalCharacter.SetActive(true);
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final");
			base.Start();

			if (Conditions.Get(Conditions.PERSISTENT.SLATE_START_DONE))
			{
				Invoke("SpawnPreEndGameProps", 1);
				WriteUtil.WriteLine("Spawning endgame props");

			}
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.PERSISTENT.SLATE_START_DONE) && Conditions.Get(Conditions.CONDITION.SLATE_DO_NOT_START_IMMIDIATE))
			{
				PlayerEffectController.Blink(5f);
				PlayerEffectController.AddLock(5f);
				PlayerEffectController.PlayAudioOneShot(AudioType.PlayerGasp_Light, 1f);
				Invoke("SpawnEndGameProps", 2f);
				Invoke("SpawnErnestoLight", 2f);
			}
		}
		public void SpawnPreEndGameProps()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village").SetActive(true);
		}

		public void SpawnEndGameProps()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village").SetActive(false);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final").SetActive(true);

			ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				var ernesto = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/AnglerFishTankPivot/Beast_Anglerfish/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Lure_PointLight_FogLight").GetComponent<Light>();
				ernesto.intensity = 5f;
				ernesto.range = 5f;
			});
			

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").transform.localRotation = new Quaternion(-0.0104f, -0.0329f, 0.0209f, 0.9992f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue_Final").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(false);
		}

		public void SpawnErnestoLight()
		{	
			ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				var ernesto = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/AnglerFishTankPivot/Beast_Anglerfish/B_angler_root/B_angler_body01/B_angler_body02/B_angler_antenna01/B_angler_antenna02/B_angler_antenna03/B_angler_antenna04/B_angler_antenna05/B_angler_antenna06/B_angler_antenna07/B_angler_antenna08/B_angler_antenna09/B_angler_antenna10/B_angler_antenna11/B_angler_antenna12_end/Lure_PointLight_FogLight");
				ernesto.AddComponent<LightFlicker>()._range = 1f;
				
			});
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}