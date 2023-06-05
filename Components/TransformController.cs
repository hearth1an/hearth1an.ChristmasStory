using ChristmasStory.Utility;
using NewHorizons.Utility;
using OWML.ModHelper;
using System;
using System.Reflection;
using UnityEngine;
using NewHorizons.Builder;
using NewHorizons.External.Modules;

namespace ChristmasStory.Components
{
    public class TransformController : MonoBehaviour
    {
        public void Start()
        {
            TimberHearthTransforms();
            VillagersTransforms();
            VariousTransforms();
            ShipTransforms();
            PrisonerTransforms();
            FixPictures();
        }

        public void TimberHearthTransforms()
        {
            try
            {
                // Endgame thing
                ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
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

                // Fixing Timber Hearth batched mesh
                var thMesh = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Geometry_LowerVillage/BatchedGroup/BatchedMeshColliders_0");
                var thMeshFixed = SearchUtilities.Find("TH_Fixed_Geometry");
                thMeshFixed.transform.parent = thMesh.transform.parent;
                thMeshFixed.transform.position = thMesh.transform.position;
                thMeshFixed.transform.rotation = thMesh.transform.rotation;

                thMesh.GetComponent<MeshCollider>().sharedMesh = thMeshFixed.GetComponent<MeshCollider>().sharedMesh;
                thMesh.SetActive(false);
                thMeshFixed.DestroyAllComponents<MeshRenderer>();

                PlayerData.GetFreezeTimeWhileReadingConversations();

                // Timber Hearth transform
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Center_Barrel").transform.localScale = new Vector3(7f, 4.5f, 7f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Center_Barrel").AddComponent<CapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterLarge").DestroyAllComponents<MeshRenderer>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Esker_Dialogue").AddComponent<CapsuleCollider>().height = 4f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking").transform.localPosition = new Vector3(-2.52f, -23.30f, 185.66f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Esker").AddComponent<CapsuleCollider>().height = 2f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue").SetActive(true);                
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/ConversationZone_RSci").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Christmas_Tree").GetComponent<CapsuleCollider>().radius = 1f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_HEA_ChertShip").AddComponent<CapsuleCollider>().radius = 7f;
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper").AddComponent<CapsuleCollider>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar").AddComponent<CapsuleCollider>();

                // Prisoner near the tree
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localPosition = new Vector3(0.551f, -0.5451f, 0.2882f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderR/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowR/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristR/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachR/Props_IP_DW_GhostbirdInstrument_Bow").transform.localRotation = new Quaternion(0.8624f, 0.037f, -0.5042f, 0.0256f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Porphy/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Effects_IP_SarcophagusGlowCenter").transform.localScale = new Vector3(0.5f, 0.2f, 1f);

                // Water wheel
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/Gears1").DestroyAllComponents<RotateTransform>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/Gears2").DestroyAllComponents<RotateTransform>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/Gears3").DestroyAllComponents<RotateTransform>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/GearsCable").DestroyAllComponents<TextureAnimator>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/WaterWheel").DestroyAllComponents<TextureAnimator>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/Structure_HEA_WaterWheel/WaterWheel").DestroyAllComponents<RotateTransform>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Audio_Village/WaterWheel").SetActive(false);

                // Nomai wire
                SearchUtilities.Find("Nomai_wire").transform.localScale = new Vector3(1f, 1.7818f, 1f);
                var nomCable = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Lighting_CaveTwin/Structure_NOM_TLECable").GetComponent<MeshRenderer>();
                var villageCable = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_wire").GetComponent<MeshRenderer>();
                villageCable.sharedMaterials = nomCable.sharedMaterials;

                // River transform
                var water = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Geometry_TH/Terrain_TH_Water_v3/Village_Lower_Water");
                var ice = SearchUtilities.Find("Moon_Body/Sector_THM/Props_THM/OtherComponentsGroup/ControlledByProxy_Structures/THM_IceShards/BatchedGroup/BatchedMeshRenderers_1").GetComponent<MeshRenderer>();

                var iceSurface = SearchUtilities.Find("Comet_Body/Sector_CO/Geometry_CO/Frictionless_Batched/BatchedGroup/BatchedMeshColliders_0").GetComponent<BatchedMaterialLookup>();
                water.SetActive(true);
                water.GetComponent<MeshRenderer>().sharedMaterials = ice.sharedMaterials;
                water.GetComponent<MeshRenderer>().materials = ice.materials;
                water.GetComponent<MeshRenderer>().sharedMaterial.shader = ice.sharedMaterial.shader;
                water.AddComponent<MeshCollider>();
                water.AddComponent<OWCollider>();

                var newRiver = SearchUtilities.Find("TH_NEW_RIVER");
                newRiver.AddComponent<BatchedMaterialLookup>();
                newRiver.GetComponent<BatchedMaterialLookup>().materials = iceSurface.materials;

                /*
                newRiver.DestroyAllComponents<StreamingMeshHandle>();
                water.DestroyAllComponents<StreamingMeshHandle>();
                */

                // Hal's stone
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

                // Snow on ground	
                var thTerrain = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Base/VillageCraterFloors/BatchedGroup/BatchedMeshRenderers_0").GetComponent<MeshRenderer>();
                var alpine = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Prefab_TH_Alpine").GetComponent<Renderer>();
                var snowTerrain = SearchUtilities.Find("QuantumMoon_Body/Sector_QuantumMoon/State_BH/Geometry_BHState/BatchedGroup/BatchedMeshRenderers_3").GetComponent<Renderer>();
                var snowMat = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_NomaiCrater/Geometry_NomaiCrater/BatchedGroup/BatchedMeshColliders_2").GetComponent<BatchedMaterialLookup>();
                thTerrain.sharedMaterials = snowTerrain.sharedMaterials;

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

                var mainTreeTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Main_Tree_Snow.png");
                var snowGrassTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow.png");
                var snowWoodTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Snow_Wood.png");
                var woodTextureClean = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Crater_Wood_Clean.png");
                var snowTreeTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Foliage_TH_Snow_Tree.png");
                var snowStructureTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Structure_HEA_VillageCabin_Snow.png");
                var sequoiaSnowLeavesTexture = ChristmasStory.Instance.ModHelper.Assets.GetTexture("planets/Content/Textures/Tree_TH_SequoiaLeaves_snow.png");

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
                var skipZone = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Exhibits/Exhibits_Pictures");

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

                // Snowman near kids
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
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }

        public void FixPictures()
        {
            try
            {
                // fixing pictures in the museum
                var oldPictures = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Geometry_Observatory/Structure_HEA_Observatory_v3/ObservatoryPivot/Observatory_Interior/Interior_Exhibits/Exhibits_Pictures");
                var newPictures = SearchUtilities.Find("Fixed_Pictures");

                var oldExhibition = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Interactables_Observatory/MapSatelliteExhibit");
                var newExhibition = SearchUtilities.Find("Fixed_Pictures_DLC");

                newPictures.transform.parent = oldPictures.transform.parent;
                newPictures.transform.localPosition = oldPictures.transform.localPosition;
                newPictures.transform.localRotation = oldPictures.transform.localRotation;
                oldPictures.SetActive(false);

                newExhibition.transform.parent = oldExhibition.transform.parent;
                newExhibition.transform.localPosition = oldExhibition.transform.localPosition;
                newExhibition.transform.localRotation = oldExhibition.transform.localRotation;
                oldExhibition.SetActive(false);
            }
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }

        public void VillagersTransforms()
        {
            try
            {
                // Marl
                var Marl_Character = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl");
                Marl_Character.transform.localPosition = new Vector3(8.3747f, 7.4018f, -8.3346f);
                Marl_Character.transform.localRotation = new Quaternion(-0.02323f, -0.8668f, 0.0022f, 0.4982f);

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Characters_Village").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_VillageCemetery/Characters_VillageCemetery").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final/ConversationZone_RSci").DestroyAllComponents<InteractReceiver>();


                // Kids playing snowballs
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

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Marl").SetActive(false);
                SearchUtilities.Find("Tektite_Trigger").SetActive(false);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/ConversationZone").DestroyAllComponents<InteractReceiver>();              
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();
                

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Moraine/ConversationZone").DestroyAllComponents<InteractReceiver>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Arkose/ConversationZone").DestroyAllComponents<InteractReceiver>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone_Tuff").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone_Tuff").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tephra/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tephra/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Galena/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Galena/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Spinel/ConversationZone").DestroyAllComponents<InteractReceiver>();
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Spinel/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

            }
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }

        public void VariousTransforms()
        {
            try
            {
                // Probe
                SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
                SearchUtilities.Find("Probe_Body/ProbeGravity/Props_NOM_GravityCrystal_Base").transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
                //SearchUtilities.Find("Probe_Body/ProbeGravity/CapsuleVolume_NOM_GravityCrystal").transform.localScale = new Vector3(2f, 2f, 2f);

                SearchUtilities.Find("TimberHearth_Body/Sector_TH/GabbroShip").transform.localPosition = new Vector3(-0.4157f, -112.9464f, 231.8697f);
                SearchUtilities.Find("TimberHearth_Body/Sector_TH/GabbroShip").transform.localRotation = new Quaternion(0.3096f, 0.3218f, 0.0643f, 0.8924f);

                // Rudolfo tiny fish
                SearchUtilities.Find("Rudolfo/AudioController/LoopSource").GetComponent<AudioSource>().volume = 0.3f;
                SearchUtilities.Find("Rudolfo").SetActive(false);

                // Riebeck 
               
                var fragment = SearchUtilities.Find("Fragment QuantumPolePath 5_Body/ScaleRoot/Fragment QuantumPolePath 5");
                fragment.DestroyAllComponents<FragmentIntegrity>();
                fragment.GetComponent<DetachableFragment>()._forceDetection = DetachableFragment.ForceMask.SunOnly;
                //fragment.GetComponent<DetachableFragment>().
                //fragment.DestroyAllComponents<FragmentDragAnimator>();
                //fragment.DestroyAllComponents<DetachableFragment>();
                //fragment.DestroyAllComponents<FragmentEffects>();
                

            }
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }

        public void ShipTransforms()
        {
            try
            {
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker").AddComponent<CapsuleCollider>();
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar").AddComponent<CapsuleCollider>();
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
                SearchUtilities.Find("Ship_Body/ShipSector/Ship_Esker/ConversationZone").GetComponent<InteractReceiver>()._usableInShip = true;
            }
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }



        public void PrisonerTransforms()
        {
            try
            {

                // Fixing prison exterior, interior, trigger
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4").transform.localPosition = new Vector3(217.25f, -75.7f, -138.55f);
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Sector_PrisonInterior").transform.localPosition = new Vector3(217.25f, -75.7f, -138.55f); // 231,6003 -75,7 -147,7002
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/SectorTrigger_PrisonDocks/SectorTrigger_PrisonDocks_2").transform.localPosition = new Vector3(245.35f, -90f, -79.5f); // 221,7 -90 -88

                // Fixing prison chains
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/chain_L_geo").transform.localScale = new Vector3(1f, 0.8f, 1f);
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/chain_m_col").transform.localScale = new Vector3(1f, 0.75f, 1f);
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Geo_Prison/Structure_IP_Prison/Chain_R_geo").transform.localScale = new Vector3(1f, 0.8f, 1f);

                // Fixing prison proxy
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Proxy_Prison/Proxy_IP_Structure_Prison").transform.localScale = new Vector3(1f, 0.85f, 1f);
                SearchUtilities.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone4/Sector_PrisonDocks/Structures_PrisonDocks/Prison_Zone4/Proxy_Prison/Proxy_IP_Structure_Prison").transform.localPosition = new Vector3(0f, -3.6f, 0f);


                // Prisoner dialogue and interact reciever
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Prisoner_Dialogue").SetActive(false);
                SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/InteractReceiver").SetActive(false);

                var prisonerArtifact = SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternController>();

                ChristmasStory.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
                {
                    prisonerArtifact.enabled = true;
                    prisonerArtifact.SetLit(true);
                    prisonerArtifact._flameStrength = 3f;
                    prisonerArtifact.SetHeldByPlayer(false);
                    prisonerArtifact.UpdateVisuals();
                });

                SearchUtilities.Find("Prisoner_Artifact").GetComponent<DreamLanternItem>()._interactRange = 5f;

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

                var prisonLigthBeam = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light");
                var prisonLight = SearchUtilities.Find("RingWorld_Body/Sector_RingWorld/Prison_Light2");

                prisonLigthBeam.transform.localPosition = new Vector3(212.7656f, -72.5982f, -135.8029f);
                prisonLigthBeam.transform.localRotation = new Quaternion(0.6013f, -0.3345f, 0.3018f, 0.6599f);
                prisonLigthBeam.transform.localScale = new Vector3(0.3f, 0.8f, 0.3f);

                prisonLight.transform.localPosition = new Vector3(201.4169f, -70.8692f, -127.4167f);
                prisonLight.transform.localRotation = new Quaternion(-0.0135f, 0.8952f, 0.1759f, -0.4093f);
                prisonLight.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

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


                var prisonerTotem = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem").GetComponent<EclipseCodeController4>();
                int[] code = { 2, 1, 3, 1, 2 };
                prisonerTotem._code = code;
                prisonerTotem.CheckForCode();




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

                ChristmasStory.Instance.ModHelper.Events.Unity.RunWhen(() => prisonerBehavior._prisonerBrain._currentBehavior == PrisonerBehavior.WaitForConversation, () =>
                {
                    SearchUtilities.Find("Prisoner_Dialogue").SetActive(true);
                });

            }
            catch (Exception ex)
            {
                WriteUtil.WriteError($"{ex}");
            }
        }

        public void SecondLoopTransforms()
        {
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger_2").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village/Slate_Trigger").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Dialogue_3").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village/Hal_Trigger_2").SetActive(false);
        }

        private static void ResetSignal(OWAudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.Start();
        }

        public void ResetVillageSignals()
        {
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/GhostBird/Signal_Prisoner").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar/Signal_Signal_Harmonica").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck/Signal_Banjo").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Villager_HEA_Esker_ANIM_Rocking/Signal_Esker").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper/Signal_Drums").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Gabbro/Signal_Flute").GetComponent<OWAudioSource>());
            ResetSignal(SearchUtilities.Find("TimberHearth_Body/Sector_TH/Nomai_ANIM_SkyWatching_Idle/Signal_Nomai").GetComponent<OWAudioSource>());
        }
    }
}
