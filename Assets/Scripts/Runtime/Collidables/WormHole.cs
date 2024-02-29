// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;
using UnityEngine.Events;
using TNNL.Player;
using nosenfield.Logging;
using System;

namespace TNNL.Collidables
{
    public class WormHole : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.WormHole;
            }
        }

        public static event Action WormHoleCollision;

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                ReportCollision();
            }
        }

        private void ReportCollision()
        {
            DefaultLogger.Instance.Log(LogLevel.DEBUG, "Player collided with worm hole");

            WormHoleCollision?.Invoke();
        }
    }
}