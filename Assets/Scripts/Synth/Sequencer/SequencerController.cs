using System;
using System.Collections;
using Inputs;
using Reaktor_Communication;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;

namespace Synth.Sequencer
{
    public class SequencerController : MonoBehaviour
    {
        public static SequencerController Instance { get; private set; }

        #region Inspector
        [SerializeField] private SequencerModeVariable globalSequencerMode;
        private SynthController.SequencerState _currentState;

        [SerializeField] private GameObject cursorPrefab;
        [SerializeField] private GridGenerator gridGenerator;
        [SerializeField] private bool cursorSmoothMovement = false;

        #endregion

        #region Properties

        public int CurrentStep { get; private set; }

        public bool IsRunning => _runningTask != null && _runningTask.Running;

        public float NoteLength => Singleton.Instance.SynthController.NoteLength;

        #endregion

        #region Fields

        private GameObject _cursorObject;
        private Task _runningTask;
        private float _cursorLeftPos;
        private int _totalColumns;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            globalSequencerMode.ValueChanged += SwitchMode;
        }
        
        private void OnDisable()
        {
            globalSequencerMode.ValueChanged -= SwitchMode;
        }

        private void SwitchMode(SynthController.SequencerState newState)
        {
            switch (newState)
            {
                case SynthController.SequencerState.Recording:
                {
                    if (_currentState == SynthController.SequencerState.Running) PlaySequencer(false);
                    SetRecordMode(true);
                    break;
                }
                case SynthController.SequencerState.Running:
                {
                    if (_currentState == SynthController.SequencerState.Recording) SetRecordMode(false);
                    PlaySequencer(true);
                    break;
                }
                case SynthController.SequencerState.Idle when _currentState == SynthController.SequencerState.Recording:
                    SetRecordMode(false);
                    break;
                case SynthController.SequencerState.Idle when _currentState == SynthController.SequencerState.Running:
                    PlaySequencer(false);
                    break;
            }
            _currentState = newState;

        }

        public static event Action<int, int> SelectNoteFromKeypad;
        public static event Action<int> UnselectCurrentStep;

        public void OnSelectNoteFromKeypad(Keypad keypadKey)
        {
            var noteValue = keypadKey.MidiValue;
            SelectNoteFromKeypad?.Invoke(noteValue, CurrentStep);
            // MoveRight();
        }

        private void MoveRightInDelay(Keypad ignored)
        {
            MoveRight();
            // StartCoroutine(KeypadMoveWhenKeyIsReleased());
        }
        
        private IEnumerator KeypadMoveWhenKeyIsReleased()
        {
            yield return new WaitForSeconds(0.3f);
            MoveRight();
        }

        private void Start()
        {
            _totalColumns = gridGenerator.columns;
            _cursorObject = cursorPrefab;
            _cursorLeftPos = _cursorObject.transform.position.x;
        }

        public void MoveRight()
        {
            if (IsRunning) return;
            CurrentStep = (CurrentStep >= _totalColumns - 1) ? 0 : CurrentStep + 1;
            MoveToStep(CurrentStep);
        }

        public void MoveLeft()
        {
            if (IsRunning) return;
            CurrentStep = (CurrentStep < 0) ? _totalColumns - 1 : CurrentStep - 1;
            MoveToStep(CurrentStep);
        }

        private void ToggleRun()
        {
            if (_runningTask == null)
            {
                _runningTask = new Task(cursorSmoothMovement ? SmoothJump() : DiscreteJump());
            }
            else if (_runningTask.Running)
            {
                _runningTask.Stop();
                _runningTask = null;
                MoveToStep(0);
            }
        }

        private void PlaySequencer(bool on)
        {
            if (on)
            {
                MoveToInit();
                _runningTask = new Task(cursorSmoothMovement ? SmoothJump() : DiscreteJump());
            }
            else if (_runningTask != null )
            {
                _runningTask.Stop();
                _runningTask = null;
                MoveToInit();
            }
        }

        private void SetRecordMode(bool record)
        {
            if (record)
            {
                InputManager.UpdateClearButtonPressed += ClearGrid;
                InputManager.UpdateRightArrowPressed += MoveRight;
                InputManager.UpdateLeftArrowPressed += MoveLeft;
                InputManager.KeypadButtonPressed += OnSelectNoteFromKeypad;
                InputManager.KeypadButtonReleased += MoveRightInDelay;
                InputManager.KeypadClearCurrentStep += ClearCurrentStep;
            }

            else
            {
                InputManager.UpdateClearButtonPressed -= ClearGrid;
                InputManager.UpdateRightArrowPressed -= MoveRight;
                InputManager.UpdateLeftArrowPressed -= MoveLeft;
                InputManager.KeypadButtonPressed -= OnSelectNoteFromKeypad;
                InputManager.KeypadButtonReleased -= MoveRightInDelay;
                InputManager.KeypadClearCurrentStep -= ClearCurrentStep;
            }
            MoveToInit();
        }

        public void ClearCurrentStep()
        {
            UnselectCurrentStep?.Invoke(CurrentStep);
            MoveRight();
        }

        #endregion

        #region Private Methods

        private void MoveToStep(int step)
        {
            var currentPosition = _cursorObject.transform.position;
            currentPosition.x = _cursorLeftPos + step * 70.43854f;
            _cursorObject.transform.position = currentPosition;
        }

        private float CursorPosition
        {
            get => _cursorObject.transform.position.x;
            set
            {
                var currentPosition = _cursorObject.transform.position;
                currentPosition.x = value;
                _cursorObject.transform.position = currentPosition;
            }
        }

        private void MoveToInit()
        {
            CurrentStep = 0;
            CursorPosition = _cursorLeftPos;
        }

        #endregion

        #region Coroutines

        private IEnumerator DiscreteJump()
        {
            for (;;)
            {
                for (int i = 0; i < _totalColumns; i++)
                {
                    CurrentStep = i;
                    yield return new WaitForSeconds(NoteLength);
                    MoveToStep(CurrentStep);
                    OnNoteTrigger((byte) i);
                }
            }
            // ReSharper disable once IteratorNeverReturns
     
        }

        private IEnumerator SmoothJump()
        {
            bool firstIteration = true;
            var position = _cursorObject.transform.position;
                position = new Vector3(
                    _cursorLeftPos, position.y,
                    position.z);
                _cursorObject.transform.position = position;
            
            for (;;)
            {
                for (int i = 0; i < _totalColumns; i++)
                {
                    CurrentStep = i;
                    float startTime = Time.time;
                    Vector3 startPosition = _cursorObject.transform.position;
                    Vector3 endPosition = startPosition;
                    endPosition.x += gridGenerator.unitWidth;

                    while (Time.time < startTime + NoteLength)
                    {
                        float t = (Time.time - startTime) / (NoteLength);
                        _cursorObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                        yield return null;
                    }

                    if (CurrentStep == 0 && !firstIteration)
                    {
                        position = _cursorObject.transform.position;
                        position = new Vector3(
                            _cursorLeftPos, position.y,
                            position.z);
                        _cursorObject.transform.position = position;
                    }
                    firstIteration = false;
                    yield return null;
                    OnNoteTrigger((byte) i);
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator SmoothJump(int step)
        {
            float startTime = Time.time;
            Vector3 startPosition = _cursorObject.transform.position;
            Vector3 endPosition = startPosition;
            endPosition.x += gridGenerator.unitWidth;

            while (Time.time < startTime + NoteLength)
            {
                float t = (Time.time - startTime) / (NoteLength);
                _cursorObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            if (step == 0)
            {
                var position = _cursorObject.transform.position;
                position = new Vector3(
                    _cursorLeftPos, position.y,
                    position.z);
                _cursorObject.transform.position = position;
            }

            yield return null;
        }

        // private IEnumerator RunCursor()
        // {
        //     for (;;)
        //     {
        //         for (int i = 0; i < _totalColumns; i++)
        //         {
        //             CurrentStep = i;
        //             yield return cursorSmoothMovement ? SmoothJump(i) : DiscreteJump(i);
        //             OnNoteTrigger((byte) i);
        //         }
        //     }
        //     // ReSharper disable once IteratorNeverReturns
        // }

        #endregion

        #region Event Actions

        public static event Action NoteReset;

        public void ClearGrid()
        {
            if (IsRunning) return;
            NoteReset?.Invoke();
        }

        public static event Action<int> NoteTrigger;
        public bool kickOn = true;

        private void OnNoteTrigger(int timeStamp)
        {
            NoteTrigger?.Invoke(timeStamp);
            if (kickOn && (timeStamp % 4 == 0))
        {
            ReaktorController.Instance.SendKick();
        }
        }
        
        


        #endregion
    }
}