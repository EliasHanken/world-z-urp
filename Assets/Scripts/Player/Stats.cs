using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public string playerName;
    public int kills;
    public int money;
    public TextMeshProUGUI moneyDisplay;

    void Update(){
        moneyDisplay.text = money.ToString() + "$";
    }
}
