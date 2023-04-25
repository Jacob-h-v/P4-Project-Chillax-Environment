using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;

public class TestConnction : MonoBehaviour
{ 
    Thread IOThread = new Thread(DataThread);
    private static SerialPort pulseStream;
    private static SerialPort gyroStream;
    private static string incomingPulseMsg = "";
    private static string outgoingPulseMsg = "";
    private static string incomingGyroMsg = "";
    private static string outgoingGyroMsg = "";

    public float BPM;
    /*public string receivestring;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public string[] datas;
    */

    private static void DataThread()
    {
        pulseStream = new SerialPort("COM7", 9600);
        // gyroStream = new SerialPort("COM5", 115200);
        pulseStream.Open();
        // gyroStream.Open();

        while(true)
        {
            if (outgoingPulseMsg != "")
            {
                pulseStream.Write(outgoingPulseMsg);
                outgoingPulseMsg = "";
            }
            incomingPulseMsg = pulseStream.ReadExisting();
            if (outgoingGyroMsg != "")
            {
                gyroStream.Write(outgoingGyroMsg);
                outgoingGyroMsg = "";
            }
            // incomingGyroMsg = gyroStream.ReadExisting();
            Thread.Sleep(200);
        }
    }

    void OnDestroy()
    {
        IOThread.Abort();
        pulseStream.Close();
        // gyroStream.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        //data_stream.Open(); //Initiate the Serial stream
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingPulseMsg != "")
        {
            Debug.Log($"Pulse signal: {incomingPulseMsg}");
            BPM = float.Parse(incomingPulseMsg);
        }

        if (incomingGyroMsg != "")
        {
            Debug.Log($"Gyro signal: {incomingGyroMsg}");
        }
        /*
        if (!data_stream.IsOpen){
        data_stream = new SerialPort("COM3", 19200);
        data_stream.ReadTimeout = 1000;
        data_stream.WriteTimeout = 1000;
        data_stream.Open();
        }
        receivestring = data_stream.ReadLine();

        string[] datas = receivestring.Split(','); // split the data between ','
        rb.AddForce(0,0,float.Parse(datas[0]) * sensitivity * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(float.Parse(datas[1]) * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        transform.Rotate(0, float.Parse(datas[2]), 0);

        Debug.Log(receivestring);
        */
    }
}
