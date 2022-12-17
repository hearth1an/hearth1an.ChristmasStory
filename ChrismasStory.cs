using NewHorizons.Utility;
using OWML.Common;
using OWML.ModHelper;
using ChrismasStory.Utilities.ModAPIs;
using NewHorizons.Handlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChrismasStory
{
    public class ChrismasStory : ModBehaviour
    {
        public static INewHorizons newHorizonsAPI;
        public static ChrismasStory Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            var newHorizonsAPI = ModHelper.Interaction.GetModApi<INewHorizons>("xen.NewHorizons");
            newHorizonsAPI.LoadConfigs(this);
            newHorizonsAPI.GetStarSystemLoadedEvent().AddListener(OnStarSystemLoaded);
            ModHelper.Console.WriteLine($"{nameof(ChrismasStory)} is loaded!", MessageType.Success);            
        }
        
        private void OnStarSystemLoaded(string systemName)
        {
            ModHelper.Console.WriteLine("LOADED SYSTEM " + systemName);
            if (systemName == "SolarSystem")
            {
                SpawnOnStart();
            }
        }

        public void SpawnOnStart()
        {
            GeoRemovements();
            TransformThings();
            CharactersReplacement();

            TravellersReplacements();

            ModHelper.Events.Unity.FireOnNextUpdate(() =>
            {
               
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

        }

        public void TransformThings()
        {

            //SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Base/VillageCraterFloors/BatchedGroup/BatchedMeshRenderers_2/BatchedMeshRenderers_2").transform.localPosition = new Vector3(0f, 0f, 1.8f);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/OtherComponentsGroup/ControlledByProxy_Base/TimberHearthVillage_BakedTerrain/BakedTerrain_GeyserArea").transform.localPosition = new Vector3(0f, 0f, 1.8f);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Geometry_Village/BatchedGroup/BatchedMeshColliders_5").transform.localPosition = new Vector3(0f, 0f, 1.8f);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Center_Barrel").transform.localScale = new Vector3(6f, 3.5f, 6f);
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
            SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Traveller_HEA_Feldspar").SetActive(false);
            SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Signal_Harmonica").SetActive(false);

        }


        public void Update()
        {
           
        }
    }
}