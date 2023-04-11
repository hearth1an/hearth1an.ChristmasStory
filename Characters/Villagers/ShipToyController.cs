using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;
using NewHorizons.Handlers;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class ShipToyController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.TUFF_DONE;

		public override void Start()
        {
            dialogue = SearchUtilities.Find("Ship_Toy_Dialogue").GetComponent<CharacterDialogueTree>();
			
			var shipDialogue = SearchUtilities.Find("Ship_Toy_Dialogue");
			shipDialogue.GetComponent<InteractReceiver>()._usableInShip = true;
			shipDialogue.GetComponent<InteractReceiver>().ChangePrompt(TranslationHandler.GetTranslation("SHIP_TOY_PROMT", TranslationHandler.TextType.UI));
			shipDialogue.SetActive(false);

			var seedToy = SearchUtilities.Find("Toy_Seed");
			var snowmanToy = SearchUtilities.Find("Toy_Snowman");
			seedToy.SetActive(false);
			snowmanToy.SetActive(false);

			if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY"))
            {
				seedToy.SetActive(true);
				snowmanToy.SetActive(false);
				shipDialogue.SetActive(true);
			}
			if (PlayerData.GetPersistentCondition("SNOWMAN_CURRENT_TOY"))
			{
				seedToy.SetActive(false);
				snowmanToy.SetActive(true);
				shipDialogue.SetActive(true);
			}
			if (PlayerData.GetPersistentCondition("TOYS_REMOVED"))
			{
				seedToy.SetActive(false);
				snowmanToy.SetActive(false);
				shipDialogue.SetActive(true);
			}

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
        {
			var seedToy = SearchUtilities.Find("Toy_Seed");
			var snowmanToy = SearchUtilities.Find("Toy_Snowman");

            if (Conditions.Get(Conditions.CONDITION.SEED_CURRENT_TOY))
            {
                PlayerEffectController.Blink(2);
				seedToy.SetActive(true);
				snowmanToy.SetActive(false);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", true);
				PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", false);
				PlayerData.SetPersistentCondition("TOYS_REMOVED", false);
			}
			if (Conditions.Get(Conditions.CONDITION.SNOWMAN_CURRENT_TOY))
			{
				PlayerEffectController.Blink(2);
				seedToy.SetActive(false);
				snowmanToy.SetActive(true);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", false);
				PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", true);
				PlayerData.SetPersistentCondition("TOYS_REMOVED", false);
			}
			if (Conditions.Get(Conditions.CONDITION.TOYS_REMOVED))
			{
				PlayerEffectController.Blink(2);
				seedToy.SetActive(false);
				snowmanToy.SetActive(false);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", false);
				PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", false);
				PlayerData.SetPersistentCondition("TOYS_REMOVED", true);
			}
		}

		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
