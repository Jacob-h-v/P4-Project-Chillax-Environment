using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pidgeon : MonoBehaviour
{
    //Variable to grab reference to the audio source
    public AudioSource pidgeonTakeOff;
    public AudioSource birdsChirping;


    private float spatialBlendSetTo3D = 1;
    


    
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
        yield return new WaitForSecondsRealtime(5);

        takingOff = true;
    }

    // Update is called once per frame
    void Update()
    {


        //Check to see if you just set the pidgeon take off toggle to positive
        if (takingOff == true && flyingBirds == false)
        {
            pidgeonTakeOff.spatialBlend = spatialBlendSetTo3D;
            pidgeonTakeOff.Play();

            flyingBirds = true;
            takingOff = false;
        }

        if (flyingBirds == true && takingOff == false && transform.position.y < 10)
        {
            transform.Translate(Vector3.up * Time.deltaTime, Space.Self);
            birdsChirping.spatialBlend = spatialBlendSetTo3D;
            birdsChirping.Play();
            birdsChirping.loop = true;
        }

      
    }
}
