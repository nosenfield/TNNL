using System;
using TMPro;
using TNNL.Events;
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

        void Awake()
        {
            healthText.color = DefaultColor;
            uiHousing.color = DefaultColor;
            criticalAlert.SetActive(false);
        }

        void Start()
        {
            EventAggregator.Subscribe<ShieldHealthUpdateEvent>(ShieldHealthUpdateEventListener);
        }

        void ShieldHealthUpdateEventListener(object e)
        {
            ShieldHealthUpdateEvent shieldHealthUpdateEvent = (ShieldHealthUpdateEvent)e;
            if (shieldHealthUpdateEvent.PercentHealth < .3f)
            {
                healthText.color = CriticalColor;
                uiHousing.color = CriticalColor;
                criticalAlert.SetActive(true);

            }
            else if (shieldHealthUpdateEvent.PercentHealth < .5f)
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

            if (shieldHealthUpdateEvent.PercentHealth >= .1f)
            {
                healthText.text = $"{Math.Max(0, Math.Round(shieldHealthUpdateEvent.PercentHealth * 100)).ToString()}%";
            }
            else
            {
                healthText.text = $"{Math.Max(0, (shieldHealthUpdateEvent.PercentHealth * 100)).ToString("0.0")}";
            }


        }

    }
}