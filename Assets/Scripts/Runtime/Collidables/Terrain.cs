using System.Collections;
using TNNL.Player;
using UnityEngine;

namespace TNNL.Collidables
{
    /// <summary>
    /// the TerrainCube behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. terrain types w/ 2x damage)
    /// </summary>
    public class Terrain : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.Terrain;
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
    }
}