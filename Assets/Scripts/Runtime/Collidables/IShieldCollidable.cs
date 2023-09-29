public interface IShieldCollidable
{
    ShieldCollisionType Type
    {
        get;
    }
}

public enum ShieldCollisionType
{
    TerrainCube,
    Mine,
    ShieldBoost
}
