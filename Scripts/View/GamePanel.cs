using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Text text_score; // current score
    public Text text_best_score; // best score

    public Button btn_restart;

    public Button btn_last;

    public Button btn_exit;

    public Transform girdParent; // parent of grid

    private int row; // number of rows

    private int col; // number of columns

    public GameObject gridPrefab;
    public GameObject numberPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();
    public Dictionary<int,int> grid_config = new Dictionary<int, int>() {
        {4, 150}, {5, 115}, {6, 90}
    };


    public MyGrid[][] grids = null; // save all the generated grids
    // last move

    public void OnLastMoveClick(){
    }

    // restart
    public void OnRestartClick(){

    }

    // exit
    public void OnExitClick(){

    }

    // initialize number of grids
    public void InitGrid(){
        // get the value
        int gridNum = PlayerPrefs.GetInt(Const.GameModel,4);

        this.grids = new MyGrid[gridNum][];
        // create number of grids
        // when 4 150 * 150
        // when 5 115 * 115
        // when 6 90 * 90
        GridLayoutGroup gridLayoutGroup = girdParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        gridLayoutGroup.cellSize = new Vector2(grid_config[gridNum],grid_config[gridNum]);

        // create grids
        row = gridNum;
        col = gridNum;

        for (int i = 0; i < row; i++){
            for(int j = 0; j < col; j++){
                if(this.grids[i] == null){
                    this.grids[i] = new MyGrid[col];
                }
                // create i, j cell
                this.grids[i][j] = CreateGrid();
            }
        }
    }
    public MyGrid CreateGrid(){
        //create instances from profabs
        GameObject gameObject = GameObject.Instantiate(gridPrefab,girdParent);
        return gameObject.GetComponent<MyGrid>();
    }

    public void CreateNumber(){
        canCreateNumberGrid.Clear();

        for (int i = 0; i < row; i++){
            for(int j = 0; j < col; j++){
                // check if the grid is empty or not
                if(!grids[i][j].IsHaveNumber()){
                    canCreateNumberGrid.Add(grids[i][j]);
                }
            }
        }

        int index = Random.Range(0,canCreateNumberGrid.Count);
        if(canCreateNumberGrid.Count == 0){
            return;
        }
        GameObject gameObject = GameObject.Instantiate(numberPrefab,canCreateNumberGrid[index].transform);
        gameObject.GetComponent<Number>().Init(canCreateNumberGrid[index]);
    }

    private void Awake() {
        // initate grid
        InitGrid();
        // create number
        CreateNumber();

    }

    public void OnPointerDown(){
        // Debug.Log("pointer down get mouse pos" + Input.mousePosition);
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointerUp(){
        // Debug.Log("pointer up get mouse pos" + Input.mousePosition);
        pointerUpPos = Input.mousePosition;
        
        if (Vector3.Distance(pointerDownPos,pointerUpPos) < 100){
            Debug.Log("Don't recognize as move.");
            return;
        }
        MoveType moveType = calculateMoveTpye();
        Debug.Log("movetype: " + moveType);
        MoveNumber(moveType);

        CreateNumber();
    }


    public MoveType calculateMoveTpye(){
        if(Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerUpPos.y - pointerDownPos.y))
        {
            // left and right move
            if(pointerUpPos.x - pointerDownPos.x > 0){
                // right
                // Debug.Log("to right");
                return MoveType.RIGHT;
            }
            else{
                // left
                // Debug.Log("to left");
                return MoveType.LEFT;
            }
        }
        else{
            // up and down move
            if(pointerUpPos.y - pointerDownPos.y > 0){
                // up
                // Debug.Log("to up");
                return MoveType.UP;
            }
            else{
                // down
                // Debug.Log("to down");
                return MoveType.DOWN;
            }

        }
    }

    public void MoveNumber(MoveType moveType){
        switch (moveType){
            case MoveType.UP:
                for(int j = 0; j < col; j ++){
                    for(int i = 1; i < row; i ++){
                        if (grids[i][j].IsHaveNumber()){
                            Number currentNumber = grids[i][j].GetNumber();
                            for(int m = i-1; m >= 0; m--){
                                
                                Number targetNumber  = null;
                                if (grids[m][j].IsHaveNumber()){
                                        targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(currentNumber,targetNumber,grids[m][j]);

                            }
                        }
                    }
                }
                break;
            case MoveType.DOWN:
                for(int j = 0; j < col; j ++){
                    for(int i = row-1; i >= 0; i --){
                        if (grids[i][j].IsHaveNumber()){
                            Number currentNumber = grids[i][j].GetNumber();
                            for(int m = i+1; m < row; m++){
                                Number targetNumber  = null;
                                if (grids[m][j].IsHaveNumber()){
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(currentNumber,targetNumber,grids[m][j]);

                            }
                        }
                    }
                }
                break;
            case MoveType.RIGHT:
                for(int j = col-2; j >= 0; j --){
                    for(int i = 0; i < row; i ++){
                        if (grids[i][j].IsHaveNumber()){
                            Number currentNumber = grids[i][j].GetNumber();
                            for(int m = j+1; m < col; m++){
                                
                                Number targetNumber  = null;
                                if (grids[i][m].IsHaveNumber()){
                                        targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(currentNumber,targetNumber,grids[i][m]);

                            }
                        }
                    }
                }
                break;
            case MoveType.LEFT:
                for(int j = 1; j < col; j ++){
                    for(int i = 0; i < row; i ++){
                        if (grids[i][j].IsHaveNumber()){
                            Number currentNumber = grids[i][j].GetNumber();
                            for(int m = j-1; m >= 0; m--){
                                
                                Number targetNumber  = null;
                                if (grids[i][m].IsHaveNumber()){
                                        targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(currentNumber,targetNumber,grids[i][m]);

                            }
                        }
                    }
                }
                break;
            default:
                break;

        }
    }

    public void HandleNumber(Number current, Number target, MyGrid targetGrid){
        if (target != null){
            // check if can merge
            if (current.GetNumber() == target.GetNumber()){
                target.Merge();
                // clear current number;
                current.GetGrid().SetNumber(null);
                GameObject.Destroy(current.gameObject);
            }
        }else{
            // move to grid
            current.MoveToGrid(targetGrid);
        }
    }

}
