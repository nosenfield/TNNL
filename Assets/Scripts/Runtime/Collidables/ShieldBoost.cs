// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using UnityEngine;

public class ShieldBoost : MonoBehaviour, IShieldCollidable
{

    public ShieldCollisionType Type
    {
        get
        {
            return ShieldCollisionType.ShieldBoost;
        }
    }

    public float Amount;
}