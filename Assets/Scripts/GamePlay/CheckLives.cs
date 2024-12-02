using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLives : MonoBehaviour
{
    private MenuController menuController;
    private UIData uIData;
    void Awake()
    {
        menuController = GetComponent<MenuController>();
        uIData = GetComponent<UIData>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckLivesGame();
    }

    private void CheckLivesGame()
    {
        if(DataManager.Instance.playerData.lives == 1)
        {
            DataManager.Instance.playerData.hearts--;
            DataManager.Instance.playerData.lives = 0;

            menuController.UpdateDataUI();
            uIData.UpdateUIData();
        }
    }
}
