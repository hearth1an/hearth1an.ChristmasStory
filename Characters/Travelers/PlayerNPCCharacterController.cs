using NewHorizons.Utility;

namespace ChrismasStory.Characters.Travelers
{
    internal class PlayerNPCCharacterController : TravelerCharacterController
	{
        /* 
            Same thing, you can ask yourself if you can go to TH just for fun. > closing eyes > he appears in ship > flight to TH > talk, closing eyes > He appears near the Christmas tree

            */

        public override void Start()
        {
            // dialogue =
            originalCharacter = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player");

			base.Start();

			ChangeState(STATE.ORIGINAL);
        }

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{

		}
	}
}
