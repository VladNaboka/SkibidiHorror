using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlatform : MonoBehaviour
{
    public FirstPersonController pcController;       // �� ����������
    public FPSMobile mobileController;   // ��������� ����������
    public GameObject mobileUI;
    public KeysController keysController;

    private void Awake()
    {
        // ���������, ����� ��������� � ������ ������
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // �������� �� ���������� � ��������� ���������
            keysController.spawnPosX = 795;
            keysController.spawnPosY = 475;
            mobileUI.SetActive(false);
            pcController.enabled = true;
            mobileController.enabled = false;
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // �������� ��������� ���������� � ��������� ��
            keysController.spawnPosX = 1800;
            keysController.spawnPosY = 975;
            mobileUI.SetActive(true);
            pcController.enabled = false;
            mobileController.enabled = true;
        }
        else
        {
            // ���� ��������� ����������, ��������� ��� �����������
            pcController.enabled = false;
            mobileController.enabled = false;
        }
    }
}
