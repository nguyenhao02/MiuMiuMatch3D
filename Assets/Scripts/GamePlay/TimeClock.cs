using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeClock : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public Slider sliderTime;
    public GameObject uIFreezeTime;
    public GameObject textTimeAdd, textTimeMinus;
    public List<Image> timeStars;

    private int remainingTime;
    private int countdownTime;  
    private int freezeTime = 25;
    private bool canCount = true, canFreeze = true; 
    private bool isfreezing = false;
    private Color green, blue, colorAlpha0, colorAlpha1;

    private UIController uIController;

    void Start()
    {
        uIController = GetComponent<UIController>();
       

        SetColor();
    }

    public void PauseTime()
    {
        canCount = false;
        canFreeze = false;
    }

    public void PlayTime()
    {
        canFreeze = true;
        if(isfreezing) return;
        canCount = true;
       
    }

    public float TimeRate()
    {
        return (float)remainingTime/countdownTime;
        
    }

    private void SetColor()
    {
        ColorUtility.TryParseHtmlString("#2CFF00", out green);
        ColorUtility.TryParseHtmlString("#00EEFF", out blue);
        colorAlpha0 = timeStars[1].color;
        colorAlpha0.a = 0f;
        colorAlpha1 = timeStars[1].color;
        colorAlpha1.a = 1f;

    }

    public IEnumerator StartCountdown()
    {
        int level = DataManager.Instance.playerData.level;
        countdownTime = DataManager.Instance.levelDatas[level].time;
        remainingTime = countdownTime;

        sliderTime.fillRect.GetComponent<Image>().color = green;
        
        while (remainingTime > 0)
        {
            //if(remainingTime > countdownTime) countdownTime = remainingTime;

            if (canCount)
            {
                int minute = remainingTime / 60;
                int second = remainingTime % 60;
                float timeRate = (float)remainingTime/countdownTime;

                textTime.text = minute.ToString("D2") + " : " + second.ToString("D2");
                sliderTime.value = timeRate;

                CheckStarTime(timeRate);

                yield return new WaitForSeconds(1);
                remainingTime--;
            }
            else
            {
                yield return null; 
            }

            if(remainingTime < 30 && canCount) textTime.GetComponent<Animator>().SetBool("isTimeRed", true);
            else textTime.GetComponent<Animator>().SetBool("isTimeRed", false);
        }

        textTime.text = "Time Up";
        uIController.OpenUILoseGameTimeUp();
    }

    private void CheckStarTime(float timeRate)
    {
        if(timeRate < 0.25f)
        {
            timeStars[1].color = colorAlpha0; 
        }
        else 
        {
            timeStars[1].color = colorAlpha1; 
        }

        if(timeRate < 0.5f)
        {
            timeStars[2].color = colorAlpha0; 
        }
        else 
        {
            timeStars[2].color = colorAlpha1; 
        }

    }

    public void AddTimeClock(int time)
    {
        if(isfreezing) return;
        StartCoroutine(StartAddTimeClock(time));
    }
    
    private IEnumerator StartAddTimeClock(int time)
    {
        yield return new WaitForSeconds(1f);
        
        remainingTime += time;

        int minute = remainingTime / 60;
        int second = remainingTime % 60;
        textTime.text = minute.ToString("D2") + " : " + second.ToString("D2");
        textTime.GetComponent<Animator>().SetTrigger("isTimeUp");

        if(remainingTime <= time) 
        {
            StartCoroutine(StartCountdown());
        }

        textTimeAdd.SetActive(true);
        textTimeAdd.GetComponent<TextMeshProUGUI>().SetText("+ " + time.ToString());
        yield return new WaitForSeconds(0.5f);
        textTimeAdd.SetActive(false);

    }

    public void MinusTimeClock(int time)
    {
        if(isfreezing) return;
        StartCoroutine(OffTextTimeMinus(time));
    }

    private IEnumerator OffTextTimeMinus(int time)
    {
        yield return new WaitForSeconds(1f);

        if(remainingTime > time) remainingTime -= time;
        else remainingTime = 0;

        int minute = remainingTime / 60;
        int second = remainingTime % 60;
        textTime.text = minute.ToString("D2") + " : " + second.ToString("D2");
        textTime.GetComponent<Animator>().SetTrigger("isTimeDown");

        textTimeMinus.SetActive(true);
        textTimeMinus.GetComponent<TextMeshProUGUI>().SetText("- " + time.ToString());

        yield return new WaitForSeconds(0.5f);
        textTimeMinus.SetActive(false);
    }

    public void Spell3()
    {
        //An sao
        for(int  i = 0; i <=2; i++ )
        {
            timeStars[i].transform.gameObject.SetActive(false);
        }

        canCount = false;
        uIFreezeTime.SetActive(true);
        isfreezing = true;
        StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        sliderTime.fillRect.GetComponent<Image>().color = blue;

        int freezeRemainingTime = freezeTime;

        while(freezeRemainingTime > 0)
        {
            canCount = false;
            if(canFreeze)
            {
                sliderTime.value = (float)freezeRemainingTime/freezeTime;
                yield return new WaitForSeconds(1);
                freezeRemainingTime--;
            }
            else
            {
                yield return null; 
            }
        }
        isfreezing = false;
        canCount = true;
        uIFreezeTime.SetActive(false);
        sliderTime.fillRect.GetComponent<Image>().color = green;

        // Hien sao
         for(int  i = 0; i <=2; i++ )
        {
            timeStars[i].transform.gameObject.SetActive(true);
        }
    }

}
