using ChrismasStory.Components;
using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrismasStory.Characters.Travelers
{
    internal class RiebeckCharacterController : TravelerCharacterController
	{
		/* Find Riebec > talk with him > check if ship is near (write a script that will check distance between ship and Riebec) > Сlose eyes > 
		Riebec disappears, signal too > activating Riebec in ship > Escort him to TH > Check if we near the Village > 
		Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And he should be the only one Riebec and signal.
		*/

		private const string ShipNearRiebeck = nameof(ShipNearRiebeck);

        public override void Start()
        {
            // dialogue =
            originalCharacter = SearchUtilities.Find("BrittleHollow_Body/Sector_BH/Sector_Crossroads/Characters_Crossroads/Traveller_HEA_Riebeck");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Riebeck");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Riebeck");

			base.Start();

			ChangeState(STATE.ORIGINAL);
        }

		protected override void Dialogue_OnStartConversation()
		{
			var isShipNear = ShipHandler.IsCharacterNearShip(originalCharacter, 50f);

			// We can use this persistent condition to change the dialogue Riebeck says
			PlayerData.SetPersistentCondition(ShipNearRiebeck, isShipNear);
		}

		protected override void Dialogue_OnEndConversation()
		{
			if (PlayerData.GetPersistentCondition(ShipNearRiebeck))
			{
				ChangeState(STATE.ON_SHIP);
			}
		}
	}
}
