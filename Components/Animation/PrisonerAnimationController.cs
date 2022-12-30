using OWML.Common;
using OWML.ModHelper;
using NewHorizons.Utility;
using UnityEngine;
using NewHorizons.Builder.Props;
using System;



namespace ChristmasStory.Components.Animation
{

    internal class PrisonerAnimationController : MonoBehaviour
    {
        private PrisonerEffects _animator;
        private VisionTorchTarget _visionTorchTarget;
        private GhostEffects _ghostEffects;
        private PrisonerBrain _prisonerBrain;
        private VisionTorchSocket _visionTorchSocket;
        private PrisonerDirector _prisonerDirector;
        private OWItem _visionTorchItem;



        public static PrisonerAnimationController Instance;
        public void Start()
        {
            Instance = this;


            _animator = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<PrisonerEffects>();
            // _visionTorchTarget = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/VisionStaffDetector").GetComponent<VisionTorchTarget>();
            _prisonerBrain = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner").GetComponent<PrisonerBrain>();
            _visionTorchTarget = SearchUtilities.Find("Prisoner_Vision").GetComponent<VisionTorchTarget>();
            _ghostEffects = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM").GetComponent<GhostEffects>();
            _visionTorchSocket = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostNodeMap_PrisonCell_Lower/Prefab_IP_GhostBird_Prisoner/Ghostbird_IP_ANIM/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:ClavicleL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ShoulderL/Ghostbird_Skin_01:Ghostbird_Rig_V01:ElbowL/Ghostbird_Skin_01:Ghostbird_Rig_V01:WristL/Ghostbird_Skin_01:Ghostbird_Rig_V01:HandAttachL/VisionTorchSocket").GetComponent<VisionTorchSocket>();
            _prisonerDirector = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Ghosts_PrisonCell/GhostDirector_Prisoner").GetComponent<PrisonerDirector>();
            _visionTorchItem = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<OWItem>();
                        
            _visionTorchTarget.onSlidesComplete = Instance.OnVisionEnd;
        }

        public void PlayLightsUp()
        {
            Instance._animator.PlayTurnOnLightsAnimation();
            SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Sector_PrisonCell/Interactibles_PrisonCell/PrisonerSequence/VisionTorchWallSocket/Prefab_IP_VisionTorchItem").GetComponent<VisionTorchItem>()._interactable = true;
            var prisonersOriginalTorch = SearchUtilities.Find("DreamWorld_Body/Sector_DreamWorld/Sector_Underground/Interactibles_Underground/Prefab_IP_VisionTorchProjector");
            var prisonersTorch = SearchUtilities.Find("Prisoner_Vision_Torch");
            SearchUtilities.Find("Prisoner_Clone").SetActive(true);

            prisonersTorch.SetActive(true);

            // Destroy(prisonersOriginalTorch);
        }
        public void OnVisionEnd()
        {            
            SearchUtilities.Find("Prisoner_Vision").SetActive(false);
            _prisonerBrain.BeginBehavior(PrisonerBehavior.WaitForTorchReturn, 4f);
            
        }
       
    }
}


        /*
        private void EnterBehaviour(PrisonerBehavior behavior)
        {
            switch (behavior)
            {
                case PrisonerBehavior.Emerge:
                    this._animator.OnRevealAnimationComplete += this.OnFinishEmergeAnimation;
                    this._animator.PlayRevealAnimation();
                    return;
                case PrisonerBehavior.ExperienceVision:
                    this._animator.PlayExperienceVisionAnimation();
                    return;
                case PrisonerBehavior.WaitForTorchReturn:
                    this._animator.PlayWaitForTorchReturnAnimation();
                    return;
                case PrisonerBehavior.ProjectVision:
                    if (behavior == PrisonerBehavior.FetchTorch)
                    {
                        this._animator.PlayProjectVisionAnimation();
                        return;
                    }
                    this._animator.PlayDefaultAnimation();
                    return;
            }
        }
        */




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



