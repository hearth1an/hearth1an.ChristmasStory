using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace ChristmasStory.Characters.Travelers
{	
	internal class GabbroCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.GABBRO_DONE;

		private GameObject _gdShip, _thShip;
		private GameObject _signal;		
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

			if (Conditions.Get(Conditions.PERSISTENT.GABBRO_DONE))
            {
				SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/ConversationZone_Gabbro").SetActive(false);
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
			SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/ConversationZone_Gabbro").SetActive(false);
		}

		protected override void Dialogue_OnStartConversation() { }
        protected override void Dialogue_OnEndConversation()
        {       
            if (Conditions.Get(Conditions.PERSISTENT.LEARN_MEDITATION) && !PlayerData.GetPersistentCondition("KNOWS_MEDITATION")) 
			{
				PlayerEffectController.Blink(2f);
				PlayerEffectController.AddLock(2f);
				PlayerData.SetPersistentCondition("KNOWS_MEDITATION", true);
				Locator.GetSceneMenuManager()._pauseMenu._skipToNextLoopButton.SetActive(true);

				WriteUtil.WriteLine("Enabling meditation");				
			}	
		}

        protected override void OnChangeState(STATE oldState, STATE newState)
		{
			_gdShip?.SetActive(newState == STATE.ORIGINAL);
			_thShip?.SetActive(newState == STATE.AT_TREE);

			_signal?.SetActive(newState == STATE.ORIGINAL);
			//dialogue?.gameObject?.SetActive(newState == STATE.ORIGINAL);
		}
	}
}
