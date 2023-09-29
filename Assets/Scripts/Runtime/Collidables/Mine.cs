// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;

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
}