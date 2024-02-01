using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SC_AudioManager : MonoBehaviour
{
    // Singleton Awake
    public static SC_AudioManager instance = null;
    public AudioClip[] playlist;
    public AudioSource audioSource;
    public AudioMixer audioMixer;
    public AudioMixerGroup soundEffectMixer;
    private GameObject[] _sameObject;
    private GameObject[] _soundAlreadyExist;

    private void Awake() {
        if (SC_AudioManager.instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
        audioMixer.SetFloat("MusicMixer", -30);
        audioMixer.SetFloat("MainVolume", -80);
        audioMixer.SetFloat("SoundMixer", 0);
    }



    private void Update()
    {
        if (audioSource.isPlaying == false) {
            audioSource.clip = playlist[0];
            audioSource.Play();
        }
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        /*To play the sound :
        public AudioClip sound;
        
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SC_AudioManager>().PlayClipAt(sound, this.transform.position);
        */

        // Pour régler le soucis d'avoir des sons qui ne se destroy jamais à cause du time 0
        _soundAlreadyExist = GameObject.FindGameObjectsWithTag("TempAudioTag");
        for (int i = 0; i < _soundAlreadyExist.Length; i++)
            Destroy(_soundAlreadyExist[i]);

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        tempGO.gameObject.tag="TempAudioTag";
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play();
        Destroy(tempGO, clip.length);
        return audioSource;
    }
}
