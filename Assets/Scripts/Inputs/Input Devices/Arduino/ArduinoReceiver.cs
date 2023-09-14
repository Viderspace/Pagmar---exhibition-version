using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using Inputs.Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Inputs.Input_Devices.Arduino
{
    public class ArduinoReceiver : MonoBehaviour
    {

        [SerializeField] private ArduinoAddresses ArduinoInterface;

        // NOTICE: The current address is valid when the board is connected to the 'Programming Port'
        // and into my 'Transcend' USB hub, which is connected
        // to the leftmost USB port (right next to the Headphone jack) in my Macbook Pro.
        // Other addresses may be invalid
        // [SerializeField] private string boardAddress = "usbmodem";
        [SerializeField] private string boardAddress = "usbserial";

        // NOTICE: Delay time must be the same as the delay time in the Arduino code!
        public static int arduinoDelay = 25;
        public int baudRate = 9600;

        private Thread _thread;
        private static SerialPort _serialPort;
        private bool _feedIsAlive;

        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text debugText;


        private void OnInputTextChanged(string newBoardName)
        {
            boardAddress = newBoardName;
            ClosePort();
            Connect();
        }



        #region MonoBehavior

        private void Awake()
        {
            inputField.onValueChanged.AddListener(OnInputTextChanged);
            if (!Connect()) return;
            // var Receiver = GetComponent<ArduinoReceiver>();

            _thread = new Thread(ThreadLoop);
            _thread.Start();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            if (_serialPort.IsOpen && debugText != null)
            {
                _serialPort.RtsEnable = true;
                debugText.text = "Arduino Receiver: No Signal yet " + _serialPort.RtsEnable;
            }

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
                _serialPort.Close();
            }

            yield return null;

        }

        private bool Connect()
        {
            string deviceName = "";
            foreach (var name in SerialPort.GetPortNames())
            {
                if (name.Contains(boardAddress) || name.Contains("220"))
                {
                    deviceName = name;
                }
            }
            
            if (deviceName == "")
            {
                print( "Arduino Receiver Board was not found");
                if (debugText != null) debugText.text = "Arduino Receiver Board was not found";
                DestroyImmediate(this);
                return false;
            }

            try
            {
                _serialPort = new SerialPort(deviceName, baudRate);
                _serialPort.ReadTimeout = arduinoDelay;
                _serialPort.WriteTimeout = arduinoDelay;
                _serialPort.Open();
            }
            catch (Exception e)
            {
                print("Arduino is not connected: " + e.Message);
                if (debugText != null) debugText.text = "Arduino Receiver Has failed";
                DestroyImmediate(this);
                return false;
            }

            return true;
        }

        private static string ReadData(int timeout = 50)
        {
            _serialPort.ReadTimeout = timeout;
            string msg;
            try
            {
                var all = _serialPort.ReadLine();
                // var all = _serialPort.ReadExisting();
                return all;
            }
            catch (Exception e )
            {
                return null;
            }
        }

        private string incomingMassage;
        void ThreadLoop()
        {
          if (!_serialPort.IsOpen) return;
            for (;;)
            {
                 incomingMassage = ReadData();
                if (!string.IsNullOrEmpty(incomingMassage))
                {
                    print("Received: (" + incomingMassage + ") from Arduino");
                }

                Thread.Sleep(arduinoDelay);
            }
        }

        private void Update()
        {
            if (incomingMassage != null)
            {
                OnDataReceived(incomingMassage);
                incomingMassage = null;
                
                if (_feedIsAlive == false && debugText!= null)
                {
                    debugText.text = "Arduino Receiver: Connected";
                    _feedIsAlive = true;
                }
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


        public void OnDataReceived(string data)
        {
           if(debugText!=null ) debugText.text = "Arduino Received Message: (" + data+ ")";
            ArduinoInterface.InterpretMassage(data);
        }
    }
}
