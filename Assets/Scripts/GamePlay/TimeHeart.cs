using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class TimeHeart : MonoBehaviour
{
    public TextMeshProUGUI textHearts, textTimeHeart;
    private int maxHearts = 5;
    private int countdownTime = 1200;
    private int remainingTime;

    void Start()
    {
        StartCoroutine(SetTimeHeart());
    }

    private IEnumerator SetTimeHeart()
    {
        remainingTime = DataManager.Instance.playerData.timeHeart;

        while(true)
        {
            if(DataManager.Instance.playerData.hearts < maxHearts)
            {
                if(remainingTime < 0)
                {
                    DataManager.Instance.playerData.hearts ++;

                    UpdateTextHeart();
                    //Reset remainingTime
                    DataManager.Instance.playerData.timeHeart = countdownTime;
                    remainingTime = countdownTime;
                }
                else
                {
                    UpdateTextTimeHeart();
                    
                    yield return new WaitForSeconds(1);
                    remainingTime --;

                    // lưu lại timeheart
                    DataManager.Instance.playerData.timeHeart --;
                }
                
            }
            else
            {
                textTimeHeart.SetText("FULL");
            }

            yield return null; 
        }
    }

    private void UpdateTextHeart()
    {
        textHearts.SetText(DataManager.Instance.playerData.hearts.ToString());
    }

    private void UpdateTextTimeHeart()
    {
        int minute = remainingTime / 60;
        int second = remainingTime % 60;
        textTimeHeart.text = minute.ToString("D2") + " : " + second.ToString("D2");
    }
}
