using ChristmasStory.Components;
using ChristmasStory.Utility;
using System.Collections;
using UnityEngine;

namespace ChristmasStory.Characters
{
	internal abstract class BaseCharacterController : MonoBehaviour
	{
		public GameObject originalCharacter, shipCharacter, treeCharacter;

		public enum STATE
		{
			NONE,
			ORIGINAL,
			ON_SHIP,
			AT_TREE
		};

		public STATE State { get; private set; }

		public abstract Conditions.PERSISTENT DoneCondition { get; }

		public virtual void Start()
		{
			if (Conditions.Get(DoneCondition))
			{
				ChangeState(STATE.AT_TREE, false);
			}
			else
			{
				ChangeState(STATE.ORIGINAL, false);
			}
		}

		public void ChangeState(STATE newState, bool blink = true)
		{
			if (State != newState)
			{
				// Blink for 2 seconds means 1 second to close eyes then 1 second to open
				// Right in the middle we change the state

				if (blink)
				{
					if (State == STATE.ORIGINAL && newState == STATE.AT_TREE)
					{
						// If going from original position to tree, they are taking their own ship
						StartCoroutine(DirectToTree(newState));
					}
					else if (State == STATE.ORIGINAL && newState == STATE.ON_SHIP)
					{
						// From original position into the ship they are walking to the ship and into the hatch
						StartCoroutine(ShipCoroutine(newState));
					}
					else if (State == STATE.ON_SHIP && newState == STATE.AT_TREE)
					{
						// Exit hatch then walk
						StartCoroutine(ShipCoroutine(newState));
					}
					else
					{
						// This shouldn't happen but just in case
						StartCoroutine(ChangeStateCoroutine(2f, newState));
					}
				}
				else
				{
					OnSetState(newState);
				}


				OnChangeState(State, newState);

				State = newState;

				if (State == STATE.AT_TREE)
				{
					// Now that they are done we set it as a persistent condition
					Conditions.Set(DoneCondition, true);
				}
			}
		}

		protected abstract void OnChangeState(STATE oldState, STATE newState);

		private IEnumerator ShipCoroutine(STATE state)
		{
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.CloseEyes(0.7f);
			yield return new WaitForSeconds(0.7f);

			// Eyes closed: swap character state
			OnSetState(state);

			PlayerEffectController.PlayAudioOneShot(AudioType.ShipHatchOpen, 0.3f);
			yield return new WaitForSeconds(1f);

			PlayerEffectController.PlayAudioOneShot(AudioType.ShipHatchClose, 0.3f);

			yield return new WaitForSeconds(0.3f);

			// Open eyes
			PlayerEffectController.OpenEyes(0.7f);

			OWInput.ChangeInputMode(InputMode.Character);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}

		protected virtual IEnumerator DirectToTree(STATE state)
		{
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.CloseEyes(1f);
			yield return new WaitForSeconds(1f);

			// Eyes closed: swap character state
			OnSetState(state);

			// Play ship takeoff sound
			PlayerEffectController.PlayAudioOneShot(AudioType.ShipThrustIgnition, 0.3f);
			TransformController.ResetVillageSignals();

			yield return new WaitForSeconds(1f);

			// Open eyes
			PlayerEffectController.OpenEyes(1f);

			OWInput.ChangeInputMode(InputMode.Character);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}

		private IEnumerator ChangeStateCoroutine(float wait, STATE state)
		{
			OWInput.ChangeInputMode(InputMode.None);
			Locator.GetPauseCommandListener().AddPauseCommandLock();

			PlayerEffectController.Blink(2);
			TransformController.ResetVillageSignals();

			yield return new WaitForSeconds(wait);

			OnSetState(state);

			OWInput.ChangeInputMode(InputMode.Character);
			Locator.GetPauseCommandListener().RemovePauseCommandLock();
		}

		protected void OnSetState(STATE state)
		{
			originalCharacter?.SetActive(state == STATE.ORIGINAL);
			shipCharacter?.SetActive(state == STATE.ON_SHIP);
			treeCharacter?.SetActive(state == STATE.AT_TREE);
		}

		

	}
}
