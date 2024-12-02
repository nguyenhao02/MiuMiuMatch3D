using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI textInfor;
    public TextMeshProUGUI textName;
    
    public List<Sprite> itemSprite;
    public List<string> itemInfor;
    public List<string> itemName;

    void Start()
    {
        SetDataInfor();
    }

    private void SetDataInfor()
    {
        int index = Random.Range(0, itemName.Count - 1);
        if((DataManager.Instance.playerData.level % 5 == 0 ) && (DataManager.Instance.playerData.level != 0 ))
        {
            index = itemName.Count - 1;
        }
        itemImage.sprite = itemSprite[index];
        textInfor.text = itemInfor[index];
        textName.text = itemName[index];
    }

}
