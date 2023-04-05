using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelDisplay : MonoBehaviour
{
    public Player player;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI damageText;
    public int stocks;

    public Image Stock_1;
    public Image Stock_2;
    public Image Stock_3;
    public Image Stock_4;

    Image[] stockArray = new Image[4];


    // Start is called before the first frame update
    void Start()
    {
        /*
        Stock_1 = GameObject.Find("Stock_1").GetComponent<Image>();
        Stock_2 = GameObject.Find("Stock_2").GetComponent<Image>();
        Stock_3 = GameObject.Find("Stock_3").GetComponent<Image>();
        Stock_4 = GameObject.Find("Stock_4").GetComponent<Image>();
        */

        stockArray[0] = Stock_1;
        stockArray[1] = Stock_2;
        stockArray[2] = Stock_3;
        stockArray[3] = Stock_4;

        nameText.SetText(player.playerName);
        
        stocks = player.stocks;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            Stock_1.SetActive(false);
        }
        */

        //Stock_1.enabled = false;
        
        if (stocks != player.stocks)
        {
            stocks = player.stocks;
            stockArray[stocks].enabled = false;
        }

        damageText.text = string.Format("{0:F1}%", player.damagePercent);
    }

    // function to remove stock from UI pannel when player dies
    public void DeleteStock()
    {
        stockArray[stocks - 1].enabled = false;
        stocks--;

    }
}
