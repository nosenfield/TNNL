using UnityEngine;
using UnityEngine.Events;
using TNNL.Player;
using nosenfield.Logging;
using System;

namespace TNNL.Collidables
{
    public class FinishLine : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.FinishLine;
            }
        }

        public static event Action FinishLineCollision;

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                ReportSectionComplete();
            }
        }

        private void ReportSectionComplete()
        {
            DefaultLogger.Instance.Log(LogLevel.DEBUG, "Player collided with finish line");

            FinishLineCollision?.Invoke();
        }
    }
}