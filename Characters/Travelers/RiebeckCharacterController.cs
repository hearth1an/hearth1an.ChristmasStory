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
			var shipNearRiebeck = ShipHandler.IsCharacterNearShip(originalCharacter.gameObject, 40f);
			var shipDestroyed = ShipHandler.HasShipExploded();
			var shipFarNotDestroyed = !shipNearRiebeck && !shipDestroyed;

			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_NEAR_RIEBECK", shipNearRiebeck);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_FAR_RIEBECK", shipFarNotDestroyed);
			DialogueConditionManager.SharedInstance.SetConditionState("SHIP_DESTROYED", shipDestroyed);
		}

		protected override void Dialogue_OnEndConversation()
		{
			if (DialogueConditionManager.SharedInstance.GetConditionState("RIEBEC_START_DONE"))
			{
				ChangeState(STATE.ON_SHIP);
			}
		}
	}
}
