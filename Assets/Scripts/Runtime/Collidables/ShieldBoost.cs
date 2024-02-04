// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using System.Collections;
using TNNL.Player;
using UnityEngine;

namespace TNNL.Collidables
{
    public class ShieldBoost : AbstractCollidable
    {

        public override ShieldCollisionType Type
        {
            get
            {
                return ShieldCollisionType.ShieldBoost;
            }
        }

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