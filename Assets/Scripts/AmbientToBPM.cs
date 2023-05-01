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
<<<<<<< Updated upstream
    float SensorBPM;
    private float debugDelay = 1f;
=======
    float SensorBPM = 80;
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
        // Testing that the BPM is accurately sent
        //Debug.Log(InternalBPM);

=======
>>>>>>> Stashed changes
        // By using the lipPD library we send the BPM float to the pd patch that sends the float as a message to change the generated musics BPM
        pdPatch.SendFloat("BPM", InternalBPM);
    }

    IEnumerator MoveTowardsBPM()
    {
        while (true)
        {
            InternalBPM = Mathf.MoveTowards(InternalBPM, SensorBPM, 200 * Time.deltaTime);
<<<<<<< Updated upstream
            Debug.Log(InternalBPM);
=======

            // Testing that the BPM is accurately sent
            Debug.Log(InternalBPM);

>>>>>>> Stashed changes
            yield return new WaitForSeconds(2);
        }
    }
}
