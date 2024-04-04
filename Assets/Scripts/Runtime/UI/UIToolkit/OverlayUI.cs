using System;
using TNNL.Level;
using TNNL.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNNL.UI.UIToolkit
{
    public class OverlayUI : MonoBehaviour
    {
        private static OverlayUI instance;
        Button startRun;
        // Button resetLevel;
        Button nextLevel;
        Button resetData;

        public static Action StartRunClick;
        // public static Action ResetLevelClicked;
        public static Action NextLevelClick;
        public static Action ResetDataClick;

        void OnEnable()
        {
            instance = this;
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            startRun = root.Q<Button>("StartRunButton");
            startRun.clicked += StartRunClickedHandler;

            // // resetLevel = root.Q<Button>("ResetLevelButton");
            // // resetLevel.clicked += ResetLevelClickedHandler;

            nextLevel = root.Q<Button>("NextLevelButton");
            nextLevel.clicked += NextLevelClickedHandler;

            resetData = root.Q<Button>("ResetDataButton");
            resetData.clicked += ResetDataClickedHandler;

        }

        void OnDisable()
        {
            Debug.Log("Remove OverlayUI event listeners");
            startRun.clicked -= StartRunClickedHandler;
            // // resetLevel.clicked -= ResetLevelClickedHandler;
            nextLevel.clicked -= NextLevelClickedHandler;
            resetData.clicked -= ResetDataClickedHandler;
        }

        void StartRunClickedHandler()
        {
            Debug.Log("StartRunClickedHandler");
            StartRunClick?.Invoke();
        }

        // void ResetLevelClickedHandler()
        // {
        //     Debug.Log("ResetLevelClickedHandler");
        //     ResetLevelClicked?.Invoke();
        // }

        void NextLevelClickedHandler()
        {
            Debug.Log("NextLevelClickedHandler");
            NextLevelClick?.Invoke();
        }

        void ResetDataClickedHandler()
        {
            Debug.Log("ResetDataClickedHandler");
            ResetDataClick?.Invoke();
        }

        public static void SetVisible(bool visible)
        {
            instance.GetComponentInParent<UIDocument>().rootVisualElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            // instance.gameObject.SetActive(visible);
        }

        public static void SetRunButtonCopy(string copy)
        {
            instance.startRun.text = $"\n{copy}\n";
        }

        public static void SetDebugState()
        {
            Debug.Log($"SetDebugState()");
            instance.startRun.visible = true;
            // instance.nextLevel.visible = true;
            instance.resetData.visible = true;
        }

        public static void SetGameplayActiveState()
        {
            Debug.Log($"SetGameplayActiveState()");
            instance.startRun.visible = true;
            // instance.nextLevel.visible = false;
            instance.resetData.visible = false;
        }

        public static void SetGameplayResetState()
        {
            Debug.Log($"SetGameplayResetState()");
            instance.startRun.visible = true;
            // instance.nextLevel.visible = true;
            instance.resetData.visible = true;
        }
    }
}