using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Synth.Sequencer.MidiButtons;
using TMPro;
using UnityEngine;

public class DebugSynthStatusDisplay : MonoBehaviour
{
    // [SerializeField] SequencerButton lowestLeftButton;
    [SerializeField] private TMP_Text synthDataText;
    float time = 0;

    private void Start()
    {
        synthDataText = GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }

        time = .5f;
        SynthSnapshot snapshot = new SynthSnapshot(Singleton.Instance.SynthController);
        synthDataText.text = snapshot.Print();
        // synthDataText.text += "\n" + lowestLeftButton.fillDebug + " " + lowestLeftButton.strokeDebug;
    }
}
