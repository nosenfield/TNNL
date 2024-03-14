using System;
using TMPro;
using TNNL.Collidables;
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
            ShieldModel.InvincibilityUpdate += InvincibilityUpdateListener;
        }

        void InvincibilityUpdateListener(float invincibilityRemainingInSeconds)
        {
            if (invincibilityRemainingInSeconds == 0f)
            {
                invincibilityUI.SetActive(false);

            }
            else
            {
                invincibilityUI.SetActive(true);
                invincibilityText.text = $"{Math.Max(0, invincibilityRemainingInSeconds).ToString("0.0")}";
            }
        }

    }
}