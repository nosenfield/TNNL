// the TerrainCube behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. terrain types w/ 2x damage)
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
}