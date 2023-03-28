using ChrismasStory.Components;
using ChristmasStory.Components.Animation;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System;
using System.Collections;
using UnityEngine;

namespace ChrismasStory.Characters.Travelers
{

	internal class PrisonerCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.PRISONER_DONE;
		public static PrisonerCharacterController Instance;
		private GameObject _lantern;		

		public override void Start()
		{
			Instance = this;
			dialogue = SearchUtilities.Find("Prisoner_Dialogue").GetComponent<CharacterDialogueTree>();
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird");
			originalCharacter = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM");
			_lantern = SearchUtilities.Find("Village_Lantern");

			

			//SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/New_Vision_Torch/VisionBeam").SetActive(true);

			var controller = _lantern.GetComponent<DreamLanternController>();
			controller.enabled = true;
			controller.SetLit(true);
			controller.SetHeldByPlayer(false);
			//controller._flameStrength = 100f;
			controller.UpdateVisuals();
			controller.GetComponent<DreamLanternItem>().EnableInteraction(false);

			ChristmasStory.Instance.ModHelper.Events.Unity.RunWhen(() => Locator.GetRingWorldController()._damBroken, () =>
			{
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/UnderwaterAudioVolume_Prison").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Effects_IP_PrisonWater").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Effects_Prison").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/WaterVolume_Prison").SetActive(false);
			});

			//Delay.FireOnNextUpdate(() => controller.transform.Find("Spotlight_Lantern").GetComponent<OWLight2>()._intensityScale = 0.2f);			

			HeldItemHandler.Instance.ItemDropped.AddListener(OnItemDropped);

			ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				originalCharacter.SetActive(true);
				if (Conditions.Get(Conditions.PERSISTENT.PRISONER_DONE))
				{
					SearchUtilities.Find("Prisoner_Artifact").SetActive(false);
					SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner").SetActive(false); ;
					SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostDirector_Prisoner").SetActive(false); 
					SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/LanternTableSocket").SetActive(false);
					SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Effects_PrisonCell/DarknessPlane").SetActive(false);
					ChristmasStory.WriteLine("Prisoner is done, disabling them in DreamWorld");
				}
			});
			

			base.Start();

		}
		protected override void Dialogue_OnStartConversation()
		{

		}

		
		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.PRISONER_START))
			{

				PrisonerAnimationController.Instance.PlayLightsUp();

				SearchUtilities.Find("Prisoner_Dialogue").SetActive(false);

				Conditions.Set(Conditions.CONDITION.PRISONER_START, false);
				Conditions.Set(Conditions.CONDITION.PRISONER_START_DONE, true);
			}
		}
		protected override void OnChangeState(STATE oldState, STATE newState)
		{
			originalCharacter.SetActive(newState == STATE.AT_TREE);
		}
		private void OnItemDropped(OWItem item)
		{
			if (item == HeldItemHandler.Instance.PrisonerLanternItem)
			{
				var distance = 100f;
				var lit = HeldItemHandler.Instance.PrisonerLantern._lit == true;
				var withinDistance = (Locator.GetPlayerTransform().position - treeCharacter.transform.position).sqrMagnitude < distance * distance;
				if (withinDistance && lit)
				{
					ChangeState(STATE.AT_TREE);
				}
			}
		}		

		protected override IEnumerator DirectToTree(STATE state)
		{
			PlayerEffectController.PlayAudioOneShot(AudioType.Ghost_Laugh, 2f);

			yield return new WaitForSeconds(2f);

			var oldInputMode = OWInput.GetInputMode();
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.CloseEyes(0.33f);
			yield return new WaitForSeconds(0.33f);

			HeldItemHandler.Instance.PrisonerLantern.gameObject.SetActive(false);

			// Eyes closed: swap character state
			OnSetState(state);

			// Open eyes
			PlayerEffectController.OpenEyes(0.33f);

			OWInput.ChangeInputMode(oldInputMode);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}		
	}
}

