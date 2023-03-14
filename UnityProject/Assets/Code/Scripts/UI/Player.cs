using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject
{
    public string playerName;
    public float damagePercent;
    public int stocks;


    public void Print()
    {
        Debug.Log(playerName);
    
    }

}
