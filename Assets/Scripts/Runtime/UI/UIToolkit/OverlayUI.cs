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
        Button nextLevel;

        public static Action StartRunClicked;
        public static Action ResetLevelClicked;
        public static Action NextLevelClicked;

        void OnEnable()
        {
            VisualElement root = GetComponentInParent<UIDocument>().rootVisualElement;
            resetShip = root.Q<Button>("StartRunButton");
            resetLevel = root.Q<Button>("ResetLevelButton");
            nextLevel = root.Q<Button>("NextLevelButton");

            resetShip.clicked += ResetShipClickedHandler;
            resetLevel.clicked += ResetLevelClickedHandler;
            nextLevel.clicked += NextLevelClickedHandler;
        }

        void OnDisable()
        {
            Debug.Log("Remove OverlayUI event listeners");

            resetShip.clicked -= ResetShipClickedHandler;
            resetLevel.clicked -= ResetLevelClickedHandler;
            nextLevel.clicked -= NextLevelClickedHandler;
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

        void NextLevelClickedHandler()
        {
            Debug.Log("NextLevelClickedHandler");
            NextLevelClicked?.Invoke();
        }

    }
}