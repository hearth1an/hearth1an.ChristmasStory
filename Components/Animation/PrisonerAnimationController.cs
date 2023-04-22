using NewHorizons.Utility;
using UnityEngine;
using NewHorizons.Builder.Props;
using ChristmasStory.Utility;

namespace ChristmasStory.Components.Animation
{

    internal class PrisonerAnimationController : MonoBehaviour
    {
        private PrisonerEffects _animator;
        private VisionTorchTarget _visionTorchTarget;
        private bool _isPromtKnown = false;

        public static PrisonerAnimationController Instance;
        public void Start()
        {
            Instance = this;
            _animator = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<PrisonerEffects>();
            _visionTorchTarget = SearchUtilities.Find("Prisoner_Vision").GetComponent<VisionTorchTarget>();
            _visionTorchTarget.onSlidesComplete = Instance.OnVisionEnd;
            _visionTorchTarget.onSlidesStart = Instance.OnVisionStart;

            TotemCodePromptVolume.Create(SearchUtilities.Find("DreamWorld_Body"), new Vector3(-17.9f, -289.6f, 681.9f), 3f);
            var totemPromt = SearchUtilities.Find("DreamWorld_Body/TotemCodePromptVolume");           
            
            if (!PlayerData.GetPersistentCondition("TOTEM_KNOWN"))
            {
                totemPromt.SetActive(false);                                        
            }

        }

        public void PlayLightsUp()
        {
            Instance._animator.PlayTurnOnLightsAnimation();
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<VisionTorchItem>()._interactable = true;
            var prisonersOriginalTorch = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector");
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/New_Vision_Torch").transform.localPosition = new Vector3(-6.7623f, -293.5659f, 676.644f);
            SearchUtilities.Find("Prisoner_Clone").SetActive(true);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Effects_IP_SIM_VisionTorch").SetActive(true);
        } 
        private void OnVisionStart()
        {
            SearchUtilities.Find("Prisoner_Dialogue").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Props_PrisonCell/LowerCell/Props_IP_GhostbirdInstrument").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Props_PrisonCell/LowerCell/Props_IP_GhostbirdInstrument_Bow").SetActive(false);
        }
        private void OnVisionEnd()
        {
            SearchUtilities.Find("Prisoner_Vision").SetActive(false);
            TransformTotemRings();
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/Elevator_Pivot/Floor_Bottom/Prefab_IP_DreamLibraryPedestal/PressurePlateRoot/DreamLanternSocket").SetActive(false);
            Locator.GetPauseCommandListener().RemovePauseCommandLock();

            Invoke("EnableElevator", 50);
        }
        private void TransformTotemRings()
        {
            var ring_1 = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem/CodeDisplay/Props_IP_CodeTotem/rings/ring01").GetComponent<RotaryDial>();
            var ring_2 = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem/CodeDisplay/Props_IP_CodeTotem/rings/ring02").GetComponent<RotaryDial>();
            var ring_3 = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem/CodeDisplay/Props_IP_CodeTotem/rings/ring03").GetComponent<RotaryDial>();
            var ring_4 = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem/CodeDisplay/Props_IP_CodeTotem/rings/ring04").GetComponent<RotaryDial>();
            var ring_5 = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem/CodeDisplay/Props_IP_CodeTotem/rings/ring05").GetComponent<RotaryDial>();

            ring_1._symbolSelected = 1;
            ring_1.Awake();

            ring_2._symbolSelected = 2;
            ring_2.Awake();

            ring_3._symbolSelected = 3;
            ring_3.Awake();

            ring_4._symbolSelected = 2;
            ring_4.Awake();

            ring_5._symbolSelected = 1;
            ring_5.Awake();

            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/IslandsRoot/IslandPivot_C/Island_C/Interactibles_Island_C/Prefab_IP_DW_CodeTotem").GetComponent<EclipseCodeController4>().CheckForCode();
            SearchUtilities.Find("DreamWorld_Body/TotemCodePromptVolume").SetActive(true);
        }
        private void EnableElevator() => SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/Elevator_Pivot/Floor_Bottom/Prefab_IP_DreamLibraryPedestal/PressurePlateRoot/DreamLanternSocket").SetActive(true);
    }   
}



