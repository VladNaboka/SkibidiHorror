using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlatform : MonoBehaviour
{
    public FirstPersonController pcController;       // ПК контроллер
    public FPSMobile mobileController;   // Мобильный контроллер
    public GameObject mobileUI;
    public KeysController keysController;

    private void Awake()
    {
        // Проверяем, какая платформа в данный момент
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Включаем ПК контроллер и выключаем мобильный
            keysController.spawnPosX = 795;
            keysController.spawnPosY = 475;
            mobileUI.SetActive(false);
            pcController.enabled = true;
            mobileController.enabled = false;
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Включаем мобильный контроллер и выключаем ПК
            keysController.spawnPosX = 1800;
            keysController.spawnPosY = 975;
            mobileUI.SetActive(true);
            pcController.enabled = false;
            mobileController.enabled = true;
        }
        else
        {
            // Если платформа неизвестна, выключаем оба контроллера
            pcController.enabled = false;
            mobileController.enabled = false;
        }
    }
}
