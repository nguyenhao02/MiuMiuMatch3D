using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIData : MonoBehaviour
{
    public TextMeshProUGUI textHearts, textTimeHeart, textCoins;
    void Start()
    {
        UpdateUIData();
    }

    public void UpdateUIData()
    {
        textHearts.SetText(DataManager.Instance.playerData.hearts.ToString());
        textCoins.SetText(DataManager.Instance.playerData.coins.ToString());
    }
}
