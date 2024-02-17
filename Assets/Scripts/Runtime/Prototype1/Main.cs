using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Prototype_1
{
    public class Main : MonoBehaviour
    {
        public static UnityAction GameOverAction;
        [SerializeField] GameObject gameoverScreen;
        [SerializeField] GameObject successScreen;
        [SerializeField] Player player;
        [SerializeField] StatusText statusText;
        [SerializeField] LevelCubeGenerator levelCubeGenerator;

        void Awake()
        {
            GameOverAction += GameOverListener;
            FinishLine.FinishLineContact += FinishLineContactListener;

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
}