using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : View
{
    // click New Game button
   public void OnNewGameButtonClick() {
       GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
       this.Hide();

   }

   // click Back to Menu Button
   public void onBackButtonClick() {
       SceneManager.LoadSceneAsync(0);
   }
}
