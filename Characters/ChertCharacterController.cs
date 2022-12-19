using NewHorizons.Utility;

namespace ChrismasStory.Characters
{
	internal class ChertCharacterController : BaseCharacterController
	{
		/* Find Chert > talk with him (he don't want to leave and you telling him that you know everything about nomai, supernova, stranger, time loop etc. He will laugh and ask to proof 3 things. 
		> 1st thing is timeloop. He will say the information only he knows, you will need to tell him it next loop. 
		> 2nd bring unknown nomai tech (Warp Core) and 3rd - strangers artifact. Since you have one loop to tell him everything, you need to bring both things. > 
		There should be a script that will check distance between Chert and Warp Core, Chert and artifact. If it worked => RunWhen thing to explore the fact that will open the dialogue node to make Chert disappear
		> Closing eyes > he appears near the Christmas tree always. And he should be the only one Chert and signal. 
		*/

		public void Start()
		{
			originalCharacter = SearchUtilities.Find("CaveTwin_Body/Sector_CaveTwin/Sector_NorthHemisphere/Sector_NorthSurface/Sector_Lakebed/Interactables_Lakebed/Traveller_HEA_Chert");
			//shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Chert");
			treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Chert_ANIM_Chatter_Chipper");


			ChangeState(STATE.ORIGINAL);
		}
	}
}
