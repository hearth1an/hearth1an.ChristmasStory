using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrismasStory.Characters
{
	internal class GabbroCharacterController : BaseCharacterController
	{
		/* Visit Gabbro > He will ask you to start the new loop with exploding your ship near him > There should be a script that will check distance between Gabbro and ship and track the explosion > 
		Gabbro should disappear, player should die too.> Next loop he will appear Feldspar and signal. 
		*/

		public void Start()
		{
			originalCharacter = SearchUtilities.Find("GabbroIsland_Body/Sector_GabbroIsland/Interactables_GabbroIsland/Traveller_HEA_Gabbro/Traveller_HEA_Gabbro_ANIM_IdleFlute");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Gabbro");

			ChangeState(STATE.ORIGINAL);
		}
	}
}
