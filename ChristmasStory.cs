using ChristmasStory.Characters.Travelers;
using ChristmasStory.Characters.Villagers;
using ChristmasStory.Components;
using ChristmasStory.Components.Animation;
using ChristmasStory.Utility;
using HarmonyLib;
using NewHorizons.Utility;
using NewHorizons.Handlers;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Reflection;
using UnityEngine;


namespace ChristmasStory
{
	public class ChristmasStory : ModBehaviour
	{
		public static INewHorizons newHorizonsAPI;
		public static ChristmasStory Instance;
		public static PrisonerBehavior behaviour;
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

			// for first launch
			TitleScreenChanges();

			LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
			{	
				if (loadScene == OWScene.TitleScreen)
                {
					// for swithcing scene
					TitleScreenChanges();
				}
				if (loadScene == OWScene.Credits_Fast)
				{
					CreditsMusic();
				}
			};
		}

		private void OnStarSystemLoaded(string systemName)
		{
			WriteUtil.WriteLine("LOADED SYSTEM " + systemName);

			if (systemName == "SolarSystem")
			{
				try
				{
					SpawnOnStart();
				}
				catch (Exception ex)
				{
					WriteUtil.WriteError($"{ex}");
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
			characterControllers.AddComponent<SlateCharacterController>();
			characterControllers.AddComponent<HornfelsCharacterController>();
			characterControllers.AddComponent<TuffCharacterController>();
			characterControllers.AddComponent<MarlCharacterController>();
			characterControllers.AddComponent<TektiteCharacterController>();
			characterControllers.AddComponent<ErnestoCharacterController>();
			characterControllers.AddComponent<MicaCharacterController>();
			characterControllers.AddComponent<SpinelCharacterController>();
			characterControllers.AddComponent<EndGameController>();
			characterControllers.AddComponent<ElevatorController>();
			characterControllers.AddComponent<ShipToyController>();

			PlayerData.SetPersistentCondition("MARK_ON_HUD_TUTORIAL_COMPLETE", true);
			PlayerData.SetPersistentCondition("COMPLETED_SHIPLOG_TUTORIAL", true);

			if (Conditions.Get(Conditions.PERSISTENT.CHERT_PHRASE_KNOWN))
			{
				Conditions.Set(Conditions.PERSISTENT.CHERT_PHRASE_KNOWN_NEXT_LOOP, true);
				WriteUtil.WriteLine("Chert phrase known.");
			}
			if (Conditions.Get(Conditions.PERSISTENT.SOLANUM_DONE))
			{
				ModHelper.Console.WriteLine("Solanum event completed.");
			}

			ModHelper.Events.Unity.RunWhen(() => Conditions.Get(Conditions.PERSISTENT.CHERT_DONE) && Conditions.Get(Conditions.PERSISTENT.ESKER_DONE) && Conditions.Get(Conditions.PERSISTENT.FELDSPAR_DONE) && Conditions.Get(Conditions.PERSISTENT.GABBRO_DONE) && Conditions.Get(Conditions.PERSISTENT.RIEBECK_DONE), () =>
			{
				Conditions.Set(Conditions.PERSISTENT.ALL_TRAVELLERS_DONE, true);
				WriteUtil.WriteLine("All travellers done.");
			});

			if (Conditions.Get(Conditions.PERSISTENT.CHERT_DONE) && Conditions.Get(Conditions.PERSISTENT.ESKER_DONE) && Conditions.Get(Conditions.PERSISTENT.FELDSPAR_DONE) && Conditions.Get(Conditions.PERSISTENT.GABBRO_DONE) && Conditions.Get(Conditions.PERSISTENT.RIEBECK_DONE))
			{
				Conditions.Set(Conditions.PERSISTENT.ALL_TRAVELLERS_DONE, true);
				WriteUtil.WriteLine("All travellers done.");
			}
			if (PlayerData.GetPersistentCondition("LAUNCH_CODES_GIVEN") && !PlayerData.GetPersistentCondition("LOOP_COUNT_GOE_2"))
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger_2").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/Slate_Trigger").SetActive(false);				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_3").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger_2").SetActive(false);
			}
#if DEBUG
			player.AddComponent<DebugCommands>();
#endif
			Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
			{
				AfterSpawn();
			});
		}
		private void AfterSpawn()
		{
			TransformThings();
			TransformOnTimberHearth();
			TransformVillagers();
		}
		public void TransformThings()
		{
			try
			{
				SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector Quantum Pole Path/Fragment QuantumPolePath 5").GetComponent<FragmentIntegrity>().enabled = false;
				SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
				SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal_Base").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").transform.localScale = new Vector3(7f, 4.5f, 7f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").AddComponent<CapsuleCollider>();
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterLarge").DestroyAllComponents<MeshRenderer>();
				// SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterSmall").DestroyAllComponents<MeshRenderer>();

				
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Esker_Dialogue").AddComponent<CapsuleCollider>().height = 4f;

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking").transform.localPosition = new Vector3(-2.52f, -23.30f, 185.66f);
				//SearchUtilities.Find("Moon_Body/Sector_THM/Characters_THM/Villager_HEA_Esker/Esker_Start_Dialogue").AddComponent<CapsuleCollider>().height = 4f;
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Esker").AddComponent<CapsuleCollider>().height = 2f;

				

				var nomCable = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Lighting_CaveTwin/Structure_NOM_TLECable").GetComponent<MeshRenderer>();
				var villageCable = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_wire").GetComponent<MeshRenderer>();

				villageCable.sharedMaterials = nomCable.sharedMaterials;


				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").SetActive(true);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/ConversationZone").transform.localPosition = new Vector3(0, 2f, 0);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/ConversationZone_RSci").DestroyAllComponents<InteractReceiver>();
				SearchUtilities.Find("Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/ConversationZone").DestroyAllComponents<InteractReceiver>();
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Prisoner_Dialogue").SetActive(false);
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/InteractReceiver").SetActive(false);
				SearchUtilities.Find("Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/ConversationZone").DestroyAllComponents<InteractReceiver>();

				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker").AddComponent<CapsuleCollider>();
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar").AddComponent<CapsuleCollider>();			
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 1f;
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").AddComponent<CapsuleCollider>().radius = 7f;				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper").AddComponent<CapsuleCollider>();				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar").AddComponent<CapsuleCollider>();

				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
				SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localPosition = new Vector3(0.551f, -0.5451f, 0.2882f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localRotation = new Quaternion(0.8624f, 0.037f, -0.5042f, 0.0256f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/ConversationZone").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Effects_IP_SarcophagusGlowCenter").transform.localScale = new Vector3(0.5f, 0.2f, 1f);

				SearchUtilities.Find("Rudolfo/AudioController/LoopSource").GetComponent<AudioSource>().volume = 0.3f;
				SearchUtilities.Find("Rudolfo").SetActive(false);

				// Transform prison sector

				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4").transform.localPosition = new Vector3(-19f, 0, 0);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Geometry_RingInterior/Terrain_Ringworld_Root/BatchedGroup/BatchedMeshColliders_0").transform.localPosition = new Vector3(-19f, 0, 0);
				SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Geometry_RingInterior/Terrain_Ringworld_Root/OtherComponentsGroup/Terrain_Ringworld_Floorbed/Terrain_Ringworld_Z4_Floorbed").transform.localPosition = new Vector3(-19f, 0, 0);

				var prisonerArtifact = SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternController>();

				Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
				{
					prisonerArtifact.enabled = true;
					prisonerArtifact.SetLit(true);
					prisonerArtifact._flameStrength = 3f;
					prisonerArtifact.SetHeldByPlayer(false);
					prisonerArtifact.UpdateVisuals();
				});

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


                prisonBodyExtFixed.GetComponent<MeshRenderer>().sharedMaterials = prisonBodyExt.GetComponent<MeshRenderer>().sharedMaterials;
                // prisonBodyExtFixed.GetComponent<MeshRenderer>().materials[4].shader = prisonBodyExt.GetComponent<MeshRenderer>().materials[4].shader;

				var prisonLigthBeam = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light");
				var prisonLight = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light2");

				prisonLigthBeam.transform.localPosition = new Vector3(202.1948f, -71.4272f, -140.7394f);
				prisonLigthBeam.transform.localRotation = new Quaternion(-0.616f, 0.3204f, -0.3022f, -0.6531f);
				prisonLigthBeam.transform.localScale = new Vector3(0.3f, 1.9f, 0.3f);

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
				SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector").transform.localPosition = new Vector3(-5f, -2f, -35f);
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
				WriteUtil.WriteError(ex.ToString());
            }
        }

		public void TransformDynamicVillagers()
        {
			try
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Characters_Village").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_VillageCemetery/Characters_VillageCemetery").SetActive(false);
			}
			catch (Exception ex)
			{
				WriteUtil.WriteError($"{ex}");
			}
		}
        public void TransformVillagers()
        {
            // Marl
            var Marl_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl");
            Marl_Character.transform.localPosition = new Vector3(8.3747f, 7.4018f, -8.3346f);
            Marl_Character.transform.localRotation = new Quaternion(-0.02323f, -0.8668f, 0.0022f, 0.4982f);

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Characters_Village").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_VillageCemetery/Characters_VillageCemetery").SetActive(false);

			var rockMoraine = SearchUtilities.Find("Rock_Body 2/Detector_Rock").GetComponent<ConstantForceDetector>();
			var rockArkose = SearchUtilities.Find("Rock_Body 3/Detector_Rock").GetComponent<ConstantForceDetector>();
			var field = SearchUtilities.Find("TimberHearth_Body/FieldDetector_TH").GetComponent<ConstantForceDetector>();
			var gravity = SearchUtilities.Find("TimberHearth_Body/GravityWell_TH").GetComponent<GravityVolume>();

			rockMoraine._detectableFields[0] = gravity;
			rockMoraine._activeVolumes[0] = gravity;
			rockMoraine._activeInheritedDetector = field;

			rockArkose._detectableFields[0] = gravity;
			rockArkose._activeVolumes[0] = gravity;
			rockArkose._activeInheritedDetector = field;			

			var snowmanTop = SearchUtilities.Find("snowman_top");
			var cairnTop = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterSmall");
			snowmanTop.transform.SetParent(cairnTop.transform);
			snowmanTop.transform.localPosition = new Vector3(0, 0.1f, 0);
			snowmanTop.transform.localRotation = new Quaternion(0, 0, 0, 0);

			var snowmanMiddle = SearchUtilities.Find("snowman_middle");
			var cairnMiddle = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterSmall2");
			snowmanMiddle.transform.SetParent(cairnMiddle.transform);
			snowmanMiddle.transform.localPosition = new Vector3(0, 0f, 0);
			snowmanMiddle.transform.localRotation = new Quaternion(0.1791f, -0.7297f, 0.1573f, -0.6408f);

			var snowmanBottom = SearchUtilities.Find("snowman_bottom");
			var cairnBottom = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterLarge2");
			snowmanBottom.transform.SetParent(cairnBottom.transform);
			snowmanBottom.transform.localPosition = new Vector3(0, -0.4f, 0);
		}
        public void TransformOnTimberHearth()
		{			
			try
            {
				Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
				{
					if (Conditions.Get(Conditions.PERSISTENT.SLATE_START_DONE))
					{						
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1).").SetActive(false);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village").SetActive(false);

						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final").SetActive(true);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final").SetActive(true);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final").SetActive(true);

						Conditions.Set(Conditions.CONDITION.HORNFELS_FISH_TOLD, false);
						WriteUtil.WriteLine("Spawning endgame props!");

						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").transform.localRotation = new Quaternion(-0.0104f, -0.0329f, 0.0209f, 0.9992f);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue_Final").SetActive(true);
						SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(false);												
					}
				});

				SearchUtilities.Find("Nomai_wire").transform.localScale = new Vector3(1f, 1.7818f, 1f);
				var water = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Geometry_TH/Terrain_TH_Water_v3/Village_Lower_Water");
				// var ice = SearchUtilities.Find("Comet_Body/Sector_CO/Geometry_CO/Frictionless_Batched/OtherComponentsGroup/Terrain_CO_Surface_Ice").GetComponent<MeshRenderer>();
				// var ice = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Geo_ImpactCrater/OtherComponentsGroup/ImpactCrater_IceShards/BatchedGroup/BatchedMeshRenderers_0").GetComponent<MeshRenderer>();
				var ice = SearchUtilities.Find("Moon_Body/Sector_THM/Props_THM/OtherComponentsGroup/ControlledByProxy_Structures/THM_IceShards/BatchedGroup/BatchedMeshRenderers_1").GetComponent<MeshRenderer>();

				var iceSurface = SearchUtilities.Find("Comet_Body/Sector_CO/Geometry_CO/Frictionless_Batched/BatchedGroup/BatchedMeshColliders_0").GetComponent<BatchedMaterialLookup>();
				water.SetActive(true);
				water.GetComponent<MeshRenderer>().sharedMaterials = ice.sharedMaterials;
				water.GetComponent<MeshRenderer>().materials = ice.materials;
				water.GetComponent<MeshRenderer>().sharedMaterial.shader = ice.sharedMaterial.shader;
				water.AddComponent<MeshCollider>();
				water.AddComponent<OWCollider>();

				// Trying to make Hal's rock not associated with working projection stone

				//var invStoneMat2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Villager_HEA_Hal_ANIM_Museum/hal_skin:player_rig_v01:Traveller_Trajectory_Jnt/hal_skin:player_rig_v01:Traveller_ROOT_Jnt/hal_skin:player_rig_v01:Traveller_Spine_01_Jnt/hal_skin:player_rig_v01:Traveller_Spine_02_Jnt/hal_skin:player_rig_v01:Traveller_Spine_Top_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Clavicle_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Shoulder_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Elbow_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Wrist_Jnt/Props_HEA_RoastingStick/Prefab_NOM_SharedStone").GetComponent<MeshRenderer>();

				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Villager_HEA_Hal_ANIM_Museum/hal_skin:player_rig_v01:Traveller_Trajectory_Jnt/hal_skin:player_rig_v01:Traveller_ROOT_Jnt/hal_skin:player_rig_v01:Traveller_Spine_01_Jnt/hal_skin:player_rig_v01:Traveller_Spine_02_Jnt/hal_skin:player_rig_v01:Traveller_Spine_Top_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Clavicle_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Shoulder_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Elbow_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Wrist_Jnt/Props_HEA_RoastingStick/Prefab_NOM_SharedStone").SetActive(false);
				
				var origStone_1 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Villager_HEA_Hal_ANIM_Museum/hal_skin:player_rig_v01:Traveller_Trajectory_Jnt/hal_skin:player_rig_v01:Traveller_ROOT_Jnt/hal_skin:player_rig_v01:Traveller_Spine_01_Jnt/hal_skin:player_rig_v01:Traveller_Spine_02_Jnt/hal_skin:player_rig_v01:Traveller_Spine_Top_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Clavicle_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Shoulder_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Elbow_Jnt/hal_skin:player_rig_v01:Traveller_LF_Arm_Wrist_Jnt/Props_HEA_RoastingStick/Prefab_NOM_SharedStone");
                var origStone_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Invite_Stone/AnimRoot/Props_NOM_SharedStone(Clone)");
				var newStone_1 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/new_stone_1");
				var newStone_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/new_stone_2");

				newStone_1.transform.parent = origStone_1.transform.parent;
				newStone_1.transform.localPosition = origStone_1.transform.localPosition;
				newStone_1.transform.rotation = origStone_1.transform.rotation;
				newStone_1.transform.localScale = origStone_1.transform.localScale;
				origStone_1.SetActive(false);

				newStone_2.transform.parent = origStone_2.transform.parent;
				newStone_2.transform.localPosition = origStone_2.transform.localPosition;
				newStone_2.transform.rotation = origStone_2.transform.rotation;
				origStone_2.SetActive(false);


				var toyDialogue = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Box/Ship_Toy_Dialogue").GetComponent<InteractReceiver>();
				toyDialogue._usableInShip = true;
				toyDialogue.ChangePrompt(TranslationHandler.GetTranslation("SHIP_TOY_PROMT", TranslationHandler.TextType.UI));
				/*
			   invStoneMat.sharedMaterials[0] = rockMat.sharedMaterials[0];
			   invStoneMat.sharedMaterials[1] = rockMat.sharedMaterials[0];
			   invStoneMat.sharedMaterials[2] = rockMat.sharedMaterials[0];

				invStoneMat.material = rockMat.material;
				invStoneMat2.sharedMaterials[0] = rockMat.sharedMaterials[0];
				invStoneMat2.sharedMaterials[1] = rockMat.sharedMaterials[0];
				invStoneMat2.sharedMaterials[2] = rockMat.sharedMaterials[0];
				*/

				SearchUtilities.Find("TH_NEW_RIVER").AddComponent<BatchedMaterialLookup>();
				SearchUtilities.Find("TH_NEW_RIVER").GetComponent<BatchedMaterialLookup>().materials = iceSurface.materials;

				var alpine = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_TH_Alpine").GetComponent<Renderer>();

				// Snow on ground	
				var thTerrain = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Base/VillageCraterFloors/BatchedGroup/BatchedMeshRenderers_0").GetComponent<MeshRenderer>();

				var snowTerrain = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_BH/Geometry_BHState/BatchedGroup/BatchedMeshRenderers_3").GetComponent<Renderer>();				
				thTerrain.sharedMaterials = snowTerrain.sharedMaterials;
				
				var snowMat = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_NomaiCrater/Geometry_NomaiCrater/BatchedGroup/BatchedMeshColliders_2").GetComponent<BatchedMaterialLookup>();
			
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_0").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_1").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_1").GetComponent<BatchedMaterialLookup>().materials[1] = snowMat.materials[1];				
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_2").GetComponent<BatchedMaterialLookup>().materials[1] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_3").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_3").GetComponent<BatchedMaterialLookup>().materials[1] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_4").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_4").GetComponent<BatchedMaterialLookup>().materials[2] = snowMat.materials[1];	
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_5").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_5").GetComponent<BatchedMaterialLookup>().materials[3] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_6").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_8").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_9").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_10").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_11").GetComponent<BatchedMaterialLookup>().materials[0] = snowMat.materials[1];

				// Snow on grass
				var snowyGrassMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/DetailPatches_LowerVillage/LandingGeyserVillageArea/Foliage_TH_GrassPatch (2)").GetComponent<Renderer>().materials[0];
				var snowyWoodMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_0").GetComponent<Renderer>().materials[0];
				var snowyTreesMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Props_LowerVillage/OtherComponentsGroup/Trees_LowerVillage/BatchedGroup/BatchedMeshRenderers_0").GetComponent<Renderer>().materials[0];
				var observatoryMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Planks").GetComponent<Renderer>().materials[0];
				var snowyStructureMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Geometry_LowerVillage/OtherComponentsGroup/ControlledByProxy_Structures/Architecture_LowerVillage/BatchedGroup/BatchedMeshRenderers_1").GetComponent<Renderer>().materials[0];
				var snowySequoiaMaterial = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Terrain/Tree_TH_Sequoia/Prefab_TH_Sequoia_V2/Leaves_1").GetComponent<Renderer>().materials[0];

				var mainTreeTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Main_Tree_Snow.png");
				var snowGrassTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow.png");
				var snowWoodTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Snow_Wood.png");
				var woodTextureClean = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Wood_Clean.png");
				var snowTreeTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow_Tree.png");
				var snowStructureTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Structure_HEA_VillageCabin_Snow.png");
				var sequoiaSnowLeavesTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Tree_TH_SequoiaLeaves_snow.png");

				alpine.material.mainTexture = mainTreeTexture;

				snowyTreesMaterial.mainTexture = snowTreeTexture;
				snowyWoodMaterial.mainTexture = snowWoodTexture;
				snowyGrassMaterial.mainTexture = snowGrassTexture;
				snowyStructureMaterial.mainTexture = snowStructureTexture;
				observatoryMaterial.mainTexture = woodTextureClean;
				snowySequoiaMaterial.mainTexture = sequoiaSnowLeavesTexture;

				var thMeshRenderers = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village").GetComponentsInChildren<Renderer>();
				var thMeshRenderers_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Props_TH/OtherComponentsGroup/ControlledByProxy_Terrain/Village").GetComponentsInChildren<Renderer>();
				var thExcludedSector = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior").GetComponentsInChildren<Renderer>();
				var thExcludedSector_2 = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory").GetComponentsInChildren<Renderer>();
				var sequoiaTreeLeaves = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Terrain/Tree_TH_Sequoia/Prefab_TH_Sequoia_V2").GetComponentsInChildren<Renderer>();
				//var skipZone = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Exhibits")

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
				foreach (var meshRenderer in thExcludedSector_2)
				{
					if (meshRenderer.material.name.Contains("Structure_HEA_VillagePlanks"))
					{
						meshRenderer.sharedMaterial = observatoryMaterial;
					}
				}
				foreach (var meshRenderer in sequoiaTreeLeaves)
				{
					if (meshRenderer.material.name.Contains("Tree_TH_SequoiaLeaves_mat (Instance)"))
					{
						meshRenderer.sharedMaterial = snowySequoiaMaterial;
					}
				}
			}
			catch (Exception ex)
			{
				WriteUtil.WriteError($"{ex}");
			}
		}
		private void CreditsMusic()
        {
			var addMusic = GameObject.Find("AudioSource").GetComponent<OWAudioSource>();			
			var newMusic = Instance.ModHelper.Assets.GetAudio("planets/Content/music/Christmas_Instrument.mp3");
			addMusic._audioLibraryClip = AudioType.None;			
			addMusic.clip = newMusic;
			addMusic.Play();			
		}		
		public void TitleScreenChanges()
		{
			var snowTreeTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow_Tree_NEW.png");
			var snowTerrainTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Terrain_BH_SnowTransition_d.png");
			var snowFoliageTexture = Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Snow.png");
			var leavesMaterial = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Trees/Tree_TH_SaplingSequoia (1)/Redwood_Leaves_1").GetComponent<MeshRenderer>().materials[0];
			var foliageMaterial = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Props/Foliage/Foliage_TH_CraterGrass (3)").GetComponent<MeshRenderer>().materials[0];
			var trees = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Trees").GetComponentsInChildren<Renderer>();
			var terrain = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Terrain_THM_TitleScreen").GetComponent<MeshRenderer>();
			var foliage = GameObject.Find("Scene/Background/PlanetPivot/PlanetRoot/Props/Foliage").GetComponentsInChildren<Renderer>();

			leavesMaterial.mainTexture = snowTreeTexture;
			
			foliageMaterial.mainTexture = snowFoliageTexture;
			terrain.material.color = Color.white;			
			
			foreach (MeshRenderer meshRenderer in trees)
            {				
				if (meshRenderer.material.name.Contains("Tree_TS_RedwoodLeaves_mat (Instance)"))
				{					
					meshRenderer.sharedMaterial = leavesMaterial;
					leavesMaterial.color = Color.white;
				}				
            }
			foreach (MeshRenderer meshRenderer in foliage)
			{				
				if (meshRenderer.material.name.Contains("Foliage_TS_Grass (Instance)"))
				{
					meshRenderer.sharedMaterial = foliageMaterial;
				}
			}			

		}
	}

}