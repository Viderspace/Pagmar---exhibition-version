using UnityEngine;
using System.IO.Ports;
public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort; // declare a SerialPort variable

    public int pinA0State; // public variable to hold the state of pin A0
    public int pin0State; // public variable to hold the state of pin 0
    public string comID = "COM9";

    void Start()
    {
        serialPort = new SerialPort(comID, 9600); // initialize the SerialPort
        serialPort.Open(); // open the serial port
        InvokeRepeating("SerialDataReading", 0f, 0.01f); // schedule repeated calls to SerialDataReading method
    }

    // void Update()
    // {
    //     if (pin0State == 1)
    //     {
    //         rotatingCube.transform.Rotate(Vector3.up, pinA0State * Time.deltaTime);
    //     }
    //     else if (pin0State == 0)
    //     {
    //         rotatingCube.transform.Rotate(Vector3.up, 0 * Time.deltaTime);
    //     }
    // }

    void SerialDataReading()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string serialData = serialPort.ReadLine(); // read the serial data as a string

            print(serialData);
            // if (serialData.StartsWith("pin A0 is: ")) // check if the serial data is for pin A0
            // {
            //     pinA0State = int.Parse(serialData.Substring(11)); // extract the pin A0 state from the serial data and update the public variable
            // }
            // else if (serialData.StartsWith("pin 0 is: ")) // check if the serial data is for pin 0
            // {
            //     pin0State = int.Parse(serialData.Substring(10)); // extract the pin 0 state from the serial data and update the public variable
            // }
        }
        else
        {
            print("no bytes to read");
        }
    }

    void OnDisable()
    {
        if (serialPort != null && serialPort.IsOpen) // check if the serial port is open before closing it
        {
            serialPort.Close(); // close the serial port
        }
    }
}