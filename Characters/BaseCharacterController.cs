using ChrismasStory.Components;
using System.Collections;
using UnityEngine;

namespace ChrismasStory.Characters
{
	internal class BaseCharacterController : MonoBehaviour
	{
		public GameObject originalCharacter, shipCharacter, treeCharacter;

		public enum STATE
		{
			ORIGINAL,
			ON_SHIP,
			AT_TREE
		}

		public void ChangeState(STATE state)
		{
			// Blink for 2 seconds means 1 second to close eyes then 1 second to open
			// Right in the middle we change the state

			PlayerEffectController.Blink(2);
			StartCoroutine(ChangeStateCoroutine(1, state));
		}

		private IEnumerator ChangeStateCoroutine(float wait, STATE state)
		{
			yield return new WaitForSeconds(wait);
			originalCharacter?.SetActive(state == STATE.ORIGINAL);
			shipCharacter?.SetActive(state == STATE.ON_SHIP);
			treeCharacter?.SetActive(state == STATE.AT_TREE);
		}
	}
}
