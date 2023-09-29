public interface IShieldCollidable
{
    ShieldCollisionType Type
    {
        get;
    }
}

public enum ShieldCollisionType
{
    Terrain,
    Mine,
    ShieldBoost
}
