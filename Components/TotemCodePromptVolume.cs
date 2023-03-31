using ChristmasStory.Utility;
using UnityEngine;
using NewHorizons.Handlers;

namespace ChristmasStory.Components
{
	internal class TotemCodePromptVolume : MonoBehaviour
	{
		private ScreenPrompt _codePrompt;
		private bool _visible;

		private Texture2D _promptTexture;

		public void Start()
		{
			_promptTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/other/promt.png");
			_visible = false;
		}

		public void OnTriggerEnter(Collider hitCollider)
		{
			if (hitCollider.attachedRigidbody == Locator.GetPlayerBody()._rigidbody)
			{
				WriteUtil.WriteLine("Entered totem prompt volume");
				PlayerData.SetPersistentCondition("TOTEM_KNOWN", true);
				string promtText = TranslationHandler.GetTranslation("TOTEM_CODE", TranslationHandler.TextType.UI);
				_codePrompt ??= PromptUtils.AddTexturePrompt(promtText + " <CMD>", PromptPosition.LowerLeft, _promptTexture);
				_visible = true;
			}
		}

		public void OnTriggerExit(Collider hitCollider)
		{
			if (hitCollider.attachedRigidbody == Locator.GetPlayerBody()._rigidbody)
			{				
				_visible = false;
			}
		}

		public void Update()
		{
			_codePrompt?.SetVisibility(_visible);
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
