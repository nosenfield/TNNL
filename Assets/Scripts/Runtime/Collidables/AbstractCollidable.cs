// the base collidable behaviour that all collidable game objects can extend
using System.Collections;
using UnityEngine;

namespace TNNL.Collidables
{
    public enum CollisionType
    {
        Terrain,
        Mine,
        ShieldBoost,
        FinishLine,
        WormHole,
    }

    public abstract class AbstractCollidable : MonoBehaviour
    {
        public GameObject container;
        public abstract CollisionType Type
        {
            get;
        }

        protected bool dirty;
        public virtual int CollisionPoints
        {
            get
            {
                return 0;
            }
        }

        public abstract void OnTriggerEnter(Collider other);
        public void Activate()
        {
            container.SetActive(true);
            dirty = false;
        }

        /// <summary>
        /// We wrap the deactivation in a yielded method for 1 frame so that we don't interrupt the ShieldView processing its half of the collision
        /// </summary>
        protected void Deactivate()
        {
            if (dirty) return;
            dirty = true;

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return null;
                container.SetActive(false);
            }
        }

        void OnDestroy()
        {
            GameObject.Destroy(container);
        }
    }
}