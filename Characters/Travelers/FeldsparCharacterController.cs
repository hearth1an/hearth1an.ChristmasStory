using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Travelers
{
	/* Find Feldspar's note > Change some signals destinations > Find him in Dark Bramble, talk to him > Сlose eyes > 
	 * He disappears, signal too > activating Feldspar in ship > Escort him to TH carefully > Check if we near the Village > 
	 * Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And sure he should be the only one Feldspar and signal (!!! There's a lot of signals). 
	 */

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

			originalCharacter.SetActive(false);

			//SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/SeedWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);

			/*
			var signalPioneer = SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/SeedWarp_ToPioneer (1)/Signal_Harmonica");
			var signalHub = SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1");
			var signalHub2 = SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/AudioSource");
			signalHub2.SetActive(false);
					
			
			

			
			

			


			// After done
			SearchUtilities.Find("DarkBramble_Body/Sector_DB/Interactables_DB/EntranceWarp_ToHub/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
			var clusterSignal = SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer").GetAllChildren();
			clusterSignal[0].SetActive(false);
			clusterSignal[4].SetActive(false);
			SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);
			*/

			// Disabled before entry
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(false);
			SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_2").SetActive(false);
			SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(false);



			ChristmasStory.Instance.ModHelper.Events.Unity.RunWhen(() => Conditions.Get(Conditions.PERSISTENT.FELDSPAR_START_ENTRY), () =>
			{

				
				originalCharacter.SetActive(true);

				// After entry
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(true);
				SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(true);


			});

			base.Start();
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

						// After done
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(false);
						SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(false);
						SearchUtilities.Find("DarkBramble_Body/Sector_DB/Interactables_DB/EntranceWarp_ToHub/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);


					}
					break;
				case STATE.ON_SHIP:
					if (Conditions.Get(Conditions.CONDITION.FELDSPAR_SHIP_DONE))
					{
						ChangeState(STATE.AT_TREE);

						// After done
						SearchUtilities.Find("DarkBramble_Body/Sector_DB/Interactables_DB/EntranceWarp_ToHub/Signal_Harmonica").SetActive(false);						
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/InnerWarp_ToPioneer/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_ClusterDimension_Body/Sector_ClusterDimension/Interactables_ClusterDimension/SeedWarp_ToPioneer/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);
						SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Feldspar_Hub_1").SetActive(false);
						SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Feldspar_Pioneer").SetActive(false);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater/BrambleSeed/InnerWarp_ToPioneer (1)/Signal_Harmonica").SetActive(false);
					}
					break;
			}
		}
		
		private void SpawnAngler()
		{
			var rudolfoFish = SearchUtilities.Find("Rudolfo");
			rudolfoFish.transform.localPosition = new UnityEngine.Vector3(3f, -7f, -19f);
			SearchUtilities.Find("Rudolfo/AudioController/LoopSource").GetComponent<OWAudioSource>().pitch = 2f;
			rudolfoFish.SetActive(true);

		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{

		}
	}
}
