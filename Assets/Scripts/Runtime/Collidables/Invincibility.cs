using TNNL.Player;
using UnityEngine;

namespace TNNL.Collidables
{
    public class Invincibility : AbstractCollidable
    {

        public override CollisionType Type
        {
            get
            {
                return CollisionType.Invincibility;
            }
        }

        public override int CollisionPoints
        {
            get
            {
                return 2000;
            }
        }

        public float DurationInSeconds;

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