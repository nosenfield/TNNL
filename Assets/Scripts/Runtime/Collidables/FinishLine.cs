// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;
using UnityEngine.Events;
using TNNL.Player;

namespace TNNL.Collidables
{
    public class FinishLine : MonoBehaviour
    {
        public static UnityEvent FinishLineCollision;

        // Handle my collision with objects of different types
        public void OnTriggerEnter(Collider other)
        {
            PlayerShip player = other.GetComponentInParent<PlayerShip>();

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
            FinishLineCollision.Invoke();
        }
    }
}