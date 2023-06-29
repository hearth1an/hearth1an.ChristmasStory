using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Travelers
{	
	internal class FeldsparCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.FELDSPAR_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueShip = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();
			dialogueVillage = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/ConversationZone").GetComponent<CharacterDialogueTree>();

			originalCharacter = SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar");

			// Disabled before entry
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_2").SetActive(false);
			SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(false);

			base.Start();

			originalCharacter.SetActive(false);

			ChristmasStory.Instance.ModHelper.Events.Unity.RunWhen(() => Conditions.Get(Conditions.PERSISTENT.FELDSPAR_START_ENTRY) && !Conditions.Get(Conditions.PERSISTENT.FELDSPAR_DONE), () =>
			{
				originalCharacter.SetActive(true);

				// After entry
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(true);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(true);
				SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Props_HEA_Radio/Feldspar_Pioneer_Radio").SetActive(true);
			});

			if (State == STATE.AT_TREE && !Conditions.Get(Conditions.PERSISTENT.FELDSPAR_LOOP_DIALOGUE_COMPLETE))
			{
				Conditions.Set(Conditions.CONDITION.FELDSPAR_SHOW_LOOP_DIALOGUE, true);				
			}

			ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				if (Conditions.Get(Conditions.PERSISTENT.FELDSPAR_DONE))
				{
					SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar").SetActive(false);
					WriteUtil.WriteLine("is it even work???");
					DisableSignals();
				}
			});

			
		}

		protected override void Dialogue_OnStartConversation()
		{
			var shipDestroyed = ShipHandler.HasShipExploded();

			var shipNearFeldspar = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f) && !shipDestroyed;
			var shipNearVillage = ShipHandler.IsShipNearVillage(100f) && !shipDestroyed;
			var shipFar = !shipNearFeldspar && !shipDestroyed;

			Conditions.Set(Conditions.CONDITION.FELDSPAR_SHIP_NEAR, shipNearFeldspar);
			Conditions.Set(Conditions.CONDITION.FELDSPAR_SHIP_FAR, shipFar);
			Conditions.Set(Conditions.CONDITION.SHIP_NEAR_VILLAGE, shipNearVillage);
		}

		protected override void Dialogue_OnEndConversation()
		{
			switch (State)
			{
				case STATE.ORIGINAL:
					if (Conditions.Get(Conditions.CONDITION.FELDSPAR_START_DONE))
					{
						ChangeState(STATE.ON_SHIP);
						Invoke("SpawnAngler", 5f);

						DisableSignals();
					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.FELDSPAR_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);
						DisableSignals();
					}
					break;
			}

			Conditions.Set(Conditions.CONDITION.FELDSPAR_SHOW_LOOP_DIALOGUE, false);
		}
		
		private void SpawnAngler()
		{
			var rudolfoFish = SearchUtilities.Find("Rudolfo");
			rudolfoFish.transform.localPosition = new UnityEngine.Vector3(3f, -7f, -19f);
			SearchUtilities.Find("Rudolfo/AudioController/LoopSource").GetComponent<OWAudioSource>().pitch = 2f;
			rudolfoFish.SetActive(true);

		}

		private void DisableSignals()
        {
			SearchUtilities.Find("DarkBramble_Body/Sector_DB/Interactables_DB/EntranceWarp_ToHub/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(false);
			SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Props_HEA_Radio/Feldspar_Pioneer_Radio").SetActive(false);
		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}
