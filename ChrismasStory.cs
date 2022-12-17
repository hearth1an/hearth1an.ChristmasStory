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
            
        }       

       
        public void Update()
        {
           
        }
    }
}