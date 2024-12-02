using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public PlayerData playerData;

    public List<LevelData> levelDatas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
            CalculateTimeDifference();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadGame()
    {
        playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        if(playerData == null) playerData = new PlayerData();
        PlayerPrefs.Save();
    }
    private void SaveGame()
    {
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData).ToString());
        PlayerPrefs.Save();
    }    


     private void OnApplicationQuit()
    {
        SaveGame();
        SaveLastQuitTime();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
            SaveLastQuitTime();
        }
    }

    private void SaveLastQuitTime()
    {
        PlayerPrefs.SetString("LastQuitTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void CalculateTimeDifference()
    {
        string lastQuitTimeString = PlayerPrefs.GetString("LastQuitTime", null);
        if (!string.IsNullOrEmpty(lastQuitTimeString))
        {
            DateTime lastQuitTime = DateTime.Parse(lastQuitTimeString);
            TimeSpan timeDifference = DateTime.Now - lastQuitTime;
            if(playerData.hearts < 5)
            {
                playerData.hearts = Mathf.Min(5, playerData.hearts + (int)(timeDifference.TotalSeconds / 1200));

            }
        }
    }
}

[System.Serializable]
public class PlayerData{
    public int hearts = 5;
    public int lives = 0;
    public int plays = 0;
    public int coins = 10000;
    public int level = 0;
    public int spell0 = 5, spell1 = 5, spell2 = 5, spell3 = 5;
    public int timeHeart = 1200;

    public bool isMusicOn = true, isSoundOn = true, isVibrationOn  = true;
}