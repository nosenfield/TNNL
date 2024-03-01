using System.Collections;
using TNNL.Player;
using UnityEngine;

namespace TNNL.Collidables
{
    public class ShieldBoost : AbstractCollidable
    {

        public override CollisionType Type
        {
            get
            {
                return CollisionType.ShieldBoost;
            }
        }

        new public float CollisionPoints = 1000;

        public float Amount;

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShieldView shield = other.GetComponentInParent<ShieldView>();
            switch (shield)
            {
                case null:
                    break;
                default:
                    Deactivate();
                    break;
            }
        }
    }
}