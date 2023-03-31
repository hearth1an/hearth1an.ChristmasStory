using ChristmasStory.Utility;
using UnityEngine;

namespace ChristmasStory.Components
{
	internal class TotemCodePromptVolume : MonoBehaviour
	{
		private ScreenPrompt _codePrompt;

		public void Start()
		{
			var promtTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/other/promt.png");
			_codePrompt = PromptUtils.AddTexturePrompt("Code: <CMD>", PromptPosition.LowerLeft, promtTexture);
			ShowPrompt(false);
		}

		public void OnTriggerEnter(Collider hitCollider)
		{
			if (hitCollider.attachedRigidbody == Locator.GetPlayerBody()._rigidbody)
			{
				ChristmasStory.Instance.ModHelper.Console.WriteLine("Entered totem prompt volume");
				ShowPrompt(true);
			}
		}

		public void OnTriggerExit(Collider hitCollider)
		{
			if (hitCollider.attachedRigidbody == Locator.GetPlayerBody()._rigidbody)
			{
				ChristmasStory.Instance.ModHelper.Console.WriteLine("Exited totem prompt volume");
				ShowPrompt(false);
			}
		}

		private void ShowPrompt(bool visible)
		{
			_codePrompt.SetVisibility(visible);
		}

		public static TotemCodePromptVolume Create(GameObject parent, Vector3 position, float radius)
		{
			var volume = new GameObject(nameof(TotemCodePromptVolume));
			volume.transform.parent = parent.transform;
			volume.transform.localPosition = position;
			volume.layer = LayerMask.NameToLayer("BasicEffectVolume");

			var sphere = volume.AddComponent<SphereCollider>();
			sphere.isTrigger = true;
			sphere.radius = radius;

			var completionVolume = volume.AddComponent<TotemCodePromptVolume>();

			return completionVolume;
		}
	}
}
