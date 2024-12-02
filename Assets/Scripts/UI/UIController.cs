using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public List<GameObject> btnSpells;
    public GameObject uiLoseGameFullBox;
    public GameObject uiLoseGameTimeUp;
    public GameObject uiFailed;
    public GameObject uiData;
    public GameObject uiShop;
    public GameObject uiSetting;

    public List<GameObject> uiSpells;

    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textLevelBroad;
    public TextMeshProUGUI textSpell0, textSpell1, textSpell2, textSpell3;

    private MatchGameController matchGameController;
    private TimeClock timeClock;
    private UIData uIData;
    

    private bool canSpell3 = true;
    private bool canSpell1 = true;

    void Awake()
    {
        matchGameController = GetComponent<MatchGameController>();
        timeClock = GetComponent<TimeClock>();
        uIData = GetComponent<UIData>();
    }
    void Start()
    {
        UpdateTextLevel();
        UpdateTextSpell();
    }

    private void UpdateTextLevel()
    {
        textLevel.SetText("Level: " + DataManager.Instance.playerData.level.ToString());
        textLevelBroad.SetText("Level " + DataManager.Instance.playerData.level.ToString());
    }

    public void UpdateTextSpell()
    {
        textSpell0.SetText(DataManager.Instance.playerData.spell0.ToString());
        textSpell1.SetText(DataManager.Instance.playerData.spell1.ToString());
        textSpell2.SetText(DataManager.Instance.playerData.spell2.ToString());
        textSpell3.SetText(DataManager.Instance.playerData.spell3.ToString());
    }

    public void ClickBtnSpell0()
    {
        if(DataManager.Instance.playerData.spell0 == 0)
        {
            timeClock.PauseTime();

            uiData.SetActive(true);
            uiData.GetComponent<Animator>().SetTrigger("isAppear");

            uiSpells[0].SetActive(true);
            uiSpells[0].GetComponent<Animator>().SetTrigger("isAppear");
        }
        else
        {
            if(!matchGameController.Spell0()) return;
            DataManager.Instance.playerData.spell0 --;
            UpdateTextSpell();

            btnSpells[0].GetComponent<Animator>().SetTrigger("isClick");
        }
        
    }
    public void ClickBtnSpell1()
    {
        if(!canSpell1) return;

        if(DataManager.Instance.playerData.spell1 == 0)
        {
            timeClock.PauseTime();

            uiData.SetActive(true);
            uiData.GetComponent<Animator>().SetTrigger("isAppear");

            uiSpells[1].SetActive(true);
            uiSpells[1].GetComponent<Animator>().SetTrigger("isAppear");
        }
        else
        {
            canSpell1 = false;

            DataManager.Instance.playerData.spell1--;
            UpdateTextSpell();

            btnSpells[1].GetComponent<Animator>().SetTrigger("isClick");
            matchGameController.Spell1();

            StartCoroutine(WaitingSpell1());
        }
    }
    private IEnumerator WaitingSpell1()
    {
        yield return new WaitForSeconds(3.5f);
        canSpell1 = true;
    }

    public void ClickBtnSpell2()
    {
        if(DataManager.Instance.playerData.spell2 == 0)
        {
            timeClock.PauseTime();

            uiData.SetActive(true);
            uiData.GetComponent<Animator>().SetTrigger("isAppear");

            uiSpells[2].SetActive(true);
            uiSpells[2].GetComponent<Animator>().SetTrigger("isAppear");
        }
        else
        {
            if(!matchGameController.Spell2()) return;

            DataManager.Instance.playerData.spell2 --;
            UpdateTextSpell();

            btnSpells[2].GetComponent<Animator>().SetTrigger("isClick");
        }
    }

    public void ClickBtnSpell3()
    {
        if(!canSpell3) return;
        if(DataManager.Instance.playerData.spell3 == 0)
        {
            timeClock.PauseTime();

            uiData.SetActive(true);
            uiData.GetComponent<Animator>().SetTrigger("isAppear");

            uiSpells[3].SetActive(true);
            uiSpells[3].GetComponent<Animator>().SetTrigger("isAppear");
        }
        else
        {
            DataManager.Instance.playerData.spell3 --;
            UpdateTextSpell();

            canSpell3 = false;

            btnSpells[3].GetComponent<Animator>().SetTrigger("isClick");
            timeClock.Spell3();

            matchGameController.Spell3();

            StartCoroutine(WaitingSpell3());
        }
    }

    private IEnumerator WaitingSpell3()
    {
        yield return new WaitForSeconds(25);
        canSpell3 = true;
    }

    public void ClickBuySpell0()
    {
        if(DataManager.Instance.playerData.coins >= 300)
        {
            DataManager.Instance.playerData.coins -= 300;
            DataManager.Instance.playerData.spell0 += 3;
            UpdateTextSpell();
            uIData.UpdateUIData();
            CloseUISpell(uiSpells[0]);
            //uiSpells[0].SetActive(false);
        }
        else
        {
            uiShop.SetActive(true);
        }
    }

    public void ClickBuySpell1()
    {
        if(DataManager.Instance.playerData.coins >= 400)
        {
            DataManager.Instance.playerData.coins -= 400;
            DataManager.Instance.playerData.spell1 += 3;
            UpdateTextSpell();
            uIData.UpdateUIData();
            CloseUISpell(uiSpells[1]);
        }
        else
        {
            uiShop.SetActive(true);
        }
    }

    public void ClickBuySpell2()
    {
        if(DataManager.Instance.playerData.coins >= 300)
        {
            DataManager.Instance.playerData.coins -= 300;
            DataManager.Instance.playerData.spell2 += 3;
            UpdateTextSpell();
            uIData.UpdateUIData();
            CloseUISpell(uiSpells[2]);
        }
        else
        {
            uiShop.SetActive(true);
        }
    }

    public void ClickBuySpell3()
    {
        if(DataManager.Instance.playerData.coins >= 600)
        {
            DataManager.Instance.playerData.coins -= 600;
            DataManager.Instance.playerData.spell3 += 3;
            UpdateTextSpell();
            uIData.UpdateUIData();
            CloseUISpell(uiSpells[3]);
        }
        else
        {
            uiShop.SetActive(true);
        }
    }
    public void CloseUISpell (GameObject uiSpell)
    {
        timeClock.PlayTime();
        StartCoroutine(StartCloseUISpell(uiSpell));
    }

    public IEnumerator StartCloseUISpell(GameObject uiSpell)
    {
        uiSpell.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1f);
        uiSpell.SetActive(false);
        uiData.SetActive(false);
    }

    public void OpenUISetting()
    {
        timeClock.PauseTime();
        uiSetting.SetActive(true);
        uiSetting.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CloseUISetting()
    {
        timeClock.PlayTime();
        StartCoroutine(StartCloseUISetting());
    }

    private IEnumerator StartCloseUISetting()
    {
        uiSetting.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1);
        uiSetting.SetActive(false);
    }


    public void ReLoadScene()
    {
        SceneManager.LoadScene("MatchScene");
    }

    public void ClickClearBar()
    {
        if(DataManager.Instance.playerData.coins < 250)
        {
            uiShop.SetActive(true);
            return;
        }

        DataManager.Instance.playerData.coins -= 250;
        uIData.UpdateUIData();
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        uiLoseGameFullBox.GetComponent<Animator>().SetTrigger("isDisappear");

        StartCoroutine(CloseUIFullBox());
    }

    private IEnumerator CloseUIFullBox()
    {
        yield return new WaitForSeconds(1f);

        uiData.SetActive(false);
        uiLoseGameFullBox.SetActive(false);

        timeClock.AddTimeClock(20);
        timeClock.PlayTime();

        StartCoroutine(matchGameController.ClearBox());
    }

    public void ClickAddTimeClock()
    {
        if(DataManager.Instance.playerData.coins < 250)
        {
            uiShop.SetActive(true);
            return;
        }
        DataManager.Instance.playerData.coins -= 250;
        uIData.UpdateUIData();
        
        uiLoseGameTimeUp.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        StartCoroutine(CloseUITimeUp());
    }

    private IEnumerator CloseUITimeUp()
    {
        timeClock.AddTimeClock(40);

        yield return new WaitForSeconds(1f);
        uiData.SetActive(false);
        uiLoseGameTimeUp.SetActive(false);

        timeClock.PlayTime();
    }

    public void ClickGiveUp()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.loseGame);
        if(!uiData.activeSelf) 
        {
            uiData.SetActive(true);
            uiData.GetComponent<Animator>().SetTrigger("isAppear");
        }
        uiFailed.SetActive(true);
        uiFailed.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void ClickFailed()
    {
        uiLoseGameFullBox.SetActive(false);
        uiLoseGameTimeUp.SetActive(false);
        uiFailed.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        StartCoroutine(ReloadSceneLose());
    }

    public void CloseUIFailed()
    {
        StartCoroutine(StartCloseUIFailed());
    }
    private IEnumerator StartCloseUIFailed()
    {
        uiFailed.GetComponent<Animator>().SetTrigger("isDisappear");
        uiData.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1f);
        uiFailed.SetActive(false);
        uiData.SetActive(false);
    }

    private IEnumerator ReloadSceneLose()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MatchScene");
    }

    public void OpenUILoseGameFullBox()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.loseGame);

        if(uiLoseGameTimeUp.activeSelf) uiLoseGameTimeUp.SetActive(false);;

        timeClock.PauseTime();

        uiLoseGameFullBox.SetActive(true);
        uiLoseGameFullBox.GetComponent<Animator>().SetTrigger("isAppear");


        uiData.SetActive(true);
        uiData.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void OpenUILoseGameTimeUp()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.loseGame);

        if(uiLoseGameFullBox.activeSelf) return;

        timeClock.PauseTime();

        uiLoseGameTimeUp.SetActive(true);
        uiLoseGameTimeUp.GetComponent<Animator>().SetTrigger("isAppear");

        uiData.SetActive(true);
        uiData.GetComponent<Animator>().SetTrigger("isAppear");
    }  
}
