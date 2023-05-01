using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
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
    private static string pulseMessage = "";

    public float BPM = 80;
    /*public string receivestring;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public string[] datas;
    */

    private StreamWriter sw;

    [SerializeField] string Filename = "log4";

    string path;

    private void CreateLogText()
    {
        // Defining the path of the log file
        path = Application.dataPath + "/TestLogs" + "/" + Filename + ".txt";

        // Checks if file already exists
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
    }

    IEnumerator LogUpdate()
    {
        while (true)
        {
            int currentTime = (int)Mathf.Round(Time.time);
            // Defining what is written on each line in the log text
            string SensorLogText = "BPM = " + BPM + ", Timestamp = " + currentTime;

            // Appending the string to the textfile which means it is written behind the current text
            sw.WriteLine(SensorLogText);

            yield return new WaitForSeconds(1);
        }
    }

    private static void DataThread()
    {
        pulseStream = new SerialPort("COM8", 9600);
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
            incomingPulseMsg = pulseStream.ReadLine();
            //Debug.Log($"incoming pulse {incomingPulseMsg}");
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
        sw.Close();
        // gyroStream.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        CreateLogText();
        
        // Creating a new Streamwriter object with desired path
        sw = new StreamWriter(path);

        StartCoroutine(LogUpdate());

        //data_stream.Open(); //Initiate the Serial stream
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingPulseMsg != "")
        {
            //Debug.Log($"Pulse signal: {incomingPulseMsg}");
            pulseMessage = incomingPulseMsg.Substring("<START>".Length, incomingPulseMsg.Length - "<START>".Length - "<END>".Length);
            BPM = float.Parse(pulseMessage);
        }

        if (incomingGyroMsg != "")
        {
            Debug.Log($"Gyro signal: {incomingGyroMsg}");
        }

        if (Input.GetKeyDown("1"))
        {
            Debug.Log("Baseline Reading Start");
            sw.WriteLine("Baseline Reading Start");
        }

        if (Input.GetKeyDown("2"))
        {
            Debug.Log("Stress Test Start");
            sw.WriteLine("Stress Test Start");
        }

        if (Input.GetKeyDown("3"))
        {
            Debug.Log("Product Test Start");
            sw.WriteLine("Product Test Start");
        }

        if (Input.GetKeyDown("4"))
        {
            Debug.Log("Product Test End");
            sw.WriteLine("Product Test End");
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
