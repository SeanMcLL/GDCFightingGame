using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public int playerCount;
    public bool player1Active;
    public bool player2Active;
    //public bool player3Active;
    //public bool player4Active;

    // function called by state machine when player runs out of lives
    public void DisablePlayer(){

    }

    

}