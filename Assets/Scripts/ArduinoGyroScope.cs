using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoGyroScope : MonoBehaviour
{
    public GameObject objectToRotate;
    SerialPort serialPort;
    string portName = "COM3"; // Change this to the correct port name
    int baudRate = 115200; // Change this to the baud rate used by the Arduino

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            // Process the data here
            Debug.Log(data);

            // Split the data into three parts (assuming it's in the format "x,y,z")
            string[] parts = data.Split(',');
            float x, y, z;
            if (float.TryParse(parts[0], out x) && float.TryParse(parts[1], out y) && float.TryParse(parts[2], out z))
            {
                // Create a new Quaternion from the Euler angles
                Quaternion rotation = Quaternion.Euler(x, y, z);

                // Set the rotation of the object
                objectToRotate.transform.rotation = rotation;
            }
        }
    }

    void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
