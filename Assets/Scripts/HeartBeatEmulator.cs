using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatEmulator : MonoBehaviour
{

    public float BPM;
    public bool heartIsBeating;
    private float BPMTarget;

    private void Start()
    {

        BPM = Random.Range(45, 120);

        heartIsBeating = true;
        StartCoroutine(RandomizationOfTheHeartbeat());

    }


    IEnumerator RandomizationOfTheHeartbeat()
    {

        

        while (heartIsBeating == true)
        {

            BPMTarget = Random.Range(45, 120);

            BPM = Mathf.MoveTowards(BPM, BPMTarget, 200 * Time.deltaTime);

            yield return new WaitForSeconds(5);
        }
    }

    void Update()
    {
        Debug.Log("BPM is currently; " + BPM);
    }
}
