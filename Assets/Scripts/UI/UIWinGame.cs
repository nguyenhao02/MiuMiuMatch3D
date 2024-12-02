using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public class UIWinGame : MonoBehaviour
{
    public GameObject uiWinGame;
    public GameObject uiData;
    public GameObject star1, star2, star3;
    public TextMeshProUGUI textCoinsCollect;

    private int coinsCollect;
    private int numStar;
    private TimeClock timeClock;

    void Awake()
    {
        timeClock = GetComponent<TimeClock>();
    }

    public void OpenUIWinGame()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.winGame);

        float timeRate = timeClock.TimeRate();
        if(timeRate >= 0.5f) numStar = 3;
        else if(timeRate >= 0.25f) numStar = 2;
            else numStar = 1;

        coinsCollect = numStar * 30;

        textCoinsCollect.text = " + " + coinsCollect.ToString();

        uiWinGame.SetActive(true);
        uiWinGame.GetComponent<Animator>().SetTrigger("isAppear");

        uiData.SetActive(true);
        uiData.GetComponent<Animator>().SetTrigger("isAppear");

        StartCoroutine(ShowStarWinGame());
    }

    private IEnumerator ShowStarWinGame()
    {
        yield return new WaitForSeconds(0.5f);
        star1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.star1);

        if(numStar == 1) yield break;
        star2.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.star2);
        
        if(numStar == 2) yield break;
        star3.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.star3);
    }

    public void ClickWinGame()
    {
        
        uiWinGame.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");

        // Thêm lại lượt chơi và level
        DataManager.Instance.playerData.lives = 0;
        DataManager.Instance.playerData.level++;
        DataManager.Instance.playerData.coins += coinsCollect;
         DataManager.Instance.playerData.plays = 0;

        StartCoroutine(ReloadSceneWin());
    }

    private IEnumerator ReloadSceneWin()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MatchScene");
    }
}
