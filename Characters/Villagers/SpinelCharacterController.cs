using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class SpinelCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.ERNESTO_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Spinel_Dialogue").GetComponent<CharacterDialogueTree>();
			
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Gneiss/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Secto r_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Rutile/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Galena/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Kids_PreGame/Villager_HEA_Tephra/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Moraine/ConversationZone").DestroyAllComponents<InteractReceiver>();
			//SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Moraine/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Arkose/ConversationZone").DestroyAllComponents<InteractReceiver>();
			//SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Arkose/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tuff/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tephra/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tephra/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Galena/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Galena/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Gossan/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Spinel/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Spinel/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			SearchUtilities.Find("New_Spinel").SetActive(false);

			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.SPINEL_TOLD))
            {
                PlayerEffectController.CloseEyes(1f);

				var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/jump.mp3");
                PlayerEffectController.PlayAudioExternalOneShot(sfx, 3f);

                Invoke("ChangePosition", 2f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel/Spinel_Dialogue").SetActive(false);
			}
		}

		private void ChangePosition()
        {
			

			PlayerEffectController.OpenEyes(1f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Spinel").SetActive(false);
			SearchUtilities.Find("New_Spinel").SetActive(true);
		}

		
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
