using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static UnityAction GameOverAction;
    public static UnityAction FinishLineContact;
    [SerializeField] GameObject gameoverScreen;
    [SerializeField] GameObject successScreen;
    [SerializeField] Player player;
    [SerializeField] StatusText statusText;
    [SerializeField] LevelCubeGenerator levelCubeGenerator;

    void Awake()
    {
        GameOverAction += GameOverListener;
        FinishLineContact += FinishLineContactListener;

        gameoverScreen.SetActive(false);
    }

    public void StartRunOnClick()
    {
        gameoverScreen.SetActive(false);
        successScreen.SetActive(false);

        statusText.IncreaseAttempts();
        player.ResetShip();
        player.StartRun();
    }

    public void ResetLevelOnClick()
    {
        levelCubeGenerator.ResetLevel();
        statusText.Reset();
    }

    public void GameOverListener()
    {
        gameoverScreen.SetActive(true);
    }

    public void FinishLineContactListener()
    {
        Debug.Log("FinishLineContactListener");
        successScreen.SetActive(true);
        statusText.IncreaseWinCount();
        player.DoUpdateShip = false;
    }
}