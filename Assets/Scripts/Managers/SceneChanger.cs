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
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            SceneManager.LoadScene("KeysScene", LoadSceneMode.Single);
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            SceneManager.LoadScene("KeysMobile", LoadSceneMode.Single);
        }
    }
}
