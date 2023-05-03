using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedPanning : MonoBehaviour
{
    [SerializeField] private float currentPan = 0.0f;
    [SerializeField] private bool enableAutoPan = true;
    [SerializeField] private float autoPanSpeed = 0.8f;
    [SerializeField] private AudioSource aSource;
    private bool panPlus;
    private bool panMinus;


    // Start is called before the first frame update
    void Start()
    {
        panPlus = true;
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Keeps panning left -> right, right -> left if enabled
        CheckCurrentPan();
        if (enableAutoPan)
        {
            if (panPlus)
            {
                currentPan = currentPan + autoPanSpeed * Time.deltaTime;
                aSource.panStereo = currentPan;
            }
            if (panMinus)
            {
                currentPan = currentPan - autoPanSpeed * Time.deltaTime;
                aSource.panStereo = currentPan;
            }
        }
        
    }

    // reverts panning direction when one extreme is reached
    private void CheckCurrentPan()
    {
        if (currentPan >= 1.0f)
        {
            panPlus = false;
            panMinus = true;
        }
        if (currentPan <= -1.0f)
        {
            panMinus = false;
            panPlus = true;
        }
    }
}
