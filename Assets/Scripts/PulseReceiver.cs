using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
using System.IO.Ports;

public class PulseReceiver : MonoBehaviour
{ 
    private Thread IOThread = new Thread(DataThread);
    private StreamWriter sw;
    private static SerialPort pulseStream;

    private static string incomingPulseMsg = "";
    private static string outgoingPulseMsg = "";
    private static string pulseMessage = "";
    [SerializeField] string Filename = "log4";
    string path;

    public float BPM = 80;

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
        pulseStream = new SerialPort("COM9", 9600);
        pulseStream.Open();

        while(true)
        {
            if (outgoingPulseMsg != "")
            {
                pulseStream.Write(outgoingPulseMsg);
                outgoingPulseMsg = "";
            }
            incomingPulseMsg = pulseStream.ReadLine();
            //Debug.Log($"incoming pulse {incomingPulseMsg}");
            Thread.Sleep(200);
        }
    }

    void OnDestroy()
    {
        IOThread.Abort();
        pulseStream.Close();
        sw.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        CreateLogText();
        
        // Creating a new Streamwriter object with desired path
        sw = new StreamWriter(path);

        StartCoroutine(LogUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingPulseMsg != "")
        {
            // Separate remove the <START> and <END> statements from the received string and parse the remaining BPM value to a float
            pulseMessage = incomingPulseMsg.Substring("<START>".Length, incomingPulseMsg.Length - "<START>".Length - "<END>".Length);
            BPM = float.Parse(pulseMessage);
        }


        // Controls for writing test progression to log file
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
    }
}
