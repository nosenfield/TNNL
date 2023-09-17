using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] GameObject gameoverScreen;
    [SerializeField] Player player;
    [SerializeField] StatusText statusText;
    [SerializeField] LevelCubeGenerator levelCubeGenerator;

    void Awake()
    {
        player.GameOverAction += GameOverListener;
    }

    public void StartRunOnClick()
    {
        statusText.IncreaseAttempts();
        gameoverScreen.SetActive(false);
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
}