using HarmonyLib;
using NewHorizons.Utility;
using OWML.ModHelper;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

namespace ChrismasStory.Components
{
	
	internal class EndGameController : MonoBehaviour
	{
		
		public static EndGameController Instance;
        

        public void Start()
        {
            Instance = this;

            // Hornfels, Hal, Slate
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village_Final").SetActive(true);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hornfels_Village_Final").SetActive(true);

            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_Observatory/Characters_Observatory/Villager_HEA_Hornfels (1)").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Hal_Village").SetActive(false);

            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village_Final").SetActive(false);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Slate_Village").SetActive(false);

            // Marl
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").transform.localRotation = new Quaternion(-0.0104f, -0.0329f, 0.0209f, 0.9992f);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue_Final").SetActive(true);
            SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl/Marl_Dialogue").SetActive(false);
        }

    }
}
