using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobCatchZoneController : MonoBehaviour
{
    [SerializeField] private MobController _mobController;
    [SerializeField] private GameObject _playerGameObject;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _playerGameObject)
        {
            _mobController.CatchPlayer();

        }
    }
}
