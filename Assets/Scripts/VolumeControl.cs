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
        // Listen for changes on the UI elements
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    void Start()
    {
        // Load previously chosen volume values
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
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
            // if Master volume was disabled and then re-enabled, set to 80% (to avoid weird bug where it remains at 0)
            slider.value = 0.8f;
        }
        else
        {
            slider.value = slider.minValue;
        }
    }

    private void OnDisable()
    {
        // Saves  user's volume preferences
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
}
