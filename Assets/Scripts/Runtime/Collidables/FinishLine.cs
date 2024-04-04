using UnityEngine;
using TNNL.Player;
using TNNL.Events;
using TNNL.Level;

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

        protected override int CollisionPoints
        {
            get
            {
                return 10000;
            }
        }

        // Handle my collision with objects of different types
        protected override void OnTriggerEnter(Collider other)
        {
            ShipView shipView = other.GetComponentInParent<ShipView>();

            if (shipView != null)
            {
                DispatchPointCollection();
                LevelCheckpointEvent.Dispatch(LevelParser.Instance.GetCurrentSection());
            }
        }
    }
}