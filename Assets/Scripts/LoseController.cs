using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController : MonoBehaviour
{
    [SerializeField] private MobController mobController;
    [SerializeField] private GameObject loseMenu;

    private void OnEnable()
    {
        mobController.OnPlayerCaught += Lose;
    }
    private void OnDisable()
    {
        mobController.OnPlayerCaught -= Lose;
    }

    private void Lose(Transform transform)
    {
        loseMenu.SetActive(true);
    }
}
