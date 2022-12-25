using System.Linq;
using UnityEngine;
using NewHorizons.Utility;
using UnityEngine.Events;

namespace ChrismasStory.Components
{
	internal class HeldItemHandler : MonoBehaviour
	{
		private ToolModeSwapper _toolModeSwapper;
		private static HeldItemHandler _instance;
		private ItemTool _itemTool;
		public SharedStone _sharedStone;
		private NomaiConversationStone _nomaiConversationStone;
		private GameObject _villageSector;

		public UnityEvent BringItem { get; private set; }

		public static HeldItemHandler Instance;

		public void Start()
		{
			BringItem = new();
			Instance = this;
			_toolModeSwapper = GameObject.FindObjectOfType<ToolModeSwapper>();
			_instance = this;
			_itemTool = GameObject.FindObjectOfType<ItemTool>();
            _sharedStone = GameObject.FindObjectsOfType<SharedStone>().First(x => x.name == "Invite_Stone");
			_nomaiConversationStone = GameObject.FindObjectOfType<NomaiConversationStone>();
			_villageSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle");
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

		public static bool IsPlayerHoldingInviteStone() => GetHeldItem() == _instance._sharedStone;

		public static bool IsCharacterNearVillage(float distance)
		{
			return (Instance._nomaiConversationStone.transform.position - Instance._villageSector.transform.position).sqrMagnitude < distance * distance;
		}

		private static void BringItem_Done()
		{
			Instance.BringItem.Invoke();
		}

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
