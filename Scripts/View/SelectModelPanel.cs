using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModelPanel : View
{
    

    // enter the grid size
    public void OnSelectModelClick(int count){
        // select a move
        PlayerPrefs.SetInt(Const.GameModel, count);
        // go to the game scence
        SceneManager.LoadSceneAsync(1);

    }

}
