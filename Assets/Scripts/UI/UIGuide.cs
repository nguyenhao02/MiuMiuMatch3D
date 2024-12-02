using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuide : MonoBehaviour
{
    public GameObject uiGuide1, uiGuide4;
    public GameObject uiGuideBoom, uiGuideSandGlass, uiGuideRocket;
    public GameObject parentObjects;
    public List<GameObject> objects;

    public Image itemImage;
    public TextMeshProUGUI textInfor;
    public TextMeshProUGUI textName;
    public List<Sprite> itemSprite;
    public List<string> itemInfor;
    public List<string> itemName;

    public GameObject btnUIGuide4;

    private int index = 0;

    void Start()
    {
        OpenGuide();

        itemImage.sprite = itemSprite[index];
        textInfor.text = itemInfor[index];
        textName.text = itemName[index];
    }

    private void OpenGuide()
    {
        int level = DataManager.Instance.playerData.level;
        if(level > 0 && level < 5) btnUIGuide4.SetActive(true);

        if(level == 0)
        {
            ClearParentObjects();
            uiGuide1.SetActive(true);
            return;
        } 
        
        if(level == 2)
        {
            StartCoroutine(OpenUIGuideBoom());
            return;
        } 

        if(level == 3)
        {
            StartCoroutine(OpenUIGuideSandGlass());
            return;
        }

        if(level == 4)
        {
            StartCoroutine(OpenUIGuideRocket());
            return;
        } 
        
    }

    private IEnumerator OpenUIGuideBoom()
    {
        yield return new WaitForSeconds(2);
        uiGuideBoom.SetActive(true);
        uiGuideBoom.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CloseUIGuideBoom()
    {
        StartCoroutine(StartCloseUIGuide(uiGuideBoom));
    }

    private IEnumerator OpenUIGuideSandGlass()
    {
        yield return new WaitForSeconds(2);
        uiGuideSandGlass.SetActive(true);
        uiGuideSandGlass.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CloseUIGuideSandGlass()
    {
        StartCoroutine(StartCloseUIGuide(uiGuideSandGlass));
    }

    private IEnumerator OpenUIGuideRocket()
    {
        yield return new WaitForSeconds(2);
        uiGuideRocket.SetActive(true);
        uiGuideRocket.GetComponent<Animator>().SetTrigger("isAppear");
    }

    public void CloseUIGuideRocket()
    {
        StartCoroutine(StartCloseUIGuide(uiGuideRocket));
    }

    public void OpenUIGuide4()
    {
        uiGuide4.SetActive(true);
        btnUIGuide4.SetActive(false);
        uiGuide4.GetComponent<Animator>().SetTrigger("isAppear");  
    }
    public void CloseUIGuide4()
    {
        btnUIGuide4.SetActive(true);
        StartCoroutine(StartCloseUIGuide(uiGuide4));
    }

    private IEnumerator StartCloseUIGuide(GameObject uiGuide)
    {
        uiGuide.GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1);
        uiGuide.SetActive(false);
    }

    private void ClearParentObjects()
    {
        foreach (Transform child in parentObjects.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject obj in objects)
        {
            obj.transform.SetParent(parentObjects.transform);
        }
        
    }

    public void ActiveObjects()
    {
        foreach(GameObject obj in objects)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void ClickNext()
    {
        index ++;
        if(index == itemSprite.Count) index = 0;

        itemImage.sprite = itemSprite[index];
        textInfor.text = itemInfor[index];
        textName.text = itemName[index];
    }

    
    
}
