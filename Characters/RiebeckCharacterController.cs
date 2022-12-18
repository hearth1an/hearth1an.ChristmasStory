using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrismasStory.Characters
{
	internal class RiebeckCharacterController : BaseCharacterController
	{
		/* Find Riebec > talk with him > check if ship is near (write a script that will check distance between ship and Riebec) > Сlose eyes > 
		Riebec disappears, signal too > activating Riebec in ship > Escort him to TH > Check if we near the Village > 
		Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And he should be the only one Riebec and signal.
		*/

		public void Start()
		{
			originalCharacter = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck");
			shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck");

			ChangeState(STATE.ORIGINAL);
		}
	}
}
