using NewHorizons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrismasStory.Characters.Travelers
{
    internal class FeldsparCharacterController : TravelerCharacterController
	{
        /* Find Feldspar's note > Change some signals destinations > Find him in Dark Bramble, talk to him > Сlose eyes > 
		He disappears, signal too > activating Feldspar in ship > Escort him to TH carefully > Check if we near the Village > 
		Talk to him in ship > Closing eyes > he appears near the Christmas tree always. And sure he should be the only one Feldspar and signal (!!! There's a lot of signals). 
		*/

        public override void Start()
        {
            // dialogue =
            originalCharacter = SearchUtilities.Find("DB_PioneerDimension_Body/Sector_PioneerDimension/Interactables_PioneerDimension/Pioneer_Characters/Traveller_HEA_Feldspar");
            shipCharacter = SearchUtilities.Find("Ship_Body/ShipSector/Ship_Feldspar");
            treeCharacter = SearchUtilities.Find("TimberHearth_Body/Sector_TH/Traveller_HEA_Feldspar");

            base.Start();

            ChangeState(STATE.ORIGINAL);
        }

		protected override void Dialogue_OnStartConversation()
		{

		}

		protected override void Dialogue_OnEndConversation()
		{

		}

		protected override void OnChangeState(STATE oldState, STATE newState)
		{
			// We can remove the signals here when it changes state
		}
	}
}
