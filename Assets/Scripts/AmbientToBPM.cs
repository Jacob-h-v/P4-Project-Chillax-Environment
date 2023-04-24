using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientToBPM : MonoBehaviour
{

    // Creating an instance of the pd patch we will communicate with
    public LibPdInstance pdPatch;

    // Gameobject that has the BPM script on it to access the float
    public GameObject playerBPM;
    float InternalBPM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InternalBPM = playerBPM.GetComponent<HeartBeatEmulator>().BPM;

        Debug.Log(InternalBPM);

        pdPatch.SendFloat("BPM", InternalBPM);
    }
}
