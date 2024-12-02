using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFunnySpin : MonoBehaviour
{
    public List<Image> boxs;
    public Button btnSpin;
    public Sprite spriteHighLight;
    public Sprite defaultSprite;

    public GameObject uiReward;
    public Image imageReward;
    public TextMeshProUGUI textReward;

    public UIController uIController;
    public UIData uIData;

    public void Spin()
    {
        if(DataManager.Instance.playerData.coins < 250)
        {
            return;
        }

        ResetBox();

        DataManager.Instance.playerData.coins -= 250;
        uIData.UpdateUIData();
        StartCoroutine(PlaySpin());
    }

    private IEnumerator PlaySpin()
    {
        //btnSpin.interactable = false;

        float timeSpin = Random.Range(5f, 8f);
        float delay = 0.05f;
        int index = Random.Range(0, 18);

        while (timeSpin > 0)
        {

            boxs[index].sprite = spriteHighLight;

            SoundManager.Instance.PlaySFX(SoundManager.Instance.objectHighlight);

            yield return new WaitForSeconds(delay);

            boxs[index].sprite = defaultSprite;

            index = (index + 1) % boxs.Count;
            timeSpin -= delay;
            delay *= 1.05f;
        }
        
        boxs[index].sprite = spriteHighLight;
        yield return new WaitForSeconds(1);
        
        //btnSpin.interactable = true;
        ShowUIReward(index);
    }

    private void ResetBox()
    {
        foreach(Image image in boxs)
        {
            image.sprite = defaultSprite;
        }
    }

    private void ShowUIReward(int index)
    {
        switch(index)
        {
            case 0:
                DataManager.Instance.playerData.spell0 += 1;
                textReward.text = "+1";
                break;
            case 1:
                DataManager.Instance.playerData.spell0 += 2;
                textReward.text = "+2";
                break;
            case 2:
                DataManager.Instance.playerData.spell0 += 3;
                textReward.text = "+3";
                break;
            case 3:
                DataManager.Instance.playerData.spell1 += 1;
                textReward.text = "+1";
                break;
            case 4:
                DataManager.Instance.playerData.spell1 += 2;
                textReward.text = "+2";
                break;
            case 5:
                DataManager.Instance.playerData.spell1 += 3;
                textReward.text = "+3";
                break;
            case 6:
                DataManager.Instance.playerData.spell2 += 1;
                textReward.text = "+1";
                break;
            case 7: 
                DataManager.Instance.playerData.spell2 += 2;
                textReward.text = "+2";
                break;
            case 8:
                DataManager.Instance.playerData.spell2 += 3;
                textReward.text = "+3";
                break;
            case 9:
                DataManager.Instance.playerData.spell3 += 1;
                textReward.text = "+1";
                break;
            case 10:
                DataManager.Instance.playerData.spell3 += 2;
                textReward.text = "+2";
                break;
            case 11:
                DataManager.Instance.playerData.spell3 += 3;
                textReward.text = "+3";
                break;
            case 12:
                DataManager.Instance.playerData.hearts += 1;
                textReward.text = "+1";
                break;
            case 13:
                DataManager.Instance.playerData.hearts += 2;
                textReward.text = "+2";
                break;
            case 14:
                DataManager.Instance.playerData.hearts += 3;
                textReward.text = "+3";
                break;
            case 15:
                DataManager.Instance.playerData.coins += 10;
                textReward.text = "+10";
                break;
            case 16:
                DataManager.Instance.playerData.coins += 100;
                textReward.text = "+100";
                break;
            case 17:
                DataManager.Instance.playerData.coins += 1000;
                textReward.text = "+1000";
                break;    
        }
        SoundManager.Instance.PlaySFX(SoundManager.Instance.winGame);

        if(uiReward.activeSelf)  uiReward.SetActive(false);
        uiReward.SetActive(true);
        uiReward.GetComponent<Animator>().SetTrigger("isAppear");
        imageReward.sprite = boxs[index].GetComponentsInChildren<Image>()[1].sprite;
        uIData.UpdateUIData();
        uIController.UpdateTextSpell();
    }
}
