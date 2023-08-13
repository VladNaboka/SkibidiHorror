using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CheckPlatform : MonoBehaviour
{
    public FirstPersonController pcController;       // �� ����������
    public FPSMobile mobileController;   // ��������� ����������
    public GameObject mobileUI;
    public KeysController keysController;

    private void Awake()
    {
        // ���������, ����� ��������� � ������ ������
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            // �������� �� ���������� � ��������� ���������
            keysController.spawnPosX = 795;
            keysController.spawnPosY = 475;
            mobileUI.SetActive(false);
            pcController.enabled = true;
            mobileController.enabled = false;
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            // �������� ��������� ���������� � ��������� ��
            keysController.spawnPosX = 1800;
            keysController.spawnPosY = 975;
            mobileUI.SetActive(true);
            pcController.enabled = false;
            mobileController.enabled = true;
        }
        //else
        //{
        //    // ���� ��������� ����������, ��������� ��� �����������
        //    pcController.enabled = false;
        //    mobileController.enabled = false;
        //}
    }
}
