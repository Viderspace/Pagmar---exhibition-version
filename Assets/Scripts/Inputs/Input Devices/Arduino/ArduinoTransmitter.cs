using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Inputs.Input_Devices.Arduino;
using Inputs.Scriptable_Objects;
using UnityEngine;


public class ArduinoTransmitter : MonoBehaviour
{
    public static ArduinoTransmitter Instance;
    // NOTICE: The current address is valid when the board is connected to the 'Programming Port'
    // and into my 'Transcend' USB hub, which is connected
    // to the leftmost USB port (right next to the Headphone jack) in my Macbook Pro.
    // Other addresses may be invalid
    private const string boardAddress = "/dev/cu.usbmodem11201";

    // NOTICE: Delay time must be the same as the delay time in the Arduino code!
    public static int arduinoDelay = 25;
    public int baudRate = 115200;

    // lED Controller Objects
    [SerializeField] private List<LedController> ledControllers;

    private Thread _thread;
    private static SerialPort _serialPort;
    
    private static readonly ArduinoCommands commandQueue = new ArduinoCommands();
    
    #region LED API
    public static void RegisterLed(byte LEDPin) => commandQueue.RegisterLed(LEDPin);
    
    private static void SetLed(byte LEDPin, bool turnOn) =>commandQueue.SetLed(LEDPin, turnOn);
    
    #endregion
    

    #region MonoBehavior

    private void Awake()
    {
        Instance = this;
        Connect();
        // var Receiver = GetComponent<ArduinoReceiver>();
        
        _thread = new Thread(ThreadLoop);
        _thread.Start();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        foreach (var led in ledControllers)
        {
            led.Setup();
            led.StateChanged += SetLed;
            yield return null;
        }


        print("<Arduino is ready>");
        yield return null;
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(Disconnect());
    }

    private void OnDestroy()
    {
        ClosePort();
    }

    private void OnDisable()
    {
        ClosePort();
    }
    

    #endregion

    #region Methods

    public void ClosePort()
    {
        if (_thread != null)
        {
            // StopCoroutine(_thread);
            _thread.Abort();
            _thread = null;
        }

        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
            _serialPort = null;
        }
    }

    public IEnumerator Disconnect()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            foreach (var led in ledControllers)
            {
                led.StateChanged -= SetLed;
                SetLed(led.pin, false);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }

    public void Connect()
    {
        try
        {
            _serialPort = new SerialPort(boardAddress, baudRate);
            _serialPort.ReadTimeout = arduinoDelay;
            _serialPort.WriteTimeout = arduinoDelay;
            _serialPort.Open();
        }
        catch (Exception e)
        {
            print("Arduino is not connected: " + e.Message);
            DestroyImmediate(this);
        }

    }

    private static string ReadData(int timeout = 50)
    {
        _serialPort.ReadTimeout = timeout;
        string msg;
        try
        {
            // msg = _serialPort.ReadLine();
            var all = _serialPort.ReadExisting();
            return all;
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    static void ThreadLoop()
    {
        if (!_serialPort.IsOpen) return;
        for (;;)
        {

            if (!commandQueue.IsEmpty)
            {
                var protocolMsg= commandQueue.GetNextCommand();
                _serialPort.WriteLine(protocolMsg);
                print("Sending to arduino: ("+protocolMsg+")");
            }
            

            var incomingMassage = ReadData();
            if (!string.IsNullOrEmpty(incomingMassage))
            {
                print("Received: (" + incomingMassage + ") from Arduino");
                OnDataReceived(incomingMassage);
               
            }
            Thread.Sleep(arduinoDelay/2);
        }
    }

    public bool GetPortStatus()
    {
        if (_serialPort != null)
        {
            return _serialPort.IsOpen;
        }

        return false;
    }

    #endregion
    
    public static event Action<string> DataReceived;
    
    public static void OnDataReceived(string data)
    {
        DataReceived?.Invoke(data);
    }
    
}



