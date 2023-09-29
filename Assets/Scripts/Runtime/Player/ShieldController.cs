using UnityEngine;
using UnityEngine.Events;

public class ShieldController
{
    private ShieldModel model;
    private ShieldView view;
    public ShieldController(ShieldView view)
    {
        this.view = view;
        model = new ShieldModel(ShieldModel.DefaultShieldStartingHealth, ShieldModel.DefaultShieldMaxHealth);
        view.SetModel(model);

        view.ShieldCollision.AddListener(CollisionListener);
    }

    private void CollisionListener(IShieldCollidable collidable)
    {
        switch (collidable.Type)
        {
            case ShieldCollisionType.Terrain:
                model.DamageShield(((Terrain)collidable).Damage);
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