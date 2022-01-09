using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    private AudioSource bg_music;
    private AudioSource merge_effect;
    private void Awake()
    {
        _instance = this;


        bg_music = transform.Find("bg_music").GetComponent<AudioSource>();
        merge_effect = transform.Find("merge_effect").GetComponent<AudioSource>();

        // get saved music and effect volumd
        bg_music.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        merge_effect.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
    }

    public void PlayMusic(AudioClip audioClip){
        if (bg_music == null){
            Debug.Log("hello");
            return;
        }
        bg_music.clip = audioClip;
        bg_music.loop = true;
        bg_music.Play();
    }

    public void PlayMergeEffect(AudioClip audioClip){
        merge_effect.PlayOneShot(audioClip);
    }

    public void OnMusicVolumneChange(float value){
        bg_music.volume = value;
    }

    public void OnEffectVolumeChange(float value){
        merge_effect.volume = value;
    }
}
