using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class TektiteCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.TEKTITE_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").GetComponent<CharacterDialogueTree>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(false);

			var tektite = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2");
			tektite.AddComponent<FacePlayerWhenTalking>();
			
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Marl").SetActive(false);
			SearchUtilities.Find("Tektite_Trigger").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite/ConversationZone").DestroyAllComponents<InteractReceiver>();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite/ConversationZone").DestroyAllComponents<CharacterDialogueTree>();

			if (Conditions.Get(Conditions.PERSISTENT.TEKTITE_DONE))
            {
               tektite.SetActive(false);
			   SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite").SetActive(true);
			}
			else
            {
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite").SetActive(false);
			}
            base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{
			if (Conditions.Get(Conditions.CONDITION.NEW_ENTRY))
			{				
				PlayerEffectController.Blink(4f);
				Invoke("DonePreparation", 2f);
			}
			if (Conditions.Get(Conditions.PERSISTENT.TEKTITE_DONE))
			{
				PlayerEffectController.Blink(4f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2").SetActive(false);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite").SetActive(true);
			}			

		}

		private void DonePreparation()
		{
			SearchUtilities.Find("Tektite_Trigger").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Marl").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/DetailPatches_ImpactCrater/ImpactCrater_Clutter").SetActive(false);
		}
		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
