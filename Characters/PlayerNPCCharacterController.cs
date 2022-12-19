using NewHorizons.Utility;

namespace ChrismasStory.Characters
{
	internal class PlayerNPCCharacterController : BaseCharacterController
	{
		/* 
            Same thing, you can ask yourself if you can go to TH just for fun. > closing eyes > he appears in ship > flight to TH > talk, closing eyes > He appears near the Christmas tree

            */

		public void Start()
		{
			originalCharacter = SearchUtilities.Find("TimeLoopRing_Body/Characters_TimeLoopRing/NPC_Player");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Player");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/NPC_Player");


			ChangeState(STATE.ORIGINAL);
		}
	}
}
