using NewHorizons.Utility;
using UnityEngine;
using NewHorizons.Builder.Props;

namespace ChristmasStory.Components.Animation
{

    internal class PrisonerAnimationController : MonoBehaviour
    {
        private PrisonerEffects _animator;
        private VisionTorchTarget _visionTorchTarget;

        public static PrisonerAnimationController Instance;
        public void Start()
        {
            Instance = this;
            _animator = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<PrisonerEffects>();        
            _visionTorchTarget = SearchUtilities.Find("Prisoner_Vision").GetComponent<VisionTorchTarget>();           
            _visionTorchTarget.onSlidesComplete = Instance.OnVisionEnd;
            _visionTorchTarget.onSlidesStart = Instance.OnVisionStart;                       
        }

        public void PlayLightsUp()
        {
            Instance._animator.PlayTurnOnLightsAnimation();
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<VisionTorchItem>()._interactable = true;
            var prisonersOriginalTorch = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector");
            var prisonersTorch = SearchUtilities.Find("Prisoner_Vision_Torch");
            SearchUtilities.Find("Prisoner_Clone").SetActive(true);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Effects_IP_SIM_VisionTorch").SetActive(true);
            prisonersTorch.SetActive(true);           
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
        } 
    }   
}



