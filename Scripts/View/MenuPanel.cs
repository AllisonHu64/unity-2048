using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectModelPanel selectModelPanel;
    public SettingPanel settingPanel;

    public AudioClip bg_clip;
    void Start(){
        AudioManager._instance.PlayMusic(bg_clip);
    }
    // click on start game
    public void OnStartGameClick(){
        // change scene
        selectModelPanel.Show();
    }

    // click on settings
    public void OnSettingsClick(){
        // show SelectModelPanel
        settingPanel.Show();

    }

    // click on exit game

    public void OnExitGameClick(){
        // exit game
        Application.Quit();
    }
}
