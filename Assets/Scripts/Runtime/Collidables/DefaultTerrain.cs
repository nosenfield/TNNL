using TNNL.Player;
using UnityEngine;

namespace TNNL.Collidables
{
    /// <summary>
    /// the TerrainCube behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. terrain types w/ 2x damage)
    /// </summary>
    public class DefaultTerrain : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.DefaultTerrain;
            }
        }
        public float Damage;

        override public int CollisionPoints
        {
            get
            {
                return 1;
            }
        }

        // Handle my collision with objects of different types
        public override void OnTriggerEnter(Collider other)
        {
            ShieldView shield = other.GetComponentInParent<ShieldView>();
            if (shield != null)
            {
                DispatchPointCollection();
                Deactivate();
            }
        }
    }
}