using ChrismasStory.Characters.Travelers;
using ChrismasStory.Components;
using ChrismasStory.Utilities.ModAPIs;
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
			var newHorizonsAPI = ModHelper.Interaction.GetModApi<INewHorizons>("xen.NewHorizons");
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

			
			
			
			

			ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				TransformThings();
				TravellersReplacements();
				CharactersReplacement();
				GeoRemovements();
			});
        }

        public void GeoRemovements()
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

            SearchUtilities.Find("DB_HubDimension_Body/Sector_HubDimension/Interactables_HubDimension/InnerWarp_ToCluster/Signal_Harmonica").SetActive(false);

            /* SearchUtilities.Find("Feldspar_Ship").SetActive(false);
            SearchUtilities.Find("Riebeck_Ship").SetActive(false);
            SearchUtilities.Find("Esker_Ship").SetActive(false);
            SearchUtilities.Find("Esker_Ship").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").SetActive(false);
            SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player").SetActive(false);
			*/

            /* Gabbro ship is not working rn
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/GabbroShip").SetActive(false);
            */
        }

        public void TransformThings()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").transform.localScale = new Vector3(7f, 4.5f, 7f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").AddComponent<OWCapsuleCollider>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").AddComponent<OWCapsuleCollider>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 2f;
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").AddComponent<OWCapsuleCollider>();
			// SearchUtilities.Find("TimberHearth_Body/Sector_TH/Geometry_GabbroShip").AddComponent<OWCapsuleCollider>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper").AddComponent<OWCapsuleCollider>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking").AddComponent<OWCapsuleCollider>();

		}
		public void CharactersReplacement()
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

		public void TravellersReplacements()
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