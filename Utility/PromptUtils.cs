using System;
using UnityEngine.InputSystem;
using UnityEngine;


namespace ChristmasStory.Utility
{
	internal static class PromptUtils
	{
		public static ScreenPrompt AddPrompt(string text, PromptPosition position, Key key)
		{
			Enum.TryParse(key.ToString().Replace("Digit", "Alpha").Replace("Numpad", "Alpha"), out KeyCode keyCode);

			return AddPrompt(text, position, keyCode);			
		}

		public static ScreenPrompt AddPrompt(string text, PromptPosition position, KeyCode keyCode)
		{
			var texture = ButtonPromptLibrary.SharedInstance.GetButtonTexture(keyCode);
			var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);
			sprite.name = texture.name;

			var prompt = new ScreenPrompt(text, sprite);
			Locator.GetPromptManager().AddScreenPrompt(prompt, position, false);

			return prompt;
		}

		public static ScreenPrompt AddTexturePrompt(string text, PromptPosition position, Texture2D texture)
		{
			var addTexture = texture;
			var sprite = Sprite.Create(addTexture, new Rect(0, 0, addTexture.width, addTexture.height), new Vector2(0.5f, 0.5f), 70, 0, SpriteMeshType.FullRect, Vector4.zero, false);
			sprite.name = addTexture.name;

			var prompt = new ScreenPrompt(text, sprite);
			Locator.GetPromptManager().AddScreenPrompt(prompt, position, true);
			Locator.GetPromptManager().TriggerVisibilityRefresh(prompt);

			return prompt;
		}



	}
}
