using UnityEngine;
using UnityEngine.UI;

namespace TNNL.Prototype_1
{
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
}