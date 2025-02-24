using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.5f);
        AdjustVolume(volumeSlider.value);

        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
    }
}
