using System;
using TNNL.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNNL.UI.UIToolkit
{
    public class OverlayUI : MonoBehaviour
    {
        Button resetShip;
        Button resetLevel;

        public static Action StartRunClicked;
        public static Action ResetLevelClicked;

        void OnEnable()
        {
            VisualElement root = GetComponentInParent<UIDocument>().rootVisualElement;
            resetShip = root.Q<Button>("StartRunButton");
            resetLevel = root.Q<Button>("ResetLevelButton");

            resetShip.clicked += ResetShipClickedHandler;
            resetLevel.clicked += ResetLevelClickedHandler;
        }

        void OnDisable()
        {
            Debug.Log("Remove OverlayUI event listeners");

            resetShip.clicked -= ResetShipClickedHandler;
            resetLevel.clicked -= ResetLevelClickedHandler;
        }

        void ResetShipClickedHandler()
        {
            Debug.Log("ResetShipClickedHandler");
            StartRunClicked?.Invoke();
        }

        void ResetLevelClickedHandler()
        {
            Debug.Log("ResetLevelClickedHandler");
            ResetLevelClicked?.Invoke();
        }

    }
}