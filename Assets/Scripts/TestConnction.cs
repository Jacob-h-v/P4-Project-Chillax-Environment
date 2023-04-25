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
    private string[] gyroCoordsArray;
    private float[] gyroCoordsFloats;
    private float[] gyroCoordsOld;
    private float rotationModX = 0;
    private float rotationModY = 0;
    private float rotationModZ = 0;
    private static string randomshit = "";
    [SerializeField] Transform capsulius;
    /*public string receivestring;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public string[] datas;
    */

    private static void DataThread()
    {
        //pulseStream = new SerialPort("COM4", 115200);
        gyroStream = new SerialPort("COM5", 115200);
        //pulseStream.Open();
        gyroStream.Open();

        while(true)
        {
            if (outgoingPulseMsg != "")
            {
                pulseStream.Write(outgoingPulseMsg);
                outgoingPulseMsg = "";
            }
            // incomingPulseMsg = pulseStream.ReadExisting();
            if (outgoingGyroMsg != "")
            {
                gyroStream.Write(outgoingGyroMsg);
                outgoingGyroMsg = "";
            }
            incomingGyroMsg = "";
            incomingGyroMsg = gyroStream.ReadLine();
            randomshit = gyroStream.ReadExisting();
            randomshit = "";
            Thread.Sleep(200);
        }
    }

    void OnDestroy()
    {
        IOThread.Abort();
        //pulseStream.Close();
        gyroStream.Close();
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
        }

        if (incomingGyroMsg != "")
        {
            Debug.Log($"Gyro signal: ");
            Debug.Log(incomingGyroMsg);
        }

        gyroCoordsArray = incomingGyroMsg.Split("\t");
        gyroCoordsFloats = new float[gyroCoordsArray.Length];
        for (int i = 0; i < gyroCoordsArray.Length; i++) {
            if (!float.TryParse(gyroCoordsArray[i], out gyroCoordsFloats[i])) {
                Debug.Log($"Failed to convert {gyroCoordsArray[i]} to float.");
            }
        }
        Debug.Log(string.Join(", ", gyroCoordsFloats));

        if (gyroCoordsArray.Length == gyroCoordsFloats.Length) 
        {
        capsulius.localRotation = Quaternion.Euler(gyroCoordsFloats[0],gyroCoordsFloats[1],gyroCoordsFloats[2]);
        }
        /*if (gyroCoordsFloats != gyroCoordsOld)
        {
        float rotationModX = Mathf.Abs(gyroCoordsFloats[1] - gyroCoordsOld[1]);
        float rotationModY = Mathf.Abs(gyroCoordsFloats[2] - gyroCoordsOld[2]);
        float rotationModZ = Mathf.Abs(gyroCoordsFloats[3] - gyroCoordsOld[3]);

        capsulius.Rotate(new Vector3(rotationModX, rotationModY, rotationModZ));

        gyroCoordsOld = (float[])gyroCoordsFloats.Clone();
        }*/
        /*if (gyroCoordsFloats != gyroCoordsOld)
        {
            rotationModX = Mathf.Abs(gyroCoordsFloats[0] - gyroCoordsOld[0]);
            rotationModY = Mathf.Abs(gyroCoordsFloats[1] - gyroCoordsOld[1]);
            rotationModZ = Mathf.Abs(gyroCoordsFloats[2] - gyroCoordsOld[2]);
            
            capsulius.Rotate(new Vector3(rotationModX, rotationModY, rotationModZ));
            
            gyroCoordsOld = gyroCoordsFloats;
        }*/

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
