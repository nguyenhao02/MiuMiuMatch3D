using UnityEngine;

public class SettingFps : MonoBehaviour
{
    void Start()
    {
        int screenRefreshRate = (int)Screen.currentResolution.refreshRateRatio.numerator;

        if (screenRefreshRate >= 120)
        {
            Application.targetFrameRate = 120;
        }
        else if (screenRefreshRate >= 60)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }

        //Debug.Log("Screen Refresh Rate: " + screenRefreshRate + " Hz");
        //Debug.Log("Target Frame Rate: " + Application.targetFrameRate + " FPS");
    }
}
