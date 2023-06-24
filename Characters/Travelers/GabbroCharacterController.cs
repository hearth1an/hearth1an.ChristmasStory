using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace ChristmasStory.Characters.Travelers
{
	[HarmonyPatch]
	internal class GabbroCharacterController : TravelerCharacterController
	{

		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.GABBRO_DONE;

		private GameObject _gdShip, _thShip;
		private GameObject _signal;

		[HarmonyPostfix]
		[HarmonyPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.Start))]
		private static void PauseMenuManager_Start(PauseMenuManager __instance)
		{
			__instance._skipToNextLoopButton.SetActive(true);
		}
		public override void Start()
		{
			dialogue = SearchUtilities.Find("Gabbro_Dialogue_Tree").GetComponent<CharacterDialogueTree>();
			_signal = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Signal_Flute");
			originalCharacter = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro");
			_gdShip = SearchUtilities.Find("GabbroShip_Body");
			_thShip = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GabbroShip");
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro/Signal_Flute").transform.localPosition = new Vector3(0, 1, 0);
			base.Start();

			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

			if (Conditions.Get(Conditions.PERSISTENT.LEARN_MEDITATION))
			{
				Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			}

			ShipHandler.Instance.ShipExplosion.AddListener(ShipHandler_ShipExplosion);
		}

		

		public override void OnDestroy()
		{
			base.OnDestroy();
			ShipHandler.Instance?.ShipExplosion?.RemoveListener(ShipHandler_ShipExplosion);
		}

		private void ShipHandler_ShipExplosion()
		{
			if (ShipHandler.IsCharacterNearShip(originalCharacter, 30f))
			{
				WriteUtil.WriteDebug("Kaboom!");
				Conditions.Set(DoneCondition, true);
				StartCoroutine(KillGabbro());
			}
		}

		private IEnumerator KillGabbro()
		{
			// Disappear after 1.5 seconds
			yield return 1.5f;
			originalCharacter.SetActive(false);
			_signal?.SetActive(false);
			//dialogue?.gameObject?.SetActive(false);
			SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro").SetActive(false);
		}

		protected override void Dialogue_OnStartConversation() { }
        protected override void Dialogue_OnEndConversation()
        {       
            if (Conditions.Get(Conditions.PERSISTENT.LEARN_MEDITATION) && !PlayerData.GetPersistentCondition("MEDITATION_KNOWN")) 
			{
				PlayerEffectController.Blink(2f);
				PlayerEffectController.AddLock(2f);
				Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
				WriteUtil.WriteLine("Enabling meditation");
				PlayerData.SetPersistentCondition("MEDITATION_KNOWN", true);
			}	
		}

        protected override void OnChangeState(STATE oldState, STATE newState)
		{
			_gdShip?.SetActive(newState == STATE.ORIGINAL);
			_thShip?.SetActive(newState == STATE.AT_TREE);

			_signal?.SetActive(newState == STATE.ORIGINAL);
			dialogue?.gameObject?.SetActive(newState == STATE.ORIGINAL);
		}
	}
}
