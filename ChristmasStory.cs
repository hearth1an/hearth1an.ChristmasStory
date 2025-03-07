using ChristmasStory.Characters.Environment;
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
		public TransformController _transformController;
		public SceneTransformController _sceneTransformController;

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

			//Check DLC
			ModHelper.Events.Unity.RunWhen(() => EntitlementsManager.IsDlcOwned() != EntitlementsManager.AsyncOwnershipStatus.NotReady, () =>
			{
				if (EntitlementsManager.IsDlcOwned() != EntitlementsManager.AsyncOwnershipStatus.Owned)
				{
					ModHelper.Console.WriteLine("Christmas Story requires DLC owned. DLC not found. Check if DLC enabled or try to verify game files.", MessageType.Fatal);
				}
			});

			var sceneTransformController = new GameObject("SceneTransformController");
			sceneTransformController.AddComponent<SceneTransformController>();

			// for first launch
			sceneTransformController.GetComponent<SceneTransformController>().TitleScreenChanges();

			LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
			{	
				if (loadScene == OWScene.TitleScreen)
                {
					var sceneTransformController_2 = new GameObject("SceneTransformController");
					sceneTransformController_2.AddComponent<SceneTransformController>();
					// for swithcing scene
					sceneTransformController_2.GetComponent<SceneTransformController>().TitleScreenChanges();
				}
				if (loadScene == OWScene.Credits_Fast)
				{
					var creditsMusic = new GameObject("CreditsController");
					creditsMusic.AddComponent<SceneTransformController>().CreditsMusic();
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
			player.AddComponent<Components.HeldItemHandler>();
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

			var transformController = new GameObject("TransformController");
			transformController.AddComponent<TransformController>();			

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
				transformController.GetComponent<TransformController>().SecondLoopTransforms();
			}
#if DEBUG
			player.AddComponent<DebugCommands>();
#endif
			Instance.ModHelper.Events.Unity.FireInNUpdates(transformController.GetComponent<TransformController>().Start, 10);
		}
	}

}