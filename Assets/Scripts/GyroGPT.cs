using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GyroGPT : MonoBehaviour
{
    public string portName = "COM4";
    public int baudRate = 115200;

    private SerialPort serialPort;
    [SerializeField] private Transform capsulius;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.BytesToRead > 0)
        {
            string message = serialPort.ReadLine();
            //Debug.Log($"Recieved full message: {message}");
            //if (message.StartsWith("<START>") && message.EndsWith("<END>"))
            //{
                // Remove start and end statements
                 message = message.Substring("<START>".Length, message.Length - "<START>".Length - "<END>".Length);

                
                // Split message into three sets of coordinates
                string[] parts = message.Split(',');
                if (parts.Length == 3)
                {
                    float x1 = float.Parse(parts[0]);
                    float y1 = float.Parse(parts[1]);
                    float z1 = float.Parse(parts[2]);
                    
                    // Do something with the coordinates
                    Debug.Log("Received coordinates: (" + x1 + ", " + y1 + ", " + z1 + ")");
                    capsulius.localRotation = Quaternion.Euler(x1,y1,z1);
                }
            //}
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
