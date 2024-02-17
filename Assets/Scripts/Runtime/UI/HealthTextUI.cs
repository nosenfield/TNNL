using System;
using TMPro;
using TNNL.Collidables;
using TNNL.Player;
using UnityEngine;

namespace TNNL.UI
{
    public class HealthTextUI : MonoBehaviour
    {
        [SerializeField] Color DefaultColor;
        [SerializeField] Color WarningColor;
        [SerializeField] Color CriticalColor;
        [SerializeField] TextMeshProUGUI healthText;

        void Start()
        {
            ShieldModel.HealthUpdate += HealthUpdateListener;
        }

        void HealthUpdateListener(float shieldHealth)
        {
            if (shieldHealth < .3f)
            {
                healthText.color = CriticalColor;
            }
            else if (shieldHealth < .5f)
            {
                healthText.color = WarningColor;
            }
            else
            {
                healthText.color = DefaultColor;
            }


            healthText.text = $"{Math.Round(shieldHealth * 100).ToString()}%";
        }

    }
}