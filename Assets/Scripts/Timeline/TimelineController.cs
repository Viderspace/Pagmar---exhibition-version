using System.Collections;
using Inputs;
using Scriptable_Objects;
using Timeline_Extentions;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Kernel.System
{
    public class TimelineController : MonoBehaviour
    {
        public static TimelineController Instance { get; private set; }

        [SerializeField]private SpecialTimelineEvents specialTimelineEvents;
        
        public SpecialTimelineEvents SpecialEvents => specialTimelineEvents;
        
        public PlayableDirector playableDirector;
        public float TimePosition => (float) playableDirector.time;

        #region MonoBehaviour
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            ResetToInit();
        }

        private void OnEnable()
        {
            InputManager.PhoneHangup += ResetToInit;
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= ResetToInit;
        }
        
        #endregion

        #region Class Methods


        public void MoveToEnd()
        {
            // playableDirector.time = playableDirector.duration - 25f;
            // playableDirector.Pause();
            StartCoroutine(MoveToEndAndPause());
        }

        IEnumerator MoveToEndAndPause()
        {
            playableDirector.time = 1117f;
            playableDirector.Play();
            yield return new WaitForSeconds(1f);
            playableDirector.Pause();
        }

 

        public void StartTimeline()
        {
            playableDirector.time = 0;
            playableDirector.Play();
        }

        public void SkipToAndPlay(DestinationMarker positionMarker)
        {
            playableDirector.Pause();
            // print("SkipToAndPlay called with time: "+positionMarker.time);
            playableDirector.time = positionMarker.time;
            playableDirector.Play();
        }
        

        
        public void PauseTimeline()
        {
            playableDirector.Pause();
        }
        
        public void ResumeTimeline()
        {
            playableDirector.Play();
        }

        public void ResetToInit()
        {
            playableDirector.time = 0;
            playableDirector.Pause();
        }
        
        #endregion

        #region Special Events
        
        public void HoldAndStartSequencerCodeChallenge() => specialTimelineEvents.HoldAndStartSequencerCodeChallenge();
        
        public void HoldUntilRecordButtonIsPressed() => specialTimelineEvents.HoldUntilRecordButtonIsPressed();

        public void  HoldUntilKeypadIsPressed() => specialTimelineEvents.HoldUntilKeypadIsPressed();
        
        public void HoldUntilReDialedKeyIsPressed() => specialTimelineEvents.HoldUntilRedialKeyIsPressed();
        
        public void HoldUntilFilterIsEnabled() => specialTimelineEvents.HoldUntilFilterIsEnabled();
        
        public void HoldUntilFilterIs2600HZ() => specialTimelineEvents.HoldUntilFilterIs2600HZ();
        
        public void EnableAdsrOn() => specialTimelineEvents.EnableAdsrOn();

        public void HoldUntilAdsrIsMatched() => specialTimelineEvents.HoldUntilAdsrIsMatched();
        
        public void HoldAndWaitForCvTrigger() => specialTimelineEvents.HoldAndWaitForCvTrigger();
        
        public void EnterSandboxMode() => specialTimelineEvents.EnterSandboxMode();

        #endregion

    }

}