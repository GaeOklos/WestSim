using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SC_SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider MainSlider;
    public Slider musicSlider;
    public Slider soundSlider;
	public Animator anim;


    private void Start()
    {
        audioMixer.GetFloat("MainVolume", out float MainValueSlider);
        MainSlider.value = MainValueSlider;

        audioMixer.GetFloat("SoundMixer", out float soundValueSlider);
        soundSlider.value = soundValueSlider;

        audioMixer.GetFloat("MusicMixer", out float musicValueSlider);
        musicSlider.value = musicValueSlider;
    }

    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat("MainVolume", Volume);
    }
    public void SetVolumeMusic(float Volume)
    {
        audioMixer.SetFloat("MusicMixer", Volume);
    }
    public void SetVolumeSound(float Volume)
    {
        audioMixer.SetFloat("SoundMixer", Volume);
    }

    public void LauncherAnim()
    {
        print("do");
		anim.SetTrigger("Flash");
        print("did");
    }
}
