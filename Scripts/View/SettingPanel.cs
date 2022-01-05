using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : View
{
    // close the setting panel
    public Slider slider_sound;
    public Slider slider_music;
    public void OnBtnCloseClick(){
        // hide the current panel
        Hide();
    } 

    // move music scroll bar
    public void OnMusicVolumeChange(float volume){
        PlayerPrefs.SetFloat(Const.Music, volume);
    }

    // move sound scroll bar
    public void OnSoundVolumeChange(float volume){
        PlayerPrefs.SetFloat(Const.Sound, volume);
    }

    // initialize music and sound slider when restarting the game

    public override void Show()
    {
        base.Show();
        slider_music.value = PlayerPrefs.GetFloat(Const.Music,0);
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound,0);

    }
    
}
