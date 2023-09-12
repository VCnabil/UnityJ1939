using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoSerial : MonoBehaviour
{

    public GameObject thecube;
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

            if (receivedData[0] != 'U')
            {
                Debug.Log("Invalid input: First character is not 'U'");
                return;
            }
            else if (receivedData[1] != 'h') {
                Debug.Log("Invalid input: this is not a hex message");
                return;
            }

            string[] parts = receivedData.Split('|');
            //
            //  Uhex | 1 | FF02 | 9 | 00 | 00 | 00 | 00 | CC | 07 | FC | 08 | uhex
            //                        4    5
            //                        FA   13 -> 13FA = 5114
            //                        D1   1E -> 1ED1 = 7889
            //                        10   27 -> 2710 = 10000
            // Combine bytes for Magnitude

           // string testByte_x = "D1";// parts[4];
           // string testByte_y = "1E";// parts[5];
           // ushort magBytes_z = Convert.ToUInt16(testByte_y + testByte_x, 16);
           // float Magnitude_z = (float)magBytes_z;
           // float MagnitudeRectified = Magnitude_z / 100.0f;
           //string outtest = testByte_x + " " + testByte_y + "     is " + magBytes_z + " rectified= " + MagnitudeRectified;
           // Debug.Log("b0 b1 : " + outtest);

            ushort magnitudeBytes = Convert.ToUInt16(parts[5] + parts[4], 16);
            float Magnitude = (float)magnitudeBytes/100.0f;

            //string testByteANG_x = "C1";// parts[4];
            //string testByteANG_y = "36";// parts[5];
            //ushort magBytesANG_z = Convert.ToUInt16(testByteANG_y + testByteANG_x, 16);
            //float MagnitudeANG_z = (float)magBytesANG_z;
            //float MagnitudeRectifiedANG = MagnitudeANG_z / 100.0f;
            //string outtest = testByteANG_x + " " + testByteANG_y + "     is " + magBytesANG_z + " rectified= " + MagnitudeRectifiedANG;
            //Debug.Log("b0 b1 : " + outtest);


            // Combine bytes for Angle
            ushort angleBytes = Convert.ToUInt16(parts[7] + parts[6], 16);
            float Angle = (float)angleBytes/100.0f;

            // Combine bytes for SliderL
            ushort sliderLBytes = Convert.ToUInt16(parts[9 ] + parts[8], 16);
            float SliderL = sliderLBytes;

            // Combine bytes for SliderR
            ushort sliderRBytes = Convert.ToUInt16(parts[11] + parts[10], 16);
            float SliderR = sliderRBytes;


            string output = "Magnitude= " + Magnitude + " Angle= " + Angle + " SliderL= " + SliderL + " SliderR= " + SliderR;
            Debug.Log(output);



            thecube.transform.eulerAngles = new Vector3(
            thecube.transform.eulerAngles.x,
            Angle*-1.0f,
            thecube.transform.eulerAngles.z
        );

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
