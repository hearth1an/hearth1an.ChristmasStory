using ChristmasStory.Components;
using ChristmasStory.Components.Animation;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace ChristmasStory.Characters.Travelers
{
	internal class SolanumCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.SOLANUM_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/Interactables_EYEState/ConversationPivot/Character_NOM_Solanum/Nomai_ANIM_SkyWatching_Idle/ConversationZone").GetComponent<CharacterDialogueTree>();
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle");
			originalCharacter = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/Interactables_EYEState/ConversationPivot/Character_NOM_Solanum");

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle/Signal_Nomai").transform.localPosition = new Vector3(0, 2, 0);
			HeldItemHandler.Instance.ItemDropped.AddListener(OnItemDropped);

			base.Start();
			SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/QMResponseText").GetComponent<NomaiWallText>().Hide(1);
		}

		protected override void Dialogue_OnStartConversation()
		{
			var holdingInviteStone = HeldItemHandler.IsPlayerHoldingInviteStone();
			Conditions.Set(Conditions.CONDITION.HOLDING_INVITE_STONE, holdingInviteStone);
		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.SOLANUM_START))
			{
				SolanumAnimationController.Instance.SolanumAnimEvent();

				Conditions.Set(Conditions.CONDITION.SOLANUM_START, false);
				Conditions.Set(Conditions.CONDITION.SOLANUM_START_DONE, true);
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}

		private void OnItemDropped(OWItem item)
		{
			var distance = 100f;
			var withinDistance = (Locator.GetPlayerTransform().position - treeCharacter.transform.position).sqrMagnitude < distance * distance;
			if (item.name == "WordStone_You" && withinDistance)
			{
				ChangeState(STATE.AT_TREE);
			}
		}

		protected override IEnumerator DirectToTree(STATE state)
		{
			// Plays for 15.33s
			PlayerEffectController.PlayAudioOneShot(AudioType.MemoryUplink_Start, 1f);
			PlayerEffectController.PlayAudioOneShot(AudioType.EYE_QuantumFoamApproach, 1f);
			Locator.GetActiveCamera().transform.Find("ScreenEffects/LightFlickerEffectBubble").GetComponent<LightFlickerController>().FlickerOffAndOn(15,2);

			yield return new WaitForSeconds(15f);

			var oldInputMode = OWInput.GetInputMode();
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.CloseEyes(0.33f);
			yield return new WaitForSeconds(0.33f);

			// Eyes closed: swap character state
			OnSetState(state);

			// Open eyes
			PlayerEffectController.OpenEyes(0.33f);
			TransformController.ResetVillageSignals();

			OWInput.ChangeInputMode(oldInputMode);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}
	}
}
