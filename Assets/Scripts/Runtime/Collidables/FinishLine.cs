using UnityEngine;
using UnityEngine.Events;
using TNNL.Player;
using nosenfield.Logging;
using System;

namespace TNNL.Collidables
{
    public class FinishLine : AbstractCollidable
    {
        public static event Action FinishLineCollision;

        public override CollisionType Type
        {
            get
            {
                return CollisionType.FinishLine;
            }
        }

        public override int CollisionPoints
        {
            get
            {
                return 10000;
            }
        }

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                FinishLineCollision?.Invoke();
            }
        }
    }
}