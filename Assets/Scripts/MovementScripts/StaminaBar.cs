using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private float _maxStamina = 100.0f;
    private Slider _staminaBar;


    private void OnEnable()
    {
        FirstPersonController.OnStaminaChange += UpdateValue;
    }

    private void OnDisable()
    {
        FirstPersonController.OnStaminaChange -= UpdateValue;
    }
    private void Awake()
    {
        _staminaBar = GetComponent<Slider>();
        SetValue();
    }
    private void SetValue()
    {
        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _maxStamina;
    }
    private void UpdateValue(float currentStamina)
    {
        _staminaBar.value = currentStamina;
    }
}
