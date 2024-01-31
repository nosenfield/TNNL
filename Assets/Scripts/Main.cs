using nosenfield.Logging;
using TNNL.Collidables;
using UnityEngine;

namespace TNNL
{
    public class Main : MonoBehaviour
    {
        void Start()
        {
            StartLevel();
        }

        void StartLevel()
        {
            FinishLine.FinishLineCollision += FinishLineCollisionListener;
        }

        void FinishLineCollisionListener()
        {
            DefaultLogger.Instance.Log(LogLevel.DEBUG, "Player collided with finish line");
        }
    }
}