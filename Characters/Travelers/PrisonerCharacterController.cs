using ChrismasStory.Components;
using ChristmasStory.Components.Animation;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace ChrismasStory.Characters.Travelers
{
	
	internal class PrisonerCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.PRISONER_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("Prisoner_Dialogue").GetComponent<CharacterDialogueTree>();
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird");
			originalCharacter = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM");
			
			
			HeldItemHandler.Instance.ItemDropped.AddListener(OnItemDropped);

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

				Conditions.Set(Conditions.CONDITION.PRISONER_START, false);
				Conditions.Set(Conditions.CONDITION.PRISONER_START_DONE, true);
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}

		private void OnItemDropped(OWItem item)
		{
			var distance = 100f;
			var withinDistance = (Locator.GetPlayerTransform().position - treeCharacter.transform.position).sqrMagnitude < distance * distance;
			if (item.name == "Prisoner_Artifact" && withinDistance)
			{
				ChangeState(STATE.AT_TREE);
			}
		}

		protected override IEnumerator DirectToTree(STATE state)
		{
			// Plays for 15.33s
			PlayerEffectController.PlayAudioOneShot(AudioType.Ghost_Laugh, 1f);

			yield return new WaitForSeconds(2f);

			var oldInputMode = OWInput.GetInputMode();
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.CloseEyes(0.33f);
			yield return new WaitForSeconds(0.33f);

			// Eyes closed: swap character state
			OnSetState(state);

			// Open eyes
			PlayerEffectController.OpenEyes(0.33f);

			OWInput.ChangeInputMode(oldInputMode);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}
	}
}
