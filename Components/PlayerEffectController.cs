using System.Collections;
using UnityEngine;

namespace ChrismasStory.Components
{
    internal class PlayerEffectController : MonoBehaviour
    {
        private PlayerCameraEffectController _cameraEffectController;

        private static PlayerEffectController _instance;

        private void Start()
        {
			_instance = this;
			_cameraEffectController = Object.FindObjectOfType<PlayerCameraEffectController>();
        }

        public static void Blink(float length = 2) => _instance.StartCoroutine(_instance.BlinkCoroutine(length));
        public static void CloseEyes(float length) => _instance._cameraEffectController.CloseEyes(length);
        public static void OpenEyes(float length) => _instance._cameraEffectController.OpenEyes(length, false);

		private IEnumerator BlinkCoroutine(float length)
        {
			CloseEyes(length / 2f);
			yield return new WaitForSeconds(length / 2f);
			OpenEyes(length / 2f);
		}

        public static void PlayAudioOneShot(AudioType audio, float volume = 1f) => Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(audio, volume);
	}
}
