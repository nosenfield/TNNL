using System;
using TMPro;
using TNNL.Collidables;
using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class HealthTextUI : MonoBehaviour
    {
        [SerializeField] Color DefaultColor;
        [SerializeField] Color WarningColor;
        [SerializeField] Color CriticalColor;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] GameObject criticalAlert;
        [SerializeField] Image uiHousing;

        void Start()
        {
            ShieldModel.HealthUpdate += HealthUpdateListener;
        }

        void HealthUpdateListener(float shieldHealth)
        {
            if (shieldHealth < .3f)
            {
                healthText.color = CriticalColor;
                uiHousing.color = CriticalColor;
                criticalAlert.SetActive(true);

            }
            else if (shieldHealth < .5f)
            {
                healthText.color = WarningColor;
                uiHousing.color = WarningColor;
                criticalAlert.SetActive(false);
            }
            else
            {
                healthText.color = DefaultColor;
                uiHousing.color = DefaultColor;
                criticalAlert.SetActive(false);
            }

            if (shieldHealth >= .1f)
            {
                healthText.text = $"{Math.Max(0, Math.Round(shieldHealth * 100)).ToString()}%";
            }
            else
            {
                healthText.text = $"{Math.Max(0, (shieldHealth * 100)).ToString("0.0")}";
            }


        }

    }
}