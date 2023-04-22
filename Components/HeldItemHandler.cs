using ChristmasStory.Utility;
using HarmonyLib;
using NewHorizons.Utility;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ChristmasStory.Components
{
	[HarmonyPatch]
	internal class HeldItemHandler : MonoBehaviour
	{
		private ToolModeSwapper _toolModeSwapper;
		private ItemTool _itemTool;
		public GameObject _sharedStone;
		private GameObject _villageSector;
		public DreamLanternController PrisonerLantern { get; private set; }
		public DreamLanternItem PrisonerLanternItem { get; private set; }

		public class ItemEvent : UnityEvent<OWItem> { }
		public ItemEvent ItemDropped = new();
		public ItemEvent ItemUnderWater = new();
		public static HeldItemHandler Instance;

		private bool isPrisonerFailed = false;

		public void Start()
		{
			Instance = this;
			_toolModeSwapper = FindObjectOfType<ToolModeSwapper>();
			_itemTool = FindObjectOfType<ItemTool>();
			_sharedStone = SearchUtilities.Find("Invite_Stone");

			_villageSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle");

			// Fix prisoner lantern
			PrisonerLantern = SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternController>();
			PrisonerLanternItem = PrisonerLantern.GetComponent<DreamLanternItem>();
			
		}
		public void PrisonerFailed()
		{
			PrisonerLantern._lit = false;
			PrisonerLantern.UpdateVisuals();

			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_IP_ANIM").SetActive(false);
			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Prisoner_Lantern").SetActive(false);
			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP").SetActive(false);
			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base").SetActive(false);
			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/LanternTableSocket").SetActive(false);
			SearchUtilities.Find("Prefab_IP_GhostBird_Prisoner").SetActive(false);
			SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostDirector_Prisoner/PrisonerTrigger_Emerge").SetActive(false);

			PlayerEffectController.PlayAudioOneShot(AudioType.Artifact_Extinguish, 1f);
			PlayerEffectController.PlayAudioOneShot(AudioType.Ghost_DeathSingle, 2f);
			WriteUtil.WriteDebug("Prisoner failed. Try next loop.");
		}
		public static OWItem GetHeldItem() => Instance._toolModeSwapper.GetItemCarryTool().GetHeldItem();

		/// <summary>
		/// Returns true if it is the functioning warp core from the ATP
		/// </summary>
		/// <returns></returns>
		public static bool IsPlayerHoldingWarpCore() => GetHeldItem() is WarpCoreItem warpCore && warpCore._wcType == WarpCoreType.Vessel;

		public static bool IsPlayerHoldingStrangerArtifact() => GetHeldItem() is DreamLanternItem or VisionTorchItem;

		public static bool IsPlayerHoldingPrisonerArtifact() => GetHeldItem()?.name == "Prisoner_Artifact";

		public static bool IsPlayerHoldingJunk() => GetHeldItem() is not WarpCoreItem or DreamLanternItem or VisionTorchItem or null;

		public static bool IsPlayerHoldingInviteStone() => GetHeldItem()?.name == "Invite_Stone";

		private void Update()
		{
			if (IsPlayerHoldingPrisonerArtifact() && PrisonerLantern._lit == true)
			{
				if (PlayerState._isCameraUnderwater == true && !isPrisonerFailed)
				{
					PrisonerFailed();
					isPrisonerFailed = true;
				}
			}
			else return;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(OWItem), nameof(OWItem.DropItem))]
		private static void OWItem_DropItem()
		{
			Instance?.ItemDropped?.Invoke(GetHeldItem());
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(DreamLanternItem), nameof(DreamLanternItem.OnEnterDreamWorld))]
		[HarmonyPatch(typeof(DreamLanternItem), nameof(DreamLanternItem.OnExitDreamWorld))]
		[HarmonyPatch(typeof(DreamLanternItem), nameof(DreamLanternItem.OnEnterFluidType))]
		private static bool PrisonerLanternPatch(DreamLanternItem __instance) => __instance != Instance.PrisonerLanternItem;

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

		public static void GivePlayerInviteStone()
		{
			var inviteStone = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Invite_Stone").GetComponent<InviteStone>();
			Locator.GetToolModeSwapper().GetItemCarryTool().PickUpItemInstantly(inviteStone);
		}
		#endregion

	}
}
