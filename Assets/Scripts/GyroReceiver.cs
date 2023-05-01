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

    private SerialPort serialPort;
    [SerializeField] private Transform capsulius;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();

        StartCoroutine(DelayDebugLog());
    }

    void Update()
    {
        if (serialPort.BytesToRead > 0)
        {
            string message = serialPort.ReadLine();
            //if (message.Length < 2);
            //{
             //   return;
           // }
            //Debug.Log($"Recieved full message: {message}");
            //if (message.StartsWith("<START>") && message.EndsWith("<END>"))
            //{
                // Remove start and end statements
                 message = message.Substring("<START>".Length, message.Length - "<START>".Length - "<END>".Length);

                
                // Split message into three sets of coordinates
                string[] parts = message.Split(',');
                if (parts.Length == 3)
                {
                    x1 = float.Parse(parts[0]);
                    y1 = float.Parse(parts[1]);
                    z1 = float.Parse(parts[2]);
                    
                    // Do something with the coordinates
                    //Debug.Log("Received coordinates: (" + x1 + ", " + y1 + ", " + z1 + ")");
                    capsulius.localRotation = Quaternion.Euler(y1,x1,z1);
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

    IEnumerator DelayDebugLog()
    {
        while (true)
        {
        Debug.Log("Received coordinates: (" + y1 + ", " + x1 + ", " + z1 + ")");
        yield return new WaitForSeconds(debugDelay);
        }
    }
}