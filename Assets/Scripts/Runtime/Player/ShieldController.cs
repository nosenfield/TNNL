using TNNL.Collidables;
using UnityEngine;

namespace TNNL.Player
{
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

            Debug.Log(model);
            Debug.Log(model.HealthUpdate);

            model.HealthUpdate.AddListener(HealthUpdateListener);
        }

        private void CollisionListener(IShieldCollidable collidable)
        {
            switch (collidable.Type)
            {
                case ShieldCollisionType.Terrain:
                    model.DamageShield(((Collidables.Terrain)collidable).Damage);
                    break;
                case ShieldCollisionType.Mine:
                    Debug.Log("Hit a mine!");
                    model.DamageShield(((Mine)collidable).Damage);
                    break;
                case ShieldCollisionType.ShieldBoost:
                    Debug.Log("Grabbed a shield!");
                    model.HealShield(((ShieldBoost)collidable).Amount);
                    break;
            }
        }

        private void HealthUpdateListener(float percentHealth)
        {
            Debug.Log("HealthUpdateListener");
            Debug.Log($"Current Health: {percentHealth}");
            Debug.Log($"model.PercentHealth: {model.PercentHealth}");

            if (percentHealth <= 0)
            {
                Debug.Log("Shield is destroyed!");
            }
            else
            {

            }
        }
    }
}