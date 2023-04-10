using ChristmasStory.Components;
using ChristmasStory.Utility;
using NewHorizons.Utility;

namespace ChristmasStory.Characters.Villagers
{
	/* 
	 * Visit Esker > He will say that he already knows everything bc he is listening to signalscope (he will be weirdo like always) >
	 * close eyes > he will appear in your ship > track if we are on Timber Hearth > talk to him > closing eyes > he will appear on TH always.
	 */

	internal class ElevatorController : TravelerCharacterController
	{
		public override Conditions.PERSISTENT DoneCondition => Conditions.PERSISTENT.TUFF_DONE;

		public override void Start()
		{
			dialogue = SearchUtilities.Find("Elevator_Dialogue").GetComponent<CharacterDialogueTree>();
			
			base.Start();
		}

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
        {
            if (Conditions.Get(Conditions.CONDITION.ELEVATOR_DONE))
            {
                SearchUtilities.Find("Elevator_Dialogue").SetActive(false);
				PlayerEffectController.PlayAudioOneShot(AudioType.ShipCockpitHeadlightsOff, 1);

				Invoke("CallElevatorDown", 5f);
            }
        }

        private void CallElevatorDown()
		{
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator").GetComponent<Elevator>().AttachPlayerAndStartLift();
			SearchUtilities.Find("TimberHearth_Body/Sector_TH/Interactables_TH/MineShaft/MineElevator/AttachPoint_MineElevator").GetComponent<PlayerAttachPoint>().DetachPlayer();
		}


		protected override void OnChangeState(STATE oldState, STATE newState) { }
	}
}
