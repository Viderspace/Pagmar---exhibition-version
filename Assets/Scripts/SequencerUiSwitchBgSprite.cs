using Scriptable_Objects;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SequencerUiSwitchBgSprite : MonoBehaviour
{
    [SerializeField] SequencerModeVariable sequencerMode;
    [SerializeField] private Image imageRenderer;
    [SerializeField] private Sprite defaultBG;
    [SerializeField] private Sprite recordBG;

    private void SetBgSprite(SynthController.SequencerState newState)
    {
        switch (newState)
        {
            case SynthController.SequencerState.Idle:
            case SynthController.SequencerState.Running:
                imageRenderer.sprite = defaultBG;
                break;
            case SynthController.SequencerState.Recording:
                imageRenderer.sprite = recordBG;
                break;
        }
    }
    
    
    
    private void OnEnable()
    {
        sequencerMode.ValueChanged += SetBgSprite;
    }
    private void OnDisable()
    {
        sequencerMode.ValueChanged -= SetBgSprite;
    }
}
