// the base collidable behaviour that all collidable game objects can extend
using UnityEngine;

public abstract class AbstractCollidable : MonoBehaviour, IShieldCollidable
{
    public abstract ShieldCollisionType Type
    {
        get;
    }
}