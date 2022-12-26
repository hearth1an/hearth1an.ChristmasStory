using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
