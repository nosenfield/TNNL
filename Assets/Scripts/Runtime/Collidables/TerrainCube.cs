// the TerrainCube behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. terrain types w/ 2x damage)
public class TerrainCube : IShieldCollidable
{
    public ShieldCollisionType Type
    {
        get
        {
            return ShieldCollisionType.TerrainCube;
        }
    }
    public float Damage;
}