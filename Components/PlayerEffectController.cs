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

        public static void Blink(float length = 2)
        {
            _instance._cameraEffectController.CloseEyes(length / 2f);
            _instance._cameraEffectController.OpenEyes(length / 2f, false);
        }
    }
}
