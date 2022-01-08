using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Timers;

public class Number : MonoBehaviour
{
    private Image bg;
    private Text number_text;

    private MyGrid parentGrid;

    public NumberStatus status;

    private float spawnScaleTime = 1;
    private bool isPlayingSpawnAnim = false;
    private float mergeScaleTime = 1;
    private float mergeScaleTimeBack = 1;
    
    private bool isPlayingMergeAnim = false;

    private float movePosTime = 1;
    private bool isMoving = false;
    private Vector3 startMovePos, endMovePos;

    public Color[] bg_colors; 
    public List<int> number_index;
    private void Awake() {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("value").GetComponent<Text>();
    }

    // initialize
    public void Init(MyGrid myGrid){
        myGrid.SetNumber(this);
        this.SetGrid(myGrid);
        this.SetNumber(2);
        this.status = NumberStatus.Normal;
        // play spawn anim
        PlaySpwanAnim();
    }

    public void SetGrid(MyGrid myGrid){
        parentGrid = myGrid;
    }

    public MyGrid GetGrid(){
        return parentGrid;
    }
    public void SetNumber(int number){
        number_text.text = number.ToString();
        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
    }

    public int GetNumber(){
        return int.Parse(number_text.text);
    }

    public void MoveToGrid(MyGrid myGrid){
        transform.SetParent(myGrid.transform);
        // transform.localPosition = Vector3.zero;
        startMovePos = transform.position;
        endMovePos = myGrid.transform.position;
        isMoving = true;
        movePosTime = 0;

        GetGrid().SetNumber(null);
        myGrid.SetNumber(this);
        SetGrid(myGrid);
    }

    public void Merge(){
        int number = this.GetNumber() * 2;
        SetNumber(number);
        if (number == 2048){
            // game is won
            GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().GameWin();
        }
        PlayMergeAnim();
        this.status = NumberStatus.NotMerge;
    }
    
    // check if can merge
    public bool IsMerge(Number number){
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal){
            return true;
        }
        return false;
    }
    // play spawn 
    public void PlaySpwanAnim(){
        // triggers Update
        spawnScaleTime = 0;
        isPlayingSpawnAnim = true;

    }

    public void PlayMergeAnim(){
        mergeScaleTime = 0;
        mergeScaleTimeBack = 0;
        isPlayingMergeAnim = true;
    }
    
    private void Update() {
        if (isPlayingSpawnAnim){
            // spawn animation
            if (spawnScaleTime <= 1){
                spawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one, spawnScaleTime);
            }
            else{
                isPlayingSpawnAnim = false;
            }

        }

        if (isPlayingMergeAnim){
            // merge animation, go big
            if (mergeScaleTime <= 1 && mergeScaleTimeBack == 0){
                mergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one,Vector3.one*1.2f, mergeScaleTime);
            }

            // merge animation, go back to normal
            if (mergeScaleTime >= 1 && mergeScaleTimeBack <= 1){
                mergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one*1.2f,Vector3.one, mergeScaleTimeBack);
            }
            
            if(mergeScaleTime >= 1 && mergeScaleTimeBack >= 1){
                isPlayingMergeAnim = false;
            }

        }

        if (isMoving){
            if (movePosTime <= 1){
                movePosTime += Time.deltaTime * 4;
                transform.position = Vector3.Lerp(startMovePos,endMovePos, movePosTime);
            }
            else{
                isMoving = false;
            }
        }
        
    }

}
