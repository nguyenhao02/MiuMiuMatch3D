using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFunnyQuestion : MonoBehaviour
{
    public List<GameObject> btnNos;
    public TextMeshProUGUI textQuestion;
    public GameObject uiReward;
    public GameObject uiNonReward;
    public Image imageReward;
    public TextMeshProUGUI textReward;
    public Sprite heartSprite;

    public UIData uiData;

    private string[] textQuestions = new string[5];
    private int numPlays = 0;
    void Start()
    {
        ShowBtnNo();
        SetTextQuestions();
    }

    private void SetTextQuestions()
    {
        textQuestions[0] = "Bạn có thấy game này hay không ?";
        textQuestions[1] = "Game này xứng đáng 5* chứ ?";
        textQuestions[2] = "Đây có phải game hay nhất bạn từng chơi không ?";
        textQuestions[3] = "Bạn có trả lời thật lòng và tự nguyện không?";
        textQuestions[4] = "Bạn muốn có cơ hội nhận 1 phần quà nho nhỏ chứ";
    }

    public void ShowBtnNo()
    {

        foreach(GameObject btnNo in btnNos)
        {
            btnNo.SetActive(false);
        }

        int index = Random.Range(0, 20);
        btnNos[index].SetActive(true);

    }

    public void ClickBtnYes()
    {
        if(numPlays == 5)
        {
            numPlays = 0;

            int winRate = 15;
            int index = Random.Range(0, 101);

            if(index <= winRate )
            {
                imageReward.sprite = heartSprite;
                textReward.text = "+1";
                DataManager.Instance.playerData.hearts ++;
                uiData.UpdateUIData();

                SoundManager.Instance.PlaySFX(SoundManager.Instance.winGame);
                uiReward.SetActive(true);
                uiReward.GetComponent<Animator>().SetTrigger("isAppear");
            }
            else
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.loseGame);
                uiNonReward.SetActive(true);
                uiNonReward.GetComponent<Animator>().SetTrigger("isAppear");
            }
            
            gameObject.SetActive(false);
        }
        else
        {
            textQuestion.text = textQuestions[numPlays];
            numPlays ++;
        }
        
    }

}
