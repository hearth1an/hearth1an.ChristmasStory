using System.Linq;
using UnityEngine;

namespace ChrismasStory.Components
{
	internal class HeldItemHandler : MonoBehaviour
	{
		private ToolModeSwapper _toolModeSwapper;
		private static HeldItemHandler _instance;

		private void Start()
		{
			_toolModeSwapper = GameObject.FindObjectOfType<ToolModeSwapper>();
			_instance = this;
		}

		public static OWItem GetHeldItem() => _instance._toolModeSwapper.GetItemCarryTool().GetHeldItem();

		/// <summary>
		/// Returns true if it is the functioning warp core from the ATP
		/// </summary>
		/// <returns></returns>
		public static bool IsPlayerHoldingWarpCore() => GetHeldItem() is WarpCoreItem warpCore && warpCore._wcType == WarpCoreType.Vessel;

		public static bool IsPlayerHoldingStrangerArtifact() => GetHeldItem() is DreamLanternItem or VisionTorchItem;

		public static void GivePlayerWarpCore()
		{
			var warpCore = FindObjectsOfType<WarpCoreItem>().First(x => x._wcType == WarpCoreType.Vessel);
			Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(warpCore);
		}

		public static void GivePlayerLantern()
		{
			var lantern = FindObjectOfType<DreamLanternItem>();
			Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(lantern);
		}
	}
}
