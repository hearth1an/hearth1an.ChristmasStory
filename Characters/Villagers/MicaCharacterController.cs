using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class MicaCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.ERNESTO_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Mica_Dialogue").GetComponent<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();
	
			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.MICA_ENTRY))
			{
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Mica_Dialogue").SetActive(false);
			}
		}

		

		private void Update()
        {
			var shipModelTriggered = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Interactables_Village/ModelRocket_Station/ModelRocketStation_AttachPoint").GetComponent<RemoteFlightConsole>().enabled;
			var rockAnimationActive = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Snowman_Cairn/Props_TH_ClutterSmall").GetComponent<NomaiCairnRock>().enabled;
            var micaDialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Mica/Mica_Dialogue");
			var shipDialogue = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Box/Ship_Toy_Dialogue");

			if (shipModelTriggered && rockAnimationActive && !PlayerData.GetPersistentCondition("SNOWMAN_DESTROYED_EARLY") && micaDialogue.activeSelf)
            {
				PlayerData.SetPersistentCondition("SNOWMAN_DESTROYED_EARLY", true);				
				Utility.WriteUtil.WriteLine("SNOWMAN_DESTROYED_EARLY");
				
            }
			if (shipModelTriggered && rockAnimationActive && !PlayerData.GetPersistentCondition("SNOWMAN_DESTROYED") && !micaDialogue.activeSelf)
			{
				PlayerData.SetPersistentCondition("SNOWMAN_DESTROYED", true);
				PlayerData.SetPersistentCondition("SNOWMAN_NOT_DESTROYED", false);
				Utility.WriteUtil.WriteLine("SNOWMAN_DESTROYED");
				micaDialogue.SetActive(true);
			}
			if (shipModelTriggered && !rockAnimationActive && !PlayerData.GetPersistentCondition("SNOWMAN_NOT_DESTROYED") && !PlayerData.GetPersistentCondition("SNOWMAN_DESTROYED") && !micaDialogue.activeSelf)
			{
				PlayerData.SetPersistentCondition("SNOWMAN_NOT_DESTROYED", true);
				Utility.WriteUtil.WriteLine("SNOWMAN_NOT_DESTROYED");
				micaDialogue.SetActive(true);
			}
			if (PlayerData.GetPersistentCondition("SNOWMAN_TOY_GIVEN") && !shipDialogue.activeSelf)
			{
				PlayerEffectController.Blink(2f);
				shipDialogue.SetActive(true);

				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", false);
				PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", true);

				SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman").SetActive(true);
				SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed").SetActive(false);
			}

		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
