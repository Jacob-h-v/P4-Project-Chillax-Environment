using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class InterpreterTest : MonoBehaviour
{
    private SerialPort serialPort;
    private float[] yprArray = new float[3];
    [SerializeField] Transform capsulius;

    void Start()
    {
        serialPort = new SerialPort("COM5", 115200);
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort == null || !serialPort.IsOpen)
        {
            return;
        }

        if (serialPort.BytesToRead == 0)
        {
            return;
        }

        string line = serialPort.ReadLine();
        /*if (!line.StartsWith("ypr"))
        {
            return;
        }*/

        string[] values = line.Substring(4).Split('\t');
        if (values.Length != 3)
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            if (!float.TryParse(values[i], out yprArray[i]))
            {
                Debug.LogWarning($"Failed to convert {values[i]} to float.");
                return;
            }
        }

        capsulius.localRotation = Quaternion.Euler(yprArray[0],yprArray[1],yprArray[2]);
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

    }
}