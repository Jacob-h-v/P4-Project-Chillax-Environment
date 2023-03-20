using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;

public class TestConnction : MonoBehaviour
{ 
    Thread IOThread = new Thread(DataThread);
    private static SerialPort data_stream;
    private static string incomingMsg = "";
    private static string outgoingMsg = "";
    /*public string receivestring;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public string[] datas;
    */

    private static void DataThread()
    {
        data_stream = new SerialPort("COM3", 19200);
        data_stream.Open();

        while(true)
        {
            if (outgoingMsg != "")
            {
                data_stream.Write(outgoingMsg);
                outgoingMsg = "";
            }
            incomingMsg = data_stream.ReadExisting();
            Thread.Sleep(200);
        }
    }

    private void OnDestroy()
    {
        IOThread.Abort();
        data_stream.Close();
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
        if (incomingMsg != "")
        {
            Debug.Log(incomingMsg);
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
