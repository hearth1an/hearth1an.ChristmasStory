using System.Linq;
using UnityEngine;

namespace ChrismasStory.Components
{
	internal class HeldItemHandler : MonoBehaviour
	{
		private ToolModeSwapper _toolModeSwapper;
		private static HeldItemHandler _instance;
		private ItemTool _itemTool;

		public void Start()
		{
			_toolModeSwapper = GameObject.FindObjectOfType<ToolModeSwapper>();
			_instance = this;
			_itemTool = GameObject.FindObjectOfType<ItemTool>();
		}

		public static OWItem GetHeldItem() => _instance._toolModeSwapper.GetItemCarryTool().GetHeldItem();
		public static OWItem GetNullItem() => _instance._itemTool._heldItem = null;




		/// <summary>
		/// Returns true if it is the functioning warp core from the ATP
		/// </summary>
		/// <returns></returns>
		public static bool IsPlayerHoldingWarpCore() => GetHeldItem() is WarpCoreItem warpCore && warpCore._wcType == WarpCoreType.Vessel;

		public static bool IsPlayerHoldingStrangerArtifact() => GetHeldItem() is DreamLanternItem or VisionTorchItem;

		public static bool IsPlayerHoldingJunk() => GetHeldItem() is not WarpCoreItem or DreamLanternItem && GetNullItem();


        #region debug
        public static void GivePlayerWarpCore()
		{
			// Giving item breaks when you're flying the ship
			if (!PlayerState.AtFlightConsole())
			{
				var warpCore = FindObjectsOfType<WarpCoreItem>().First(x => x._wcType == WarpCoreType.Vessel);
				Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(warpCore);
			}
		}

		public static void GivePlayerLantern()
		{
			if (!PlayerState.AtFlightConsole())
			{
				var lantern = FindObjectOfType<DreamLanternItem>();
				Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(lantern);
			}
		}
		#endregion
	}
}
