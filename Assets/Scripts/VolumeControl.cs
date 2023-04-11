// Original script by Jason Weimann https://www.youtube.com/watch?v=MmWLK9sN3s8&t=1s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolParam";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    [SerializeField] private Toggle toggle;
    private bool disableMasterToggleEvent;

    void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        disableMasterToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableMasterToggleEvent = false;
    }

    private void HandleToggleValueChanged(bool enableMaster)
    {
        if (disableMasterToggleEvent)
        {
            return;
        }
        if (enableMaster)
        {
            slider.value = 0.8f;
        }
        else
        {
            slider.value = slider.minValue;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
}
