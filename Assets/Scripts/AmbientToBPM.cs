using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientToBPM : MonoBehaviour
{

    // Creating an instance of the pd patch we will communicate with
    public LibPdInstance pdPatch;

    // Gameobject that has the BPM script on it to access the float
    public GameObject playerBPM;
    float InternalBPM = 80;
    float SensorBPM;
    private float debugDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTowardsBPM());
    }

    // Update is called once per frame
    void Update()
    {
        // The BPM float from the user object is acessed each update cycle and syncronized with the internal BPM float
        SensorBPM = playerBPM.GetComponent<TestConnction>().BPM;

        // Testing that the BPM is accurately sent
        //Debug.Log(InternalBPM);

        // By using the lipPD library we send the BPM float to the pd patch that sends the float as a message to change the generated musics BPM
        pdPatch.SendFloat("BPM", InternalBPM);
    }

    IEnumerator MoveTowardsBPM()
    {
        while (true)
        {
            InternalBPM = Mathf.MoveTowards(InternalBPM, SensorBPM, 200 * Time.deltaTime);
            Debug.Log(InternalBPM);
            yield return new WaitForSeconds(2);
        }
    }
}
