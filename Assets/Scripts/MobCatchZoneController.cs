using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobCatchZoneController : MonoBehaviour
{
    [SerializeField] private GameObject _playerGameObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _playerGameObject)
        {
            Debug.Log("Player lost!");
        }
    }
}
