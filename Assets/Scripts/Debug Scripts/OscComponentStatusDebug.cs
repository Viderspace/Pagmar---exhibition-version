
using extOSC;
using Reaktor_Communication;
using TMPro;
using UnityEngine;

public class OscComponentStatusDebug : MonoBehaviour
{
    TMP_Text _debugText;
    OSCReceiver _receiver;
    OSCTransmitter _transmitter;
    private bool Rok => _receiver.IsStarted;
    private bool Tok => _transmitter.IsStarted;
    // Start is called before the first frame update
    void Start()
    {
        _debugText = GetComponent<TMP_Text>();
        _debugText.text = "";
        _receiver = ReaktorController.Instance.Receiver;
        _transmitter = ReaktorController.Instance.Transmitter;
    }

    private void PrintStatus(string receiverStatus, string transmitterStatus)
    {
        _debugText.text = $"Receiver: " +
                          $"{receiverStatus}\nTransmitter: {transmitterStatus}";
    }


    private string TranStatus()
    {
        return $"<color={(Tok ? "green" : "red")}>" +
               (Tok ? "Connected " : "Disconnected ") +
               " | port " + _transmitter.RemotePort + " </color>";
    }

    private string RecStatus()
    {
        return $"<color={(Rok ? "green" : "red")}>" +
               (Rok ? "Connected" : "Disconnected") +
               " | port " + _receiver.LocalPort + " </color> local host " + _receiver.LocalHost + " started: " + _receiver.IsStarted;
    }
    void FixedUpdate()
    {
        if (_receiver == null || _transmitter == null) return;
        PrintStatus(RecStatus(), TranStatus());
    }
}
