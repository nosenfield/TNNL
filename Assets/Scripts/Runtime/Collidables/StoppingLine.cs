using UnityEngine;
using TNNL.Player;
using TNNL.Events;
using System;
using nosenfield.Logging;

namespace TNNL.Collidables
{
    public class StoppingLine : AbstractCollidable
    {
        public static Action Collision;
        public override CollisionType Type
        {
            get
            {
                return CollisionType.StoppingLine;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"Stopping line collision with ship");
                Collision?.Invoke();
                // StoppingLineCollisionEvent.Dispatch();
            }
        }
    }
}