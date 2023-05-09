using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientToBPM : MonoBehaviour
{
    [SerializeField] bool useSensor;

    // Creating an instance of the pd patch we will communicate with
    public LibPdInstance pdPatch;

    // Gameobject that has the BPM script on it to access the float
    public GameObject playerBPM;
    public float InternalBPM = 80;
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

    // Audio Sources in the Unity Space and volume values used to turn off and on based on BPM

    // Rain
    public AudioSource rainAbove;
    public AudioSource rainNear;
    bool isRaining = false;
    float rainVolume = 0f;

    // Wind
    public AudioSource strongWind;
    bool isWindy = false;
    float windVolume = 0f;

    // The seconds between each fade update
    float UpdateFrequency = 0.1f;

    // Controls the maximum change per second in the fade
    float FadeDelay = 0.1f;

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
        if (useSensor)
        {
        SensorBPM = playerBPM.GetComponent<PulseReceiver>().BPM;
        }
        else
        {
            SensorBPM = 80;
        }

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

            yield return new WaitForSeconds(0.15f);
        }
    }

    // Function that fades between the different songs between 0 and 1
    private void Fade(float song1, float song2, float song3, float song4, float rain, float wind)
    {
        songVolume1 = Mathf.MoveTowards(songVolume1, song1, FadeDelay * Time.deltaTime);
        songVolume2 = Mathf.MoveTowards(songVolume2, song2, FadeDelay * Time.deltaTime);
        songVolume3 = Mathf.MoveTowards(songVolume3, song3, FadeDelay * Time.deltaTime);
        songVolume4 = Mathf.MoveTowards(songVolume4, song4, FadeDelay * Time.deltaTime);
        rainVolume = Mathf.MoveTowards(rainVolume, rain, FadeDelay * Time.deltaTime);
        windVolume = Mathf.MoveTowards(windVolume, wind, FadeDelay * Time.deltaTime);
    }

    // ThresholdFading() is for checking the current BPM and changing the song in pd based on current threshold value
    IEnumerator ThresholdFading()
    {
        while (true)
        {
            if (InternalBPM <= Threshold1)
            {
                Fade(1f, 0f, 0f, 0f, 1f, 0f);

                if (isRaining == false)
                {
                    rainAbove.Play();
                    rainNear.Play();
                    isRaining = true;
                }
            }

            if (InternalBPM > Threshold1 && InternalBPM <= Threshold2)
            {
                Fade(0f, 1f, 0f, 0f, 0f, 0f);

                if (isRaining == true)
                {
                    rainAbove.Pause();
                    rainNear.Pause();
                    isRaining = false;
                }

                if (isWindy == true)
                {
                    strongWind.Pause();
                    isWindy = false;
                }
            }

            if (InternalBPM > Threshold2 && InternalBPM <= Threshold3)
            {
                Fade(0f, 0f, 1f, 0f, 0f, 0.5f);

                if (isWindy == false)
                {
                    strongWind.Play();
                    isWindy = true;
                }
            }

            if (InternalBPM > Threshold3)
            {
                Fade(0f, 0f, 0f, 1f, 0f, 1f);
            }

            // Sends all volume floats for each song to pd, which either fades songs in or out
            pdPatch.SendFloat("SongVolume1", songVolume1);
            pdPatch.SendFloat("SongVolume2", songVolume2);
            pdPatch.SendFloat("SongVolume3", songVolume3);
            pdPatch.SendFloat("SongVolume4", songVolume4);
            rainAbove.volume = rainVolume;
            rainNear.volume = rainVolume;
            strongWind.volume = windVolume;

            yield return new WaitForSeconds(UpdateFrequency);
        }
    }
}
