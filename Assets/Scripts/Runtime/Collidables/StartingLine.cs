using UnityEngine;
using TNNL.Player;
using TNNL.Events;

namespace TNNL.Collidables
{
    public class StartingLine : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.StartingLine;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                StartingLineCollisionEvent.Dispatch();
            }
        }
    }
}