using System.Collections;
using TNNL.Player;
using UnityEngine;


namespace TNNL.Collidables
{
    /// <summary>
    /// The Mine Behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
    /// A SuperMine would be a secondary prefab that had a Mine behaviour and higher damage.
    /// In order to record this we either need to store the entire model (type + damage)
    /// Or we need to create a new type for the Supermine
    /// </summary>
    public class Mine : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.Mine;
            }
        }
        public float Damage;

        public override int CollisionPoints
        {
            get
            {
                return -500;
            }
        }

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShieldView shield = other.GetComponentInParent<ShieldView>();
            if (shield != null)
            {
                Deactivate();
            }
        }
    }
}