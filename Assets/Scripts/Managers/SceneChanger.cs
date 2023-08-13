using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
    public void ChangeSceneWithAd(string name)
    {
        YandexGame.FullscreenShow();
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void ChangeScenePlatform()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            SceneManager.LoadScene("KeysScene");
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            SceneManager.LoadScene("KeysMobile");
        }
        //else
        //{
        //    SceneManager.LoadScene("KeysScene");
        //}
    }
}
