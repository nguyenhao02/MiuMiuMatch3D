using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject uiShop;
    public TextMeshProUGUI textLevel;
    public GameObject uiMain;
    public GameObject uiData;
    public GameObject uiLoading;
    public GameObject uiSetting;
    public GameObject uiGuide;
    public GameObject cannonHardLevel;
    public GameObject uiMaxLevel;
    public Slider loadSlider;
    public GameObject btnSetting;
    public GameObject iconSkull;



    private TaskController taskController;
    private TimeClock timeClock;
    private SetMap setMap;
    private MatchGameController matchGameController;

    void Awake()
    {   
        taskController = GetComponent<TaskController>();
        timeClock = GetComponent<TimeClock>();
        setMap = GetComponent<SetMap>();
        matchGameController = GetComponent<MatchGameController>();
    }


    void Start()
    {
        UpdateDataUI();
        ShowMenuGame();
        CheckHardLevel();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.gameMusic);
    }

    public void UpdateDataUI()
    {
        // Test Game
        if(DataManager.Instance.playerData.level + 1 > DataManager.Instance.levelDatas.Count)
        {
            DataManager.Instance.playerData.level = 0;
            uiMaxLevel.SetActive(true);
            uiMaxLevel.GetComponent<Animator>().SetTrigger("isAppear");
            return;
        }

        textLevel.SetText(DataManager.Instance.playerData.level.ToString());
    }
    public void ShowMenuGame()
    {
        uiMain.GetComponent<Animator>().SetTrigger("isAppear");
        uiData.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CheckHardLevel()
    {
        if((DataManager.Instance.playerData.level % 5 == 0 ) && (DataManager.Instance.playerData.level != 0 ))
        {
            iconSkull.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickBtnPlay()
    {
        if(DataManager.Instance.playerData.hearts <= 0) 
        {
            uiShop.SetActive(true);
            btnSetting.SetActive(false);
            return;
        }
        
        // Tính lượt chơi vs Setmap
        DataManager.Instance.playerData.lives = 1;
        DataManager.Instance.playerData.plays ++;
        taskController.SetTaskCards();
        setMap.LoadLevel();

        uiMain.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        StartCoroutine(ShowMatchGame());
    }

    private IEnumerator ShowMatchGame()
    {
        yield return new WaitForSeconds(0.5f);
        uiLoading.SetActive(true);
        uiLoading.GetComponent<Animator>().SetTrigger("isAppear");
        uiMain.SetActive(false);
        uiData.SetActive(false);

        yield return ShowUILoading();
        //Check uiGuide
        if(DataManager.Instance.playerData.level < 5) uiGuide.SetActive(true);
        
        SoundManager.Instance.PlayMusic(SoundManager.Instance.playMusic);
        matchGameController.PlayPickUp();
        //check hard level
        if((DataManager.Instance.playerData.level % 5 == 0 ) && (DataManager.Instance.playerData.level != 0 ))
        {
            cannonHardLevel.SetActive(true);
        }

        uiLoading.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1f);
        uiLoading.SetActive(false);

        
        // tinhs thời gian matchGame
        StartCoroutine(timeClock.StartCountdown());
    }

    private IEnumerator ShowUILoading()
    {
        float timeLoading = 2.5f;
        float timeCount = 0f;
        while(timeCount < timeLoading)
        {
            loadSlider.value = timeCount / timeLoading;
            timeCount += Time.deltaTime;
            yield return null;
        }
        
    }

    public void OpenUISetting()
    {
        uiSetting.SetActive(true);
        uiSetting.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CloseUISetting()
    {
        StartCoroutine(StartCloseUISetting());
    }

    private IEnumerator StartCloseUISetting()
    {
        uiSetting.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1);
        uiSetting.SetActive(false);
    }


    public void ClickBtnTestGame()
    {
        if(DataManager.Instance.playerData.hearts == 0) DataManager.Instance.playerData.hearts += 5;
        if(DataManager.Instance.playerData.level != 41) DataManager.Instance.playerData.level = 40;
        ClickBtnPlay();
    }
}
