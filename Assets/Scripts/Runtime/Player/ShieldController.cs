using UnityEngine;
using UnityEngine.Events;

public class ShieldController
{
    private ShieldModel model;
    private ShieldView view;
    public ShieldController(ShieldView view)
    {
        this.view = view;
        model = new ShieldModel();
        view.SetModel(model);

        view.ShieldCollision.AddListener(CollisionListener);
    }

    private void CollisionListener(IShieldCollidable collidable)
    {
        switch (collidable.Type)
        {
            case ShieldCollisionType.TerrainCube:
                model.DamageShield(((TerrainCube)collidable).Damage);

                break;
            case ShieldCollisionType.Mine:
                model.DamageShield(((Mine)collidable).Damage);
                break;
            case ShieldCollisionType.ShieldBoost:
                model.HealShield(((ShieldBoost)collidable).Amount);
                break;
        }
    }
}