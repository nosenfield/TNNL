using UnityEngine;

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
}