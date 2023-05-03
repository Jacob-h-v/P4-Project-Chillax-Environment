using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GyroReceiver : MonoBehaviour
{
    public string portName = "COM5";
    public int baudRate = 115200;
    private float debugDelay = 1f;
    private float x1;
    private float y1;
    private float z1;
    private Quaternion previousRotationData;
    private Quaternion newRotationData;

    private SerialPort serialPort;
    [SerializeField] private Transform capsulius;

    void Start()
    {
        // Opening the connection to the Arduino / gyro
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        // Setting default player object rotation
        capsulius.localRotation = Quaternion.identity;
        // Starts reporting sensor data on a preset delay
        StartCoroutine(DelayDebugLog());
    }

    void Update()
    {
        if (serialPort.BytesToRead > 0)
        {
            string message = serialPort.ReadLine();
            // Remove start and end statements
            message = message.Substring("<START>".Length, message.Length - "<START>".Length - "<END>".Length);

                
            // Split the received string into three floats
            string[] parts = message.Split(',');
            if (parts.Length == 3)
            {
                x1 = float.Parse(parts[1]);
                y1 = float.Parse(parts[0]);
                z1 = float.Parse(parts[2]);

                newRotationData = Quaternion.Euler(0f, y1, 0f);
                Quaternion delta = newRotationData * Quaternion.Inverse(previousRotationData);

                capsulius.localRotation *= delta;
                previousRotationData = newRotationData;
            }
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    IEnumerator DelayDebugLog()
    {
        while (true)
        {
        Debug.Log("Received coordinates: (" + y1 + ", " + x1 + ", " + z1 + ")");
        yield return new WaitForSeconds(debugDelay);
        }
    }
}