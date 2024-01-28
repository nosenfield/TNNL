using UnityEngine;

namespace TNNL.Collidables
{
    public interface IShieldCollidable
    {
        ShieldCollisionType Type
        {
            get;
        }

        void OnTriggerEnter(Collider other);
    }

    public enum ShieldCollisionType
    {
        Terrain,
        Mine,
        ShieldBoost,
        FinishLine
    }

}