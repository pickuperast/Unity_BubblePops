using UnityEngine;
using System.Collections;

public class Score  {
    int _score;
    public int GetScore(){
        return _score;
    }
    public void SetScore(int score){
        _score = score;
    }
	
    // fomular score scale 
    public int CalculateScore(int pointSameColor, int fallingDown, int ballNumber){
        return pointSameColor * ballNumber  + fallingDown * ballNumber; 
    }

}
