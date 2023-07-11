using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private float _maxStamina = 100.0f;
    private Slider _staminaBar;
    public float stamina = 100.0f;

    private void Awake()
    {
        _staminaBar = GetComponent<Slider>();
        SetValue();
    }
    private void Update()
    {
        _staminaBar.value = stamina;
    }
    private void SetValue()
    {
        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _maxStamina;
    }
}
