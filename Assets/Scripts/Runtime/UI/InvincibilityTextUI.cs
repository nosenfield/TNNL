using System;
using TMPro;
using TNNL.Collidables;
using TNNL.Events;
using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class InvincibilityTextUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI invincibilityText;
        [SerializeField] GameObject invincibilityUI;

        void Awake()
        {
            EventAggregator.Subscribe<ShieldInvincibilityUpdateEvent>(ShieldInvincibilityUpdateEventListener);
        }

        void ShieldInvincibilityUpdateEventListener(object e)
        {
            ShieldInvincibilityUpdateEvent shieldInvincibilityUpdateEvent = (ShieldInvincibilityUpdateEvent)e;
            if (shieldInvincibilityUpdateEvent.SecondsInvincibleRemaining == 0f)
            {
                invincibilityUI.SetActive(false);

            }
            else
            {
                invincibilityUI.SetActive(true);
                invincibilityText.text = $"{Math.Max(0, shieldInvincibilityUpdateEvent.SecondsInvincibleRemaining).ToString("0.0")}";
            }
        }

    }
}