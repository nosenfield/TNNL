using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealthSlider : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Awake()
    {
        Shield.ShieldChanged += SetSliderValue;
    }
    public void SetSliderValue(float amount)
    {
        slider.value = amount;
    }
}