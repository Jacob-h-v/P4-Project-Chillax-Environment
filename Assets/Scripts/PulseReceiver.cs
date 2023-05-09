using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Threading;
using System.IO;
using System.IO.Ports;

public class PulseReceiver : MonoBehaviour
{ 
    private Thread IOThread = new Thread(DataThread);
    private StreamWriter sw;
    private StreamWriter sw2;
    private static SerialPort pulseStream;
    DateTimeOffset localTime;

    private static string incomingPulseMsg = "";
    private static string outgoingPulseMsg = "";
    private static string pulseMessage = "";
    [SerializeField] string newFileName = "";
    static string Filename = "";
    public string portName = "COM9";
    string path;
    string path2;

    public float BPM = 80;
    [SerializeField] float rawSensorData;
    [SerializeField] AmbientToBPM ambientToBPM;
    [SerializeField] float smoothedBPM;
    
    public int baudRate = 9600;

    IEnumerator LogUpdate()
    {
        while (true)
        {
            smoothedBPM = ambientToBPM.InternalBPM;
            smoothedBPM = (int)Mathf.Round(smoothedBPM);
            int currentTime = (int)Mathf.Round(Time.time);
            // Defining what is written on each line in the log text
            string SensorLogText = "Sensor BPM = " + BPM + ", Smoothed BPM = " + smoothedBPM + ", Timestamp = " + currentTime;

            // Appending the string to the textfile which means it is written behind the current text
            sw.WriteLine(SensorLogText);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator RawLogUpdate()
    {
        while (true)
        {
            // Defining what is written on each line in the log text
            string RawLogText = "Raw Sensor Data = " + rawSensorData + ", Timestamp = " + Time.time;

            // Appending the string to the textfile which means it is written behind the current text
            sw2.WriteLine(RawLogText);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private static void DataThread()
    {

        while(true)
        {
            if (outgoingPulseMsg != "")
            {
                pulseStream.Write(outgoingPulseMsg);
                outgoingPulseMsg = "";
            }
            incomingPulseMsg = pulseStream.ReadLine();
            //Debug.Log($"incoming pulse {incomingPulseMsg}");
            Thread.Sleep(100);
        }
    }

    void OnDestroy()
    {
        IOThread.Abort();
        pulseStream.Close();
        sw.Close();
        sw2.Close();
    }

    void Awake()
    {
        // Get the current date and time in GMT+2
        localTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(2));

        if (PlayerPrefs.GetString("Filename", Filename) != "" && PlayerPrefs.GetString("Filename", Filename) != null)
        {
        Filename = PlayerPrefs.GetString("Filename", Filename);
        }
        else
        {
            Filename = "emergencyLog";
        }

        if (newFileName != "")
        {
            if (newFileName != Filename)
            {
                Filename = newFileName;
            }
        }

        CreateLogText();
    }

    // Start is called before the first frame update
    void Start()
    {
        pulseStream = new SerialPort(portName, baudRate);
        pulseStream.Open();
        pulseStream.ReadExisting();
        IOThread.Start();
        
        // Creating a new Streamwriter object with desired path
        sw = new StreamWriter(path);
        sw2 = new StreamWriter(path2);

        StartCoroutine(LogUpdate());
        StartCoroutine(RawLogUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingPulseMsg != "")
        {
            // Separate remove the <START> and <END> statements from the received string and parse the remaining BPM value to a float
            pulseMessage = incomingPulseMsg.Substring("<START>".Length, incomingPulseMsg.Length - "<START>".Length - "<END>".Length);
            string[] parts = pulseMessage.Split(',');
            BPM = float.Parse(parts[0]);
            rawSensorData = float.Parse(parts[1]);
        }


        // Controls for writing test progression to log file
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("Baseline Reading Start");
            sw.WriteLine("Baseline Reading Start");
            sw2.WriteLine("Baseline Reading Start");
        }

        if (Input.GetKeyDown("2"))
        {
            Debug.Log("Stress Test Start");
            sw.WriteLine("Stress Test Start");
            sw2.WriteLine("Stress Test Start");
        }

        if (Input.GetKeyDown("3"))
        {
            Debug.Log("Product Test Start");
            sw.WriteLine("Product Test Start");
            sw2.WriteLine("Product Test Start");
        }

        if (Input.GetKeyDown("4"))
        {
            Debug.Log("Product Test End");
            sw.WriteLine("Product Test End");
            sw2.WriteLine("Product Test End");
        }
    }

    private void CreateLogText()
    {

        // Get current time and date
        string filedate = localTime.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture) + ".txt";

        // Define the path of the log file
        path = Application.dataPath + "/TestLogs" + "/" + Filename + filedate;
        path2 = Application.dataPath + "/TestLogs" + "/" + Filename + "Raw " + filedate;

        // Write to the file
        File.WriteAllText(path, "");
        File.WriteAllText(path2, "");
        
    }
}