using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using BayatGames.SaveGameFree;
using UnityEngine.UI;
public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    
    public void Start()
    {        
        float sliderValue = SaveGame.Load<float>("sliderVol", 0.5f);
        GetComponent<Slider>().value = sliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevel(float sliderValue)
    {
        if(sliderValue != 0)
            //Mixer volume is on a log scale
            mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        else
            //Mute the audio
            mixer.SetFloat("MusicVolume", -80);

        SaveGame.Save<float>("sliderVol", sliderValue);
    }
}
