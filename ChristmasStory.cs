using ChrismasStory.Characters.Travelers;
using ChrismasStory.Components;
using ChrismasStory.Utilities.ModAPIs;
using ChristmasStory.Components;
using ChristmasStory.Components.Animation;
using ChristmasStory.Utility;
using HarmonyLib;
using NewHorizons.Utility;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ChrismasStory
{
    public class ChristmasStory : ModBehaviour
	{
		public static INewHorizons newHorizonsAPI;
		public static ChristmasStory Instance;
		public static PrisonerBehavior behaviour;
		public FluidDetector FluidDetector;

		public Material grassMaterial;

		private void Awake()
		{
			Instance = this;
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
		}

		private void Start()
		{
			var newHorizonsAPI = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
			newHorizonsAPI.LoadConfigs(this);
			newHorizonsAPI.GetStarSystemLoadedEvent().AddListener(OnStarSystemLoaded);
			ModHelper.Console.WriteLine($"{nameof(ChristmasStory)} is loaded!", MessageType.Success);
		}

		public static void WriteDebug(string line)
		{
#if DEBUG
			Instance.ModHelper.Console.WriteLine("Debug: " + line, MessageType.Info);
#endif
		}
		public static void WriteLine(string line) => WriteLine(line, MessageType.Info);
		public static void WriteError(string line) => WriteLine(line, MessageType.Error);
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
			player.AddComponent<PrisonerAnimationController>();

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
			characterControllers.AddComponent<PrisonerCharacterController>();
			characterControllers.AddComponent<HalCharacterController>();

			// characterControllers.AddComponent<HornfelsCharacterController>(); characterControllers.AddComponent<ErnestoCharacterController>();

			if (Conditions.Get(Conditions.PERSISTENT.CHERT_PHRASE_KNOWN))
			{
				Conditions.Set(Conditions.PERSISTENT.CHERT_PHRASE_KNOWN_NEXT_LOOP, true);
				WriteLine("Chert phrase known.");
			}
			if (Conditions.Get(Conditions.PERSISTENT.SOLANUM_DONE))
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
			THModifications();
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
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_StartingCamp/Characters_StartingCamp/Villager_HEA_Slate").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/ConversationZone").SetActive(false);
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
				SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
				SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal_Base").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").transform.localScale = new Vector3(7f, 4.5f, 7f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").AddComponent<OWCapsuleCollider>();
								
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/ConversationZone").transform.localPosition = new Vector3(0, 2f, 0);
								
				var prisonerArtifact = SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternController>();
				Delay.FireOnNextUpdate(() =>
				{
					prisonerArtifact.enabled = true;
					prisonerArtifact.SetLit(true);					
					prisonerArtifact.UpdateVisuals();					
				});
				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 0.5f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").GetComponent<CapsuleCollider>().radius = 7f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar").AddComponent<OWCapsuleCollider>();
                SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player/ConversationZone_NPC_Player").SetActive(false);
                SearchUtilities.Find("DB_AnglerNestDimension_Body/Sector_AnglerNestDimension/Traveller_HEA_Feldspar/ConversationZone").transform.localPosition = new Vector3(0, 0, 0);
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Whistling").transform.localPosition = new Vector3(0, 1f, 0);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/ConversationZone").transform.localPosition = new Vector3(0, 1f, 0);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/Signal_Banjo").transform.localPosition = new Vector3(0, 1f, 0);
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").transform.localPosition = new Vector3(-0.1f, 0.8f, 0);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/ConversationZone").transform.localPosition = new Vector3(-0.1f, 0.4f, 0);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/Signal_Harmonica").transform.localPosition = new Vector3(0f, 0f, 0f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localPosition = new Vector3(0.551f, -0.5451f, 0.2882f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localRotation = new Quaternion(0.8624f, 0.037f, -0.5042f, 0.0256f);              
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<OWItem>()._interactable = true;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_IP_DreamLanternItem_2/Props_IP_Artifact/Flame").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);               
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_IP_DreamLanternItem_2").GetComponent<DreamLanternItem>()._interactable = false;
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/InteractReceiver").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Effects_IP_SarcophagusGlowCenter").transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/PrisonerHeadDetector").SetActive(false);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/LightSensor_GhostHead").SetActive(false);

				SearchUtilities.Find("Ernando/AudioController/LoopSource").GetComponent<AudioSource>().pitch = 1.3f;
				SearchUtilities.Find("Gustavo/AudioController/LoopSource").GetComponent<AudioSource>().pitch = 0.5f;
				SearchUtilities.Find("Rudolfo/AudioController/LoopSource").GetComponent<AudioSource>().pitch = 1f;

				SearchUtilities.Find("Ernando").SetActive(false);
				SearchUtilities.Find("Gustavo").SetActive(false);
				SearchUtilities.Find("Rudolfo").SetActive(false);


				// var slateOrigComponents = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_StartingCamp/Characters_StartingCamp/Villager_HEA_Slate/Villager_HEA_Slate_ANIM_LogSit/Slate_Skin_01:Slate_Mesh:Villager_HEA_Slate/Slate_Skin_01:Slate_Mesh:Slate_Skin").GetComponent<S>;
				// var slateNewComponents = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/Villager_HEA_Slate_ANIM_LogSit/Slate_Skin_01:Slate_Mesh:Villager_HEA_Slate").GetComponentsInChildren<SkinnedMeshRenderer>();

				

				// Transform prison sector

				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4").transform.localPosition = new Vector3(-19f, 0, 0);

				// Disabling water in prison, should be done only after the damb crush!
				/*
				
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/UnderwaterAudioVolume_Prison").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Effects_IP_PrisonWater").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Effects_Prison").SetActive(false);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Volumes_PrisonInterior/WaterVolume_Prison").SetActive(false);
				*/

				var artifact = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_IP_DreamLanternItem_2").GetComponent<DreamLanternController>();
				artifact.enabled = true;
				artifact._lit = true;

				var prisonBodyExt = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/prison_body_ext");
				var prisonBodyInt = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/Prison_Interior/Prison_Body_Interior");

				var prisonBodyExtCollider = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/prison_body_ext/body_ext_col");
				var prisonBodyIntCollider = SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/Prison_Interior/Prison_Body_Interior/COL_Prison_Body_Interior");

				var prisonBodyExtFixed = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Exterior_Fixed");
				var prisonBodyInteriorFixed = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Interior_Fixed");
			

				prisonBodyExtFixed.transform.position = prisonBodyInt.transform.position;
				prisonBodyExtFixed.transform.rotation = prisonBodyInt.transform.rotation;

				prisonBodyInteriorFixed.transform.position = prisonBodyExt.transform.position;
				prisonBodyInteriorFixed.transform.rotation = prisonBodyExt.transform.rotation;				

				prisonBodyExt.SetActive(false);
				prisonBodyInt.SetActive(false);

				var prisonLigthBeam = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light");
				var prisonLight = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light2");

				prisonLigthBeam.transform.localPosition = new Vector3(202.1948f, -71.4272f, -140.7394f);
				prisonLigthBeam.transform.localRotation = new Quaternion(-0.616f, 0.3204f, -0.3022f, -0.6531f);
				prisonLigthBeam.transform.localScale = new Vector3(0.3f, 1/2f, 0.3f);

				prisonLight.transform.localPosition = new Vector3(196.7155f, -70.8414f, -136.5985f);
				prisonLight.transform.localRotation = new Quaternion(-0.0191f, 0.8858f, 0.183f, -0.426f);
				prisonLight.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);


				var thMesh = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Geometry_LowerVillage/BatchedGroup/BatchedMeshColliders_0");
				var thMeshFixed = SearchUtilities.Find("TH_Fixed_Geometry");
				thMeshFixed.transform.parent = thMesh.transform.parent;
				thMeshFixed.transform.position = thMesh.transform.position;
				thMeshFixed.transform.rotation = thMesh.transform.rotation;

				thMesh.GetComponent<MeshCollider>().sharedMesh = thMeshFixed.GetComponent<MeshCollider>().sharedMesh;
				thMesh.SetActive(false);
				thMeshFixed.DestroyAllComponents<MeshRenderer>();


				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (9)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (1)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (2)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (10)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (10)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (2)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (3)").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (14)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (15)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (5)").transform.localPosition = new Vector3(-5f, -2f, -35f);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (6)").transform.localPosition = new Vector3(-5f, -2f, -35f);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (6)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (7)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (7)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (6)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (6)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (13)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (13)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (8)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (7)").transform.localPosition = new Vector3(-5f, -10f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (8)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (5)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (14)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (15)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (12)").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior/Interactibles_PrisonInterior/Prefab_IP_Sarcophagus/Prefab_IP_SleepingMummy_v2 (PRISONER)/Mummy_IP_ArtifactAnim").SetActive(false);
                SearchUtilities.Find("Prisoner_Dialogue").SetActive(false);
                SearchUtilities.Find("Prisoner_Clone").SetActive(false);
                SearchUtilities.Find("Prisoner_Vision_Torch").SetActive(false);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Vision_Torch/VisionBeam").SetActive(true);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Effects_IP_SIM_VisionTorch").SetActive(false);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector").transform.localPosition = new Vector3(-5f, -2f, -35f);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Volumes_PrisonCell/ElevatorLightsOutTrigger").SetActive(false);
                SearchUtilities.Find("Prisoner_Lantern").GetComponent<DreamLanternController>()._lit = true;
            
                var prisonerDialogue = SearchUtilities.Find("Prisoner_Dialogue");
                var prisonerInteractReciever = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/InteractReceiver");
                prisonerDialogue.transform.parent = prisonerInteractReciever.transform.parent;
                prisonerDialogue.transform.localPosition = new Vector3(0, 2.92f, 0.369f);

                var prisonerVision = SearchUtilities.Find("Prisoner_Vision");
                prisonerVision.transform.parent = prisonerInteractReciever.transform.parent;
                prisonerVision.transform.localPosition = new Vector3(0, 2.92f, 0.369f);

                var prisonerClone = SearchUtilities.Find("Prisoner_Clone");
                prisonerClone.SetActive(false);
                prisonerClone.AddComponent<CapsuleCollider>();

                var ghostBird = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_Merged").GetComponent<SkinnedMeshRenderer>();
                var ghostBirdAntler = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_Accessories/Ghostbird_Skin_01:Ghostbird_v004:Antlers_Left/Ghostbird_Skin_01:Ghostbird_v004:Antler_Upward").GetComponent<SkinnedMeshRenderer>();
                var ghostBirdAntlerBroken = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_IP/Ghostbird_Skin_01:Ghostbird_v004:Ghostbird_Accessories/Ghostbird_Skin_01:Ghostbird_v004:Antlers_Right/Ghostbird_Skin_01:Ghostbird_v004:Antler_Broken 1").GetComponent<SkinnedMeshRenderer>();
                var ghostBirdInstrument = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/Props_IP_DW_GhostbirdInstrument/Ghostbird_Instrument_geo").GetComponent<MeshRenderer>();
                var ghostBirdInstrumentStand_1 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/Props_IP_DW_GhostbirdInstrument/ip_instrument_stand/stand_bottom").GetComponent<MeshRenderer>();
                var ghostBirdInstrumentStand_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/Props_IP_DW_GhostbirdInstrument/ip_instrument_stand/stand_middle").GetComponent<MeshRenderer>();
                var ghostBirdInstrumentStand_3 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/Props_IP_DW_GhostbirdInstrument/ip_instrument_stand/stand_top").GetComponent<MeshRenderer>();
                var ghostBirdInstrumentBow = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").GetComponent<MeshRenderer>();
                var ghostBirdInstrumentMusicBox = SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/Props_IP_DW_GhostbirdInstrument/instrument_music_box").GetComponent<MeshRenderer>();
                var simHeadMaterial = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Prisoner_Clone/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/SIM_GhostBirdHead").GetComponent<MeshRenderer>();

                var neededMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Effects_IP_SarcophagusGlowCenter").GetComponent<MeshRenderer>();

                ghostBird.materials[0].shader = neededMaterial.materials[0].shader;
                ghostBird.materials[1].shader = neededMaterial.materials[0].shader;
                ghostBird.materials[2].shader = simHeadMaterial.materials[0].shader;

                ghostBird.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);
                ghostBird.materials[1].CopyPropertiesFromMaterial(neededMaterial.material);
                ghostBird.materials[2].CopyPropertiesFromMaterial(simHeadMaterial.material);

                ghostBirdAntler.material.shader = neededMaterial.material.shader;
                ghostBirdAntler.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);
                ghostBirdAntlerBroken.material.shader = neededMaterial.material.shader;
                ghostBirdAntlerBroken.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);

				ghostBirdInstrument.materials[0].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrument.materials[1].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrument.materials[2].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrument.materials[3].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrument.materials[4].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrument.materials[5].shader = neededMaterial.materials[0].shader;

				ghostBirdInstrument.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrument.materials[1].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrument.materials[2].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrument.materials[3].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrument.materials[4].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrument.materials[5].CopyPropertiesFromMaterial(neededMaterial.material);

				ghostBirdInstrumentStand_1.material.shader = neededMaterial.material.shader;
                ghostBirdInstrumentStand_1.materials = neededMaterial.materials;

                ghostBirdInstrumentStand_2.material.shader = neededMaterial.material.shader;
                ghostBirdInstrumentStand_2.materials = neededMaterial.materials;

                ghostBirdInstrumentStand_3.material.shader = neededMaterial.material.shader;
                ghostBirdInstrumentStand_3.materials = neededMaterial.materials;

				ghostBirdInstrumentMusicBox.materials[0].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrumentMusicBox.materials[1].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrumentMusicBox.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);
                ghostBirdInstrumentMusicBox.materials[1].CopyPropertiesFromMaterial(neededMaterial.material);

				ghostBirdInstrumentBow.materials[0].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrumentBow.materials[1].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrumentBow.materials[2].shader = neededMaterial.materials[0].shader;
				ghostBirdInstrumentBow.materials[3].shader = neededMaterial.materials[0].shader;

				ghostBirdInstrumentBow.materials[0].CopyPropertiesFromMaterial(neededMaterial.material);
                ghostBirdInstrumentBow.materials[1].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrumentBow.materials[2].CopyPropertiesFromMaterial(neededMaterial.material);
				ghostBirdInstrumentBow.materials[3].CopyPropertiesFromMaterial(neededMaterial.material);

				var prisonerBehavior = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostDirector_Prisoner").GetComponent<PrisonerDirector>();

				ModHelper.Events.Unity.RunWhen(() => prisonerBehavior._prisonerBrain._currentBehavior == PrisonerBehavior.WaitForConversation, () =>
				{
					SearchUtilities.Find("Prisoner_Dialogue").SetActive(true);					
				});
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
                Marl_Character.transform.localPosition = new Vector3(8.3747f, 7.4018f, -8.3346f);
                Marl_Character.transform.localRotation = new Quaternion(-0.02323f, -0.8668f, 0.0022f, 0.4982f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/ConversationZone").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue_Final").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Villager_HEA_Marl_ANIM_StareDwn").GetComponent<CharacterAnimController>().lookOnlyWhenTalking = false;

                // Tephra
                var Tephra_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra");
                Tephra_Character.transform.localPosition = new Vector3(-5.9785f, 8.7614f, -1.742f);
                Tephra_Character.transform.localRotation = new Quaternion(0.0245f, 0.5553f, 0.0357f, 0.8305f);

                // Galena
                var Galena_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena");
                Galena_Character.transform.localPosition = new Vector3(1.2199f, 7.7457f, -2.38f);

                // Hal
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Character_HEA_Hal_Museum").SetActive(false);

                SearchUtilities.Find("TimberHearth_Body/Hal_Village/Hal_Dialogue_2").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Hal_Village/Hal_Dialogue_2_ConversationTrigger").SetActive(false);

				// SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Characters_Village/Villager_HEA_Hal_Outside").SetActive(false);

				// Endgame event things disabling on start 

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/AnglerFishExhibit/Ernesto_Dialogue").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Ernesto").SetActive(false);
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

		public void THModifications()
		{
			/*
			// More snowflakes
			var effects = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Effects_TH/Effects_TH_Snowflakes");
			var vectionController = effects.GetComponent<PlanetaryVectionController>();
			vectionController._exclusionSectors.Clear();

			for (int i = 0; i < 10; i ++)
			{
				var moreEffects = GameObject.Instantiate(effects);
				moreEffects.transform.parent = effects.transform.parent;
				moreEffects.transform.localPosition = Vector3.zero;
			}			
			*/ 
			var alpine = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_TH_Alpine").GetComponent<Renderer>();			
				
			// Snow on ground
			var thTerrain = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Base/VillageCraterFloors/BatchedGroup/BatchedMeshRenderers_0").GetComponent<Renderer>();
			var snowTerrain = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_BH/Geometry_BHState/BatchedGroup/BatchedMeshRenderers_3").GetComponent<Renderer>();

			thTerrain.material.shader = snowTerrain.material.shader;
			thTerrain.materials = snowTerrain.materials;
			

			// Snow on grass
			var snowyGrassMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/DetailPatches_LowerVillage/LandingGeyserVillageArea/Foliage_TH_GrassPatch (2)").GetComponent<Renderer>().materials[0];
			var snowyWoodMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_0").GetComponent<Renderer>().materials[0];

            var snowyTreesMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Trees_LowerVillage/BatchedGroup/BatchedMeshRenderers_0").GetComponent<Renderer>().materials[0];
			var observatoryMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Planks").GetComponent<Renderer>().materials[0];
			var snowyStructureMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Geometry_LowerVillage/OtherComponentsGroup/ControlledByProxy_Structures/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_1").GetComponent<Renderer>().materials[0];

			var mainTreeTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Main_Tree_Snow.png");
			var snowGrassTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow.png");
			var snowWoodTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Snow_Wood.png");
			var woodTextureCleam = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Wood_Clean.png");
			var snowTreeTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow_Tree.png");			
			var snowStructureTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Structure_HEA_VillageCabin_Snow.png");

			alpine.material.mainTexture = mainTreeTexture;			
			
			snowyTreesMaterial.mainTexture = snowTreeTexture;
			snowyWoodMaterial.mainTexture = snowWoodTexture;
			snowyGrassMaterial.mainTexture = snowGrassTexture;
			snowyStructureMaterial.mainTexture = snowStructureTexture;
			observatoryMaterial.mainTexture = woodTextureCleam;

			var thMeshRenderers = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village").GetComponentsInChildren<Renderer>();
			var thMeshRenderers_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Props_TH/OtherComponentsGroup/ControlledByProxy_Terrain/Village").GetComponentsInChildren<Renderer>();
			var thExcludedSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior").GetComponentsInChildren<Renderer>();
			
			foreach (var meshRenderer in thMeshRenderers)
			{
				if (meshRenderer.name.Contains("Foliage_TH"))
				{
					meshRenderer.sharedMaterial = snowyGrassMaterial;
				}
				else if (meshRenderer.material.name.Contains("Terrain_TH_THSurface_mat"))
				{
					meshRenderer.sharedMaterial = snowTerrain.material;
				}
				else if (meshRenderer.material.name.Contains("Foliage_TH"))
				{
					meshRenderer.sharedMaterial = snowTerrain.material;
				}
				else if (meshRenderer.material.name.Contains("VillagePlanks_mat"))
				{
					meshRenderer.sharedMaterial = snowyWoodMaterial;
				}
				else if (meshRenderer.material.name.Contains("Tree_TH_RedwoodLeaves"))
				{
					meshRenderer.sharedMaterial = snowyTreesMaterial;
					snowyTreesMaterial.color = new Color(1f, 1f, 1f, 1f);
				}				
				else if (meshRenderer.material.name.Contains("Tree_TH_RedwoodLeaves_mat (Instance)"))
				{
					meshRenderer.sharedMaterial = snowyTreesMaterial;
				}
				else if (meshRenderer.material.name.Contains("Structure_HEA_VillageCabin"))
				{
					meshRenderer.sharedMaterial = snowyStructureMaterial;
				}
			}
			foreach (var meshRenderer in thMeshRenderers_2)
			{
				if (meshRenderer.material.name.Contains("Tree_TH_RedwoodLeaves"))
				{
					meshRenderer.sharedMaterial = snowyTreesMaterial;
					snowyTreesMaterial.color = new Color(1f, 1f, 1f, 1f);
				}
			}
			foreach (var meshRenderer in thExcludedSector)
			{
				if (meshRenderer.material.name.Contains("Structure_HEA_VillagePlanks"))
				{
					meshRenderer.sharedMaterial = observatoryMaterial;
				}
			}
		}	
	}
}