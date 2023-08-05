using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject doorObj;
    [SerializeField] private KeysController keysController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && keysController.currentIndex == 4)
        {
            doorObj.transform.DORotate(new Vector3(-90, 0, 0f), 2f);
        }
    }
}
