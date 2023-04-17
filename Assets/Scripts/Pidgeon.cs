using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pidgeon : MonoBehaviour
{
    //Variable to grab reference to the audio source
    public AudioSource pidgeonTakeOff;
    public AudioSource birdsChirping;


    private float spatialBlendSetTo3D = 1;
    
    public AudioClip takeOffSound;


    //Detect when you use the toggle, ensures music isn’t played multiple times
    private bool takingOff;
    
    private bool flyingBirds;

    void Start()
    {
        takingOff = false;
        flyingBirds = false;

        StartCoroutine(PidgeonsTakingOff());


    }


    IEnumerator PidgeonsTakingOff()
    {
        yield return new WaitForSeconds(10);

        takingOff = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        

        //Check to see if you just set the pidgeon take off toggle to positive
        if (takingOff == true)
        {
            pidgeonTakeOff.spatialBlend = spatialBlendSetTo3D;
            pidgeonTakeOff.PlayOneShot(takeOffSound);
            takingOff = false;
            flyingBirds = true;
        }

        if (flyingBirds == true && takingOff == false)
        {
            birdsChirping.Play();
        }

        while (takingOff == true)
        {

        }
    }
}
