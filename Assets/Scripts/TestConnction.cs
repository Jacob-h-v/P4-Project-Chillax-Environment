using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class TestConnction : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("\\\\.\\COM3",19200); // Arduino is connected to COM3, with 19200 baud rate
    public string receivestring;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;

    public string[] datas;


    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open(); //Initiate the Serial stream
    }

    // Update is called once per frame
    void Update()
    {
        receivestring = data_stream.ReadLine();

        string[] datas = receivestring.Split(','); // split the data between ','
        rb.AddForce(0,0,float.Parse(datas[0]) * sensitivity * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(float.Parse(datas[1]) * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        transform.Rotate(0, float.Parse(datas[2]), 0);

        Debug.Log(receivestring);
    }
}
