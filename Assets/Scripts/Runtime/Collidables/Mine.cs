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
        public override ShieldCollisionType Type
        {
            get
            {
                return ShieldCollisionType.Mine;
            }
        }
        public float Damage;

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

        /// <summary>
        /// We wrap the deactivation in a yielded method for 1 frame so that we don't interrupt the ShieldView processing its half of the collision
        /// </summary>
        private void Deactivate()
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return null;
                container.SetActive(false);
            }
        }

        public override void Activate()
        {
            container.SetActive(true);
        }
    }
}