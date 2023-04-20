using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatEmulator : MonoBehaviour
{

    public float BPM;
    public bool heartIsBeating;


    private void Start()
    {
        heartIsBeating = true;
        StartCoroutine(RandomizationOfTheHeartbeat());

    }


    IEnumerator RandomizationOfTheHeartbeat()
    {
        while(heartIsBeating == true)
        {
            BPM = Random.Range(45, 120);
            yield return new WaitForSeconds(5);
        }
    }

    void Update()
    {
        Debug.Log("BPM is currently; " + BPM);
    }
}
