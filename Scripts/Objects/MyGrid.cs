using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public Number number; // current number of this grid

    // check if have number
    public bool IsHaveNumber(){
        return number != null;
    }


    public Number GetNumber(){
        return number;
    }


    public void SetNumber(Number number){
        this.number = number;

    }
}
