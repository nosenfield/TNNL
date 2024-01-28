// the base collidable behaviour that all collidable game objects can extend
using UnityEngine;

namespace TNNL.Collidables
{
    public abstract class AbstractCollidable : MonoBehaviour, IShieldCollidable
    {
        public abstract ShieldCollisionType Type
        {
            get;
        }

        public abstract void OnTriggerEnter(Collider other);
    }
}