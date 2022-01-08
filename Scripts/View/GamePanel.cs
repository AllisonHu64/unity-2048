using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{
    #region UI controller
    public Text text_score; // current score
    public Text text_best_score; // best score

    public Button btn_restart;

    public Button btn_last;

    public Button btn_exit;

    public WinPanel winPanel;
    public LosePanel losePanel;
    #endregion

    #region Property Variable
    public Transform girdParent; // parent of grid

    private int row; // number of rows

    private int col; // number of columns

    public GameObject gridPrefab;
    public GameObject numberPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedCreateNumber = false;

    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();
    public Dictionary<int,int> grid_config = new Dictionary<int, int>() {
        {4, 150}, {5, 115}, {6, 90}
    };

    public MyGrid[][] grids = null; // save all the generated grids

    public int currentScore = 0;

    public StepModel lastStepModel;
    #endregion

    #region Game Logic
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
    
    public void CreateNumber(MyGrid myGrid, int number){
        GameObject gameObject = GameObject.Instantiate(numberPrefab,myGrid.transform);
        gameObject.GetComponent<Number>().Init(myGrid);
        myGrid.GetNumber().SetNumber(number);
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
                                
                                if (grids[m][j].IsHaveNumber()){
                                    Number targetNumber = grids[m][j].GetNumber();
                                    HandleNumber(currentNumber,targetNumber, grids[m][j]);
                                    break;
                                }
                                HandleNumber(currentNumber,null,grids[m][j]);

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
                    
                                if (grids[m][j].IsHaveNumber()){
                                    Number targetNumber = grids[m][j].GetNumber();
                                    HandleNumber(currentNumber,targetNumber, grids[m][j]);
                                    break;
                                }
                                HandleNumber(currentNumber,null,grids[m][j]);

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

                                if (grids[i][m].IsHaveNumber()){
                                    Number targetNumber = grids[i][m].GetNumber();
                                    HandleNumber(currentNumber,targetNumber, grids[i][m]);
                                    break;
                                }
                                HandleNumber(currentNumber,null,grids[i][m]);

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
                                
                                if (grids[i][m].IsHaveNumber()){
                                    Number targetNumber = grids[i][m].GetNumber();
                                    HandleNumber(currentNumber,targetNumber, grids[i][m]);
                                    break;
                                }
                                HandleNumber(currentNumber,null,grids[i][m]);

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
            if (current.IsMerge(target)){
                target.Merge();
                // clear current number;
                current.GetGrid().SetNumber(null);
                GameObject.Destroy(current.gameObject);
                isNeedCreateNumber = true;
            }
        }else{
            // move to grid
            current.MoveToGrid(targetGrid);
            isNeedCreateNumber = true;
        }
    }

    public void ResetNumberStatus() {
        for (int i = 0; i < row; i++){
            for(int j = 0; j < col; j++){
                // check if the grid is empty or not
                if(grids[i][j].IsHaveNumber()){
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    // check if game is lost
    public bool IsGameLose() {
        // check if the grid is full
        for (int i = 0; i < row; i++){
            for(int j = 0; j < col; j++) {
                if (!grids[i][j].IsHaveNumber()){
                    return false;
                }
            }
        }

        // check if merge is possible
        for (int i = 0; i < row; i ++) {
            for (int j = 0; j < col; j++) {

                MyGrid up = IsHaveGrid(i-1,j)? grids[i - 1][j] : null;
                MyGrid right = IsHaveGrid(i,j+1)? grids[i][j + 1] : null;

                if (up != null){
                    if (grids[i][j].GetNumber().IsMerge(up.GetNumber())){
                        return false;
                    }
                }

                if (right != null){
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber())){
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public bool IsHaveGrid(int i, int j){
        if (i >= 0 && i < row && j >= 0 && j < col){
            return true;
        }
        return false;
    }
    #endregion

    #region LifeCycle
    private void Awake() {
        // initiate UI
        InitPanelInfo();
        // initate grid
        InitGrid();
        // create number
        CreateNumber();

    }

    #endregion

    #region User Event
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

        lastStepModel.UpdateData(this.currentScore, PlayerPrefs.GetInt(Const.BestScore, 0), grids);
        this.btn_last.interactable = true;

        MoveType moveType = calculateMoveTpye();
        Debug.Log("movetype: " + moveType);
        MoveNumber(moveType);

        if (isNeedCreateNumber){
            CreateNumber();
        }

        // change the number status to normal
        ResetNumberStatus();
        isNeedCreateNumber = false;

        // check if game is lost
        if (IsGameLose()) { // meaning game is lost
            GameLose();
        }
    }


    public void OnLastMoveClick(){
        BackToLastStep();
        btn_last.interactable = false;
    }

    public void OnRestartClick(){
        RestartGame();
    }

    public void OnExitGameClick(){
        ExitGame();
    }

    #endregion

    #region Update UI
    
    public void InitPanelInfo(){
        this.text_best_score.text = PlayerPrefs.GetInt(Const.BestScore, 0).ToString();
        this.lastStepModel = new StepModel();
        this.btn_last.interactable = false;
    }

    public void AddScore(int score){
        currentScore += score;
        UpdateScore(currentScore);

        // check if it is best score
        if (currentScore > PlayerPrefs.GetInt(Const.BestScore, 0)){
            PlayerPrefs.SetInt(Const.BestScore, currentScore);
            UpdateBestScore(currentScore);
        }
    }

    public void UpdateScore(int score){
        this.text_score.text = score.ToString();
    }

    public void ResetScore(){
        currentScore = 0;
        UpdateScore(currentScore);
    }

    public void UpdateBestScore(int bestScore){
        this.text_best_score.text = bestScore.ToString();        
    }
    #endregion

    #region GameLifeCycle

    public void BackToLastStep(){
        // reset score back
        currentScore = lastStepModel.score;
        UpdateScore(lastStepModel.score);
        // reset best score back
        PlayerPrefs.SetInt(Const.BestScore, lastStepModel.bestScore);
        UpdateBestScore(lastStepModel.bestScore);
        // reset grid
        for (int i = 0; i < row; i++){
            for (int j = 0; j < col; j++){
                if (lastStepModel.numbers[i][j] == 0 && grids[i][j].IsHaveNumber()){
                    GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                    grids[i][j].SetNumber(null);
                }
                else if (lastStepModel.numbers[i][j] != 0 && !grids[i][j].IsHaveNumber()){
                    // create number
                    CreateNumber(grids[i][j],lastStepModel.numbers[i][j]);
                }
                else if (lastStepModel.numbers[i][j] != 0 && grids[i][j].IsHaveNumber()){
                    // change number
                    grids[i][j].GetNumber().SetNumber(lastStepModel.numbers[i][j]);
                }
            }
        }
    }

    public void ExitGame() {
        SceneManager.LoadSceneAsync(0);
    }
    public void RestartGame(){
        // clear datar
        this.btn_last.interactable = false;
        // clear score
        ResetScore();

        // clear number
        for (int i = 0; i < row; i ++){
            for (int j = 0; j < col; j++) {
                if (grids[i][j].IsHaveNumber()){
                    GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                    grids[i][j].SetNumber(null);
                }
            }
        }

        // create a number
        CreateNumber();
    }

    public void GameWin() {
        Debug.Log("Game Win");
        winPanel.Show();
    }

    public void GameLose() {
        Debug.Log("Game Lose");
        losePanel.Show();
    }
    #endregion 
}
