// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;
using UnityEngine.Events;
using TNNL.Player;
using nosenfield.Logging;
using System;

namespace TNNL.Collidables
{
    public class FinishLine : MonoBehaviour
    {
        public static event Action FinishLineCollision;

        // Handle my collision with objects of different types
        public void OnTriggerEnter(Collider other)
        {
            PlayerView player = other.GetComponentInParent<PlayerView>();

            switch (player)
            {
                case null:
                    break;
                default:
                    ReportSectionComplete();
                    break;
            }
        }

        private void ReportSectionComplete()
        {
            DefaultLogger.Instance.Log(LogLevel.DEBUG, "Player collided with finish line");

            FinishLineCollision?.Invoke();
        }
    }
}