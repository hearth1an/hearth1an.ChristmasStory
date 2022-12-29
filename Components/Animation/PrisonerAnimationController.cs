using OWML.Common;
using OWML.ModHelper;
using NewHorizons.Utility;
using UnityEngine;
using NewHorizons.Builder.Props;
using UnityEngine.Events;
using OW.Utilities;


namespace ChristmasStory.Components.Animation
{
    internal class PrisonerAnimationController : MonoBehaviour
    {
        private PrisonerEffects _animator;
        private VisionTorchTarget _visionTorchTarget;
        private GhostEffects _ghostEffects;

        public static PrisonerAnimationController Instance;
        public void Start()
        {
            Instance = this;


            _animator = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<PrisonerEffects>();
            // _visionTorchTarget = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/VisionStaffDetector").GetComponent<VisionTorchTarget>();
           
            _visionTorchTarget = SearchUtilities.Find("Prisoner_Vision").GetComponent<VisionTorchTarget>();
            _ghostEffects = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<GhostEffects>();

            OnVisionStart();
        }

        public void PlayLightsUp()
        {
            Instance._animator.PlayTurnOnLightsAnimation();         
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<VisionTorchItem>()._interactable = true;
        }
        public void OnVisionStart()
        {
            Instance._visionTorchTarget.onSlidesStart = Instance._animator.PlayExperienceVisionAnimation;
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (14)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (14)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (15)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (15)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (5)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (5)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (6)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (6)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (7)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (7)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (6)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (6)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (13)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (13)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_L (8)").SetActive(false);
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/SarcophagusController/PrisonerFootprints/Decal_DW_Footprint_R (8)").SetActive(false);

            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector").SetActive(false);
        }




        public void CancelDream()
        {
            Instance._ghostEffects.Anim_SnapNeck();
        }



        /*
       public void RevealWallText()
       {
           Instance._nomaiText.Show();
       }
       public void StopWriteWallText()
       {
           Instance._animator.StopWritingMessage(gestureToText: true);
       }
       public void GestureToStone()
       {
           Instance._animator.PlayGestureToWordStones();
       }
       public void RevealStone()
       {
           Instance._conversationStone.Reveal();
           Instance._audioSource.PlayOneShot(AudioType.SolanumSymbolReveal, 0.5f);
       }
       public void StopSound()
       {
           Instance._audioSource.Stop();
       }
       public void TransformWatchingTarget()
       {
           Instance._animator.StartWatchingPlayer();
           Instance._animator._playerCameraTransform = _sharedStone.transform;
       }
       public void StopWatching()
       {
           Instance._animator.StopWatchingPlayer();
       }
       public void PlayWholeAnimation()
       {
           Invoke("WriteWallText", 1f);
           Invoke("RevealWallText", 5f);
           Invoke("StopWriteWallText", 6f);
           Invoke("GestureToStone", 7f);
           Invoke("RevealStone", 10.5f);
           Invoke("StopSound", 11.4f);
           Invoke("StopWatching", 20f);
       }
       public void SolanumAnimEvent()
       {
           Invoke("TransformWatchingTarget", 2f);
           Invoke("PlayWholeAnimation", 2f);
       }

       */

    }
}
