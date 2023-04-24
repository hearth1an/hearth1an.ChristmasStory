using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{	
	internal class TektiteCharacterController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.TEKTITE_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").GetComponent<CharacterDialogueTree>();
			var tektite = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2");		

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
			if (Conditions.Get(Conditions.CONDITION.NEW_ENTRY) && !Conditions.Get(Conditions.PERSISTENT.TEKTITE_DONE))
			{				
				PlayerEffectController.CloseEyes(1f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(false);
				Invoke("DonePreparation", 2f);
				Invoke("OpenEyes", 12f);
				Invoke("InvokeTrigger", 14f);
				PlayerEffectController.AddLock(15f);
				var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/bye_bye_seed.mp3");
				PlayerEffectController.PlayAudioExternalOneShot(sfx, 2f);
			}
			else if (Conditions.Get(Conditions.PERSISTENT.TEKTITE_DONE))
			{
				PlayerEffectController.Blink(2f);
				PlayerEffectController.AddLock(2f);
				SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(false);				
				Invoke("DoneThings", 1f);
			}
		}

		private void DonePreparation()
		{		
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Marl").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Interactables_ImpactCrater").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/DetailPatches_ImpactCrater/ImpactCrater_Clutter").SetActive(false);
		}

		private void DoneThings()
		{
			var sfx = ChristmasStory.Instance.ModHelper.Assets.GetAudio("planets/Content/music/tektite_go.mp3");
			PlayerEffectController.PlayAudioExternalOneShot(sfx, 3f);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_Village/Sector_LowerVillage/Characters_LowerVillage/Villager_HEA_Marl").SetActive(true);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2").SetActive(false);
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/New_Tektite").SetActive(true);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Box/Ship_Toy_Dialogue").SetActive(true);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Snowman").SetActive(false);
			SearchUtilities.Find("Ship_Body/Module_Cockpit/Toy_Seed").SetActive(true);

			
			PlayerData.SetPersistentCondition("SEED_CURRENT_TOY", true);
			PlayerData.SetPersistentCondition("SNOWMAN_CURRENT_TOY", false);
			PlayerData.SetPersistentCondition("TOY_PLACED", true);
			PlayerData.SetPersistentCondition("TOYS_REMOVED", false);
		}

		private void OpenEyes() => PlayerEffectController.OpenEyes(1f);
        private void InvokeTrigger()
        {
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Sector_ImpactCrater/Characters_ImpactCrater/Villager_HEA_Tektite_2/Tektite_Dialogue").SetActive(true);
			SearchUtilities.Find("Tektite_Trigger").SetActive(true);
			SearchUtilities.Find("Tektite_Trigger").GetComponent<UnityEngine.SphereCollider>().enabled = false;
			SearchUtilities.Find("Tektite_Trigger").GetComponent<UnityEngine.SphereCollider>().enabled = true;
		}


        protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
