// the base collidable behaviour that all collidable game objects can extend
using System.Collections;
using TNNL.Events;
using UnityEngine;

namespace TNNL.Collidables
{
    public enum CollisionType
    {
        StartingLine,
        FinishLine,
        StoppingLine,
        DefaultTerrain,
        Mine,
        ShieldBoost,
        WormHole,
        ElectricGate,
        Invincibility,
    }

    public abstract class AbstractCollidable : MonoBehaviour
    {
        [SerializeField] protected GameObject container;
        public abstract CollisionType Type
        {
            get;
        }

        protected bool internalDirty;
        public bool ExternalDirty;

        protected virtual int CollisionPoints
        {
            get
            {
                return 0;
            }
        }

        protected abstract void OnTriggerEnter(Collider other);
        public void Activate()
        {
            container.SetActive(true);
            internalDirty = false;
            ExternalDirty = false;
        }

        /// <summary>
        /// We wrap the deactivation in a yielded method for 1 frame so that we don't interrupt the ShieldView processing its half of the collision
        /// </summary>
        protected void Deactivate()
        {
            if (internalDirty) return;
            internalDirty = true;

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

        protected void DispatchPointCollection()
        {
            if (internalDirty) return;

            PointCollectionEvent.Dispatch(CollisionPoints, this);
        }
    }
}