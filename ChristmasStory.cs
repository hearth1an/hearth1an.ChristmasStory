using ChrismasStory.Characters.Travelers;
using ChrismasStory.Components;
using ChrismasStory.Utilities.ModAPIs;
using ChristmasStory.Components;
using NewHorizons.Utility;
using OWML.Common;
using OWML.ModHelper;
using System;
using UnityEngine;

namespace ChrismasStory
{
	public class ChristmasStory : ModBehaviour
	{
		public static INewHorizons newHorizonsAPI;
		public static ChristmasStory Instance;

		private void Awake()
		{
			Instance = this;
		}
		private void Start()
		{
			var newHorizonsAPI = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
			newHorizonsAPI.LoadConfigs(this);
			newHorizonsAPI.GetStarSystemLoadedEvent().AddListener(OnStarSystemLoaded);
			ModHelper.Console.WriteLine($"{nameof(ChristmasStory)} is loaded!", MessageType.Success);
		}

		public static void WriteLine(string line) => Instance.ModHelper.Console.WriteLine(line, MessageType.Info);
		public static void WriteError(string line) => Instance.ModHelper.Console.WriteLine(line, MessageType.Error);
		public static void WriteLine(string line, MessageType type) => Instance.ModHelper.Console.WriteLine($"{type}: " + line, type);

		private void OnStarSystemLoaded(string systemName)
		{
			WriteLine("LOADED SYSTEM " + systemName);

			if (systemName == "SolarSystem")
			{
				try
				{
					SpawnOnStart();
				}
				catch (Exception ex)
				{
					WriteError($"{ex}");
				}
			}
		}

		public void SpawnOnStart()
		{
			var player = SearchUtilities.Find("Player_Body");
			player.AddComponent<PlayerEffectController>();
			player.AddComponent<HeldItemHandler>();
			player.AddComponent<SolanumAnimationController>();
			// player.AddComponent<PrisonerAnimationController>();

			var ship = SearchUtilities.Find("Ship_Body");
			ship.AddComponent<ShipHandler>();

			// Handles collecting each character
			var characterControllers = new GameObject("ChristmasCharacterControllers");
			characterControllers.AddComponent<ChertCharacterController>();
			characterControllers.AddComponent<EskerCharacterController>();
			characterControllers.AddComponent<FeldsparCharacterController>();
			characterControllers.AddComponent<GabbroCharacterController>();
			characterControllers.AddComponent<RiebeckCharacterController>();
			characterControllers.AddComponent<PlayerNPCCharacterController>();
			characterControllers.AddComponent<SolanumCharacterController>();
			// characterControllers.AddComponent<PrisonerCharacterController>();


			if (PlayerData.PersistentConditionExists("CHERT_PHRASE_KNOWN"))
			{
				PlayerData.SetPersistentCondition("CHERT_PHRASE_KNOWN_NEXT_LOOP", true);
				ModHelper.Console.WriteLine("Chert phrase known.", MessageType.Success);
			}
			if (PlayerData.PersistentConditionExists("SOLANUM_DONE"))
			{				
				ModHelper.Console.WriteLine("Solanum event completed.", MessageType.Success);
			}


#if DEBUG
			player.AddComponent<DebugCommands>();
#endif

			Delay.FireOnNextUpdate(AfterSpawn);
		}

		private void AfterSpawn()
		{
			TransformThings();
			TravellersReplacements();
			CharactersReplacement();
			GeoRemovements();

			
		}	

		public void GeoRemovements()
		{
			try
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/Geysers/Geyser_Village").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/Geysers/Geyser_TutorialLand").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Geometry_TH/Terrain_TH_Water_v3/Village_Lower_Water").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Geometry_LowerVillage/OtherComponentsGroup/ControlledByProxy_Structures/Terrain_TH_VillageGeyser").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Lighting_LowerVillage/OtherComponentsGroup/LowerVillage/GeyserPlatform_Low").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Lighting_LowerVillage/OtherComponentsGroup/LowerVillage/GeyserPlatform_Mid").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/GeyserBoards_Flags").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Lighting_LowerVillage/OtherComponentsGroup/LowerVillage/Props_HEA_Lantern (11)").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Volumes_Village/MusicVolume_Village").SetActive(false);
				SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector Quantum Pole Path/Fragment QuantumPolePath 5").GetComponent<FragmentIntegrity>().enabled = false;
				SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/Interactables_EYEState/ConversationPivot/Character_NOM_Solanum/ConversationZone").SetActive(false);
				SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_EYE/Interactables_EYEState/ConversationPivot/Character_NOM_Solanum/WatchZone").SetActive(false);
				SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);


			}
			catch (Exception ex)
			{
				WriteError(ex.ToString());
			}
		}

		public void TransformThings()
		{
			try
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").transform.localScale = new Vector3(7f, 4.5f, 7f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 2f;
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").GetComponent<CapsuleCollider>().radius = 7f;
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 2f;
				// SearchUtilities.Find("TimberHearth_Body/Sector_TH/Geometry_GabbroShip").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker").AddComponent<OWCapsuleCollider>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar").AddComponent<OWCapsuleCollider>();
				SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player/ConversationZone_NPC_Player").SetActive(false);


                SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/ConversationZone").transform.localPosition = new Vector3(0, 0, 0);
				SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/Signal_Harmonica").transform.localPosition = new Vector3(0, 0, 0);

				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;

				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/ConversationZone").transform.localPosition = new Vector3(0, 1f, 0);
				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Whistling").transform.localPosition = new Vector3(0, 1f, 0);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/ConversationZone").transform.localPosition = new Vector3(0, 1f, 0);

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/Signal_Banjo").transform.localPosition = new Vector3(0, 1f, 0);
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").transform.localPosition = new Vector3(-0.1f, 0.8f, 0);
				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/ConversationZone").transform.localPosition = new Vector3(-0.1f, 0.4f, 0);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/Signal_Harmonica").transform.localPosition = new Vector3(0f, 0f, 0f);				

				// SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebec/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				/*
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				*/
			}
            catch (Exception ex)
			{
				WriteError(ex.ToString());
			}

		}

		public void CharactersReplacement()
		{
			try
			{
				// Marl
				var Marl_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl");
				Marl_Character.transform.localPosition = new Vector3(10.2747f, 7.3018f, -7.3346f);
				Marl_Character.transform.localRotation = new Quaternion(-0.0151f, 0.1999f, -0.0247f, -0.9794f);

				// Tephra
				var Tephra_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra");
				Tephra_Character.transform.localPosition = new Vector3(-5.9785f, 8.7614f, -1.742f);
				Tephra_Character.transform.localRotation = new Quaternion(0.0245f, 0.5553f, 0.0357f, 0.8305f);

				// Galena
				var Galena_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena");
				Galena_Character.transform.localPosition = new Vector3(1.2199f, 7.7457f, -2.38f);
			}
			catch (Exception ex)
			{
				WriteError(ex.ToString());
			}
		}

		public void TravellersReplacements()
		{
			try
			{
				// Feldspar
				SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters").SetActive(false);

				// Riebec
				SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck").SetActive(false);
				SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Signal_Banjo").SetActive(false);

				// Esker
				SearchUtilities.Find("Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/ConversationZone_Esker").SetActive(false);

				// Chert
				SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert/ConversationZone_Chert").SetActive(false);

				// Gabbro
				SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/ConversationZone_Gabbro").SetActive(false);
			}
			catch (Exception ex)
			{
				WriteError(ex.ToString());
			}
		}



		public void Ernesto_Start()
		{
			/* 
            Slate will ask to check if everyone is ready to call hornfels and start, Chert would ask if I want to invite Owlks or Solanum so player can go for it or skip and talk to Hornfels instead.

            Player goes to Hornfels, telling about celebration > closing your eyes > Hornfels appears near the Christmas tree and asks to put something shiny on the top of the tree and we can search for something like this in the observatory >
            There will be Ernesto dialogue and he will shine very BRIGHT! > player asks him for help > closing eyes > appears on the top of the tree. 

            */
		}

		public void Optional_Prisoner_Start()
		{
			/* 
            Player will need to go to the stranger, do the EoTE ending but players vision will be different, about inviting him to the christmas > he goes away like always and his vision torch will give player new vision about how player can bring him to the TH >
            He will show that in his prison IRL there will be an artifact that will make his proection when dropped. > Player will bring it to the TH > script will need to check if artifact on timber hearth to complete the quest.

            */
		}

		public void Optional_Solanum_Start()
		{
			/* 
            At the start of the game when player will have a conversation with Gal. He will say that he happy about translator and he also can do nomai writings on stones and he 
            wish if someone could read them one day. > Player will need to ask him if he could write something like "Merry Christmas, Solanum! Join us to celebration on Timber Hearth!" > closing eyes, stone with writing appears >
            player brings it to Solanum > Drop it near her > Script checking the distance > she replying that she probably will be able to appear on TH, we just need to take Nomai rock to TH > player takes it and brings to TH.

            > she appears there

            */
		}



		public void Update()
		{

		}
	}
}