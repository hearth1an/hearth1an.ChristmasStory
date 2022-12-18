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

        public static void Blink()
        {

            var time = 2f;
            _instance._cameraEffectController.CloseEyes(time / 2f);
            _instance._cameraEffectController.OpenEyes(time / 2f, false);
        }
    }
}
