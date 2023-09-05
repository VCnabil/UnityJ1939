using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
public class ArduinoSerial : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM4", 115200); // COM port and baud rate
    string receivedData = "";

    void Start()
    {
        // Open the serial port
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
        }
        // Set read timeout
        serialPort.ReadTimeout = 100;
    }

    void Update()
    {
        // Send data to Arduino
        // PGN.b0.b1.b2.b3.b4.b5.b6.b7
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendData("6,18FF02,09,0,0,0,0,A7,03,69,05");
        }
        else
        if (Input.GetKeyDown(KeyCode.D))
        {
            SendData("6,18FF02,09,0,0,0,0,C6,07,2C,0B");
        }


        // Read data from Arduino
        try
        {
            receivedData = serialPort.ReadLine();
            Debug.Log("Received: " + receivedData);
        }
        catch (System.TimeoutException)
        {
            // Handle timeout exception if needed
        }
    }

    void SendData(string data)
    {
        if (serialPort.IsOpen)
        {
            serialPort.WriteLine(data);
            Debug.Log("Sent: " + data);
        }
    }

    void OnApplicationQuit()
    {
        // Close the serial port when the application quits
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
