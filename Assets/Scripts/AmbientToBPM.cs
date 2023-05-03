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
    float SensorBPM = 80;

    // Threshold values for BPM changes to music melody
    [SerializeField] float Threshold1 = 60;
    [SerializeField] float Threshold2 = 75;
    [SerializeField] float Threshold3 = 90;

    // Volume values for turning on or off the different melodies in pd
    float songVolume1 = 0;
    float songVolume2 = 0;
    float songVolume3 = 0;
    float songVolume4 = 0;

    // The seconds between each fade update
    float UpdateFrequency = 0.1f;

    // Controls the maximum change per second in the fade
    [SerializeField] float FadeDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTowardsBPM());
        StartCoroutine(ThresholdFading());
    }

    // Update is called once per frame
    void Update()
    {
        // The BPM float from the user object is acessed each update cycle and syncronized with the internal BPM float
        SensorBPM = playerBPM.GetComponent<PulseReceiver>().BPM;

        // By using the lipPD library we send the BPM float to the pd patch that sends the float as a message to change the generated musics BPM
        pdPatch.SendFloat("BPM", InternalBPM);
    }

    // MoveTowardsBPM() is there to smooth the sensor BPM to an internal BPM used in other function
    // This is done to avoid sudden changes in the BPM that would be unpleasant for the user
    IEnumerator MoveTowardsBPM()
    {
        while (true)
        {
            InternalBPM = Mathf.MoveTowards(InternalBPM, SensorBPM, 200 * Time.deltaTime);

            // Testing that the BPM is accurately sent
            Debug.Log(InternalBPM);

            yield return new WaitForSeconds(2);
        }
    }

    // Function that fades between the different songs between 0 and 1
    private void Fade(float song1, float song2, float song3, float song4)
    {
        songVolume1 = Mathf.MoveTowards(songVolume1, song1, FadeDelay * Time.deltaTime);
        songVolume2 = Mathf.MoveTowards(songVolume2, song2, FadeDelay * Time.deltaTime);
        songVolume3 = Mathf.MoveTowards(songVolume3, song3, FadeDelay * Time.deltaTime);
        songVolume4 = Mathf.MoveTowards(songVolume4, song4, FadeDelay * Time.deltaTime);
    }

    // ThresholdFading() is for checking the current BPM and changing the song in pd based on current threshold value
    IEnumerator ThresholdFading()
    {
        while (true)
        {
            if (InternalBPM <= Threshold1)
            {
                Fade(1f, 0f, 0f, 0f);
            }

            if (InternalBPM > Threshold1 && InternalBPM <= Threshold2)
            {
                Fade(0f, 1f, 0f, 0f);
            }

            if (InternalBPM > Threshold2 && InternalBPM <= Threshold3)
            {
                Fade(0f, 0f, 1f, 0f);
            }

            if (InternalBPM > Threshold3)
            {
                Fade(0f, 0f, 0f, 1f);
            }

            // Sends all volume floats for each song to pd, which either fades songs in or out
            pdPatch.SendFloat("SongVolume1", songVolume1);
            pdPatch.SendFloat("SongVolume2", songVolume2);
            pdPatch.SendFloat("SongVolume3", songVolume3);
            pdPatch.SendFloat("SongVolume4", songVolume4);

            yield return new WaitForSeconds(UpdateFrequency);
        }
    }
}
