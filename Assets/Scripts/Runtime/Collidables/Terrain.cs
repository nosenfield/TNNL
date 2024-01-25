// the TerrainCube behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. terrain types w/ 2x damage)
using System.Collections;
using UnityEngine;

public class Terrain : AbstractCollidable
{
    public override ShieldCollisionType Type
    {
        get
        {
            return ShieldCollisionType.Terrain;
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
            gameObject.SetActive(false);
        }
    }
}