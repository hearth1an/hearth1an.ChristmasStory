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

			var vineCollider = SearchUtilities.Find("Ship_Body/ShipSector/Vine_Collider");
			vineCollider.AddComponent<OWCapsuleCollider>();

			vineCollider.transform.parent = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_3/Terrain_DB_Vine_v2").transform.parent;

			var shipDialogue = SearchUtilities.Find("Ship_Toy_Dialogue");
			shipDialogue.GetComponent<InteractReceiver>().ChangePrompt(TranslationHandler.GetTranslation("SHIP_TOY_PROMT", TranslationHandler.TextType.UI));
			shipDialogue.GetComponent<InteractReceiver>()._usableInShip = true;
			shipDialogue.SetActive(false);
			SearchUtilities.Find("Toy_Box").AddComponent<OWCapsuleCollider>();

			var seedToy = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed");
			var snowmanToy = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman");
			seedToy.SetActive(false);
			snowmanToy.SetActive(false);

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_1").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_2").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_3").SetActive(false);

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_3").AddComponent<UnityEngine.CapsuleCollider>().radius = 2f;

			CheckVines();
			CheckLastToy();

			if (PlayerData.GetPersistentCondition("SEED_TOY_GIVEN") || PlayerData.GetPersistentCondition("SNOWMAN_TOY_GIVEN"))
			{
				shipDialogue.SetActive(true);
			}

			base.Start();
			Invoke("ChangePromt", 1f);
		}

		protected override void Dialogue_OnStartConversation()
		{
			
			if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY") == true)
			{
				Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
				Conditions.Set(Conditions.CONDITION.TOY_PLACED, true);
				Conditions.Set(Conditions.CONDITION.SNOWMAN_CURRENT_TOY, false);
				Conditions.Set(Conditions.CONDITION.SEED_CURRENT_TOY, true);
			}
			if (PlayerData.GetPersistentCondition("SNOWMAN_CURRENT_TOY") == true)
			{
				Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
				Conditions.Set(Conditions.CONDITION.TOY_PLACED, true);
				Conditions.Set(Conditions.CONDITION.SNOWMAN_CURRENT_TOY, true);
				Conditions.Set(Conditions.CONDITION.SEED_CURRENT_TOY, false);
			}
			if (PlayerData.GetPersistentCondition("TOYS_REMOVED") == true)
			{
				Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, true);
				Conditions.Set(Conditions.CONDITION.TOY_PLACED, false);
				Conditions.Set(Conditions.CONDITION.SNOWMAN_CURRENT_TOY, false);
				Conditions.Set(Conditions.CONDITION.SEED_CURRENT_TOY, false);
			}
			
		}
		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.TOY_PLACED) == true)
            {
				WriteUtil.WriteLine("toy  placed");				
				if (Conditions.Get(Conditions.CONDITION.SEED_CURRENT_TOY) && !Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					PlayerEffectController.Blink(2); PlayerEffectController.AddLock(2);
					Invoke("AddSeed", 1f);
					WriteUtil.WriteLine("Seed toy");
				}

				if (Conditions.Get(Conditions.CONDITION.SNOWMAN_CURRENT_TOY) && !Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					PlayerEffectController.Blink(2); PlayerEffectController.AddLock(2f);
					Invoke("AddSnowman", 1f);
					WriteUtil.WriteLine("Snowman toy");
				}
				if (Conditions.Get(Conditions.CONDITION.TOYS_REMOVED) && !Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					PlayerEffectController.Blink(2); PlayerEffectController.AddLock(2);
					Invoke("RemoveToys", 1f);
					Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, true);
					WriteUtil.WriteLine("Toys removed");
				}
				if (Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					WriteUtil.WriteLine("Skip, no blink");
					Conditions.Set(Conditions.CONDITION.SKIP_BLINK, false);
				}
			}

			if (Conditions.Get(Conditions.CONDITION.TOY_PLACED) != true)
            {
				WriteUtil.WriteLine("toy not placed");
				if (Conditions.Get(Conditions.CONDITION.SEED_CURRENT_TOY) && !Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					PlayerEffectController.Blink(2); PlayerEffectController.AddLock(2);
					Invoke("AddSeed", 1f);
					Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
					WriteUtil.WriteLine("Seed toy");
				}

				if (Conditions.Get(Conditions.CONDITION.SNOWMAN_CURRENT_TOY) && !Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					PlayerEffectController.Blink(2); PlayerEffectController.AddLock(2f);
					Invoke("AddSnowman", 1f);
					Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
					WriteUtil.WriteLine("Snowman toy");
				}

				if (Conditions.Get(Conditions.CONDITION.SKIP_BLINK))
				{
					WriteUtil.WriteLine("Skip, no blink");
					Conditions.Set(Conditions.CONDITION.SKIP_BLINK, false);
				}
			}
		}

		private void CheckVines()
		{
			if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY_1_LOOP") == true)
			{
				SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_1").SetActive(true);

				if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY_2_LOOP") == true)
				{
					SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_2").SetActive(true);

					if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY_3_LOOP") == true)
					{
						SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_3").SetActive(true);
					}
					PlayerData.SetPersistentCondition("SEED_CURRENT_TOY_3_LOOP", true);
				}
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY_2_LOOP", true);
			}
		}

		private void DestroyVines()
        {
			PlayerData.SetPersistentCondition(("SEED_CURRENT_TOY_1_LOOP"), false);
			PlayerData.SetPersistentCondition(("SEED_CURRENT_TOY_2_LOOP"), false);
			PlayerData.SetPersistentCondition(("SEED_CURRENT_TOY_3_LOOP"), false);

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_1").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_2").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed/Toy_Vine_3").SetActive(false);
		}

		private void CheckLastToy()
		{
			var seedToy = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed");
			var snowmanToy = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman");
			var shipDialogue = SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Box/Ship_Toy_Dialogue");

			if (PlayerData.GetPersistentCondition("SEED_CURRENT_TOY"))
			{
				seedToy.SetActive(true);
				snowmanToy.SetActive(false);
				shipDialogue.SetActive(true);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY_1_LOOP", true);
			}

			if (PlayerData.GetPersistentCondition("SNOWMAN_CURRENT_TOY"))
			{
				seedToy.SetActive(false);
				snowmanToy.SetActive(true);
				shipDialogue.SetActive(true);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY_1_LOOP", false);
			}
			if (PlayerData.GetPersistentCondition("TOYS_REMOVED"))
			{
				seedToy.SetActive(false);
				snowmanToy.SetActive(false);
				shipDialogue.SetActive(true);
				PlayerData.SetPersistentCondition("SEED_CURRENT_TOY_1_LOOP", false);
			}
		}

		private void AddSeed()
		{
			Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
			Conditions.Set(Conditions.CONDITION.TOY_PLACED, true);
			Conditions.Set(Conditions.CONDITION.SNOWMAN_CURRENT_TOY, false);
			

			PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", true);
			PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", false);
			PlayerData.SetPersistentCondition("TOYS_REMOVED", false);
			PlayerData.SetPersistentCondition(("SEED_CURRENT_TOY_1_LOOP"), true);

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed").SetActive(true);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman").SetActive(false);
		}
		private void AddSnowman()
		{
			Conditions.Set(Conditions.CONDITION.TOYS_REMOVED, false);
			Conditions.Set(Conditions.CONDITION.TOY_PLACED, true);
			Conditions.Set(Conditions.CONDITION.SEED_CURRENT_TOY, false);			

			PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", true);
			PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", false);
			PlayerData.SetPersistentCondition("TOYS_REMOVED", false);

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman").SetActive(true);

			DestroyVines();
		}
		private void RemoveToys()
		{
			Conditions.Set(Conditions.CONDITION.TOY_PLACED, false);
			Conditions.Set(Conditions.CONDITION.SEED_CURRENT_TOY, false);
			Conditions.Set(Conditions.CONDITION.SNOWMAN_CURRENT_TOY, false);			

			PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", false);
			PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", false);
			PlayerData.SetPersistentCondition("TOYS_REMOVED", true);
			

			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman").SetActive(false);

			DestroyVines();
		}

		private void ChangePromt()
		{
			SearchUtilities.Find("Ship_Toy_Dialogue").GetComponent<InteractReceiver>().ChangePrompt(TranslationHandler.GetTranslation("SHIP_TOY_PROMT", TranslationHandler.TextType.UI));
		}

		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}