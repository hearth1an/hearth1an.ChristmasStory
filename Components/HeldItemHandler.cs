using HarmonyLib;
using NewHorizons.Utility;
using OWML.ModHelper;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

namespace ChrismasStory.Components
{
	[HarmonyPatch]
	internal class HeldItemHandler : MonoBehaviour
	{
		private ToolModeSwapper _toolModeSwapper;
		private static HeldItemHandler _instance;
		private ItemTool _itemTool;
		public SharedStone _sharedStone;
		private GameObject _villageSector;
		public DreamLanternController PrisonerLantern { get; private set; }
		public DreamLanternItem PrisonerLanternItem { get; private set; }

		private OWTriggerVolume _bellInterior;
		private EntrywayTrigger _bellEntry;

		public class ItemEvent : UnityEvent<OWItem> { }
		public ItemEvent ItemDropped = new();

		public static HeldItemHandler Instance;

		public void Start()
		{
			Instance = this;
			_toolModeSwapper = GameObject.FindObjectOfType<ToolModeSwapper>();
			_instance = this;
			_itemTool = GameObject.FindObjectOfType<ItemTool>();
			_sharedStone = GameObject.FindObjectsOfType<SharedStone>().First(x => x.name == "Invite_Stone");

			_villageSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle");

			// Fix prisoner lantern
			PrisonerLantern = SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternController>();
			PrisonerLanternItem = PrisonerLantern.GetComponent<DreamLanternItem>();

			PrisonerLanternItem._fluidDetector.OnEnterFluidType += PrisonerLanternItem_EnterFluidType;
			PrisonerLanternItem._fluidDetector._shape.SetActivation(true);

			PrisonerLanternItem.onPickedUp.AddListener(PrisonerLanternItem_OnPickedUp);

			// Add the lantern to the right volumes
			Locator.GetRingWorldController()._insideRingWorldVolume.AddObjectToVolume(PrisonerLanternItem._fluidDetector.gameObject);

			// Fix appearance
			Delay.FireOnNextUpdate(() =>
			{
				PrisonerLantern.enabled = true;
				PrisonerLantern.SetLit(true);
				PrisonerLantern._focus = 100f;
				PrisonerLantern.UpdateVisuals();
			});

			_bellInterior = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/PrisonInteriorVolume")
				.GetComponent<OWTriggerVolume>();

			_bellEntry = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/PrisonInteriorVolume/Entryway")
				.GetComponent<EntrywayTrigger>();
		}

		private static void PrisonerLanternItem_EnterFluidType(FluidVolume.Type fluidType)
		{
			ChristmasStory.WriteDebug($"Prisoner artifact entered fluid [{fluidType}]");
			if (Instance.PrisonerLantern.IsLit() && fluidType == FluidVolume.Type.WATER)
			{
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_IP_ANIM").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Prisoner_Lantern").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP").SetActive(false);

				Instance.PrisonerLantern.SetLit(false);

				PlayerEffectController.PlayAudioOneShot(AudioType.Ghost_DeathSingle, 0.5f);
			}
		}

		private static void PrisonerLanternItem_OnPickedUp(OWItem item)
		{
			var isPlayerInsideBell = Instance._bellInterior.IsTrackingObject(Locator.GetPlayerDetector());

			ChristmasStory.WriteDebug($"Is player inside bell: [{isPlayerInsideBell}]");

			// If we pick it up from outside the bell we have to update the volumes
			if (!isPlayerInsideBell)
			{
				Instance._bellInterior.RemoveObjectFromVolume(Instance.PrisonerLanternItem._fluidDetector.gameObject);
				Instance._bellEntry.OnShapeExit(Instance.PrisonerLanternItem._fluidDetector._shape);
			}
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
		#endregion
	}
}
