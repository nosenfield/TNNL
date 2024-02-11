using System;
using TNNL.Collidables;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL.Player
{
    public class ShieldController
    {
        public static Action<AbstractCollidable> ShieldCollision;
        public static Action ShieldDestroyed;
        private ShieldModel model;
        private ShieldView view;

        public ShieldController(ShieldView view)
        {
            this.view = view;
            model = new ShieldModel(ShieldModel.DefaultShieldStartingHealth, ShieldModel.DefaultShieldMaxHealth);
            view.SetModel(model);

            view.ShieldCollision += CollisionListener;
            model.HealthUpdate += HealthUpdateListener;

            OverlayUI.ResetShipClicked += ResetShield;
        }

        private void ResetShield()
        {
            model.ResetShield();
        }

        private void CollisionListener(AbstractCollidable collidable)
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

            ShieldCollision.Invoke(collidable);
        }

        private void HealthUpdateListener(float percentHealth)
        {
            if (percentHealth <= 0)
            {
                Debug.Log("Shield is destroyed!");
                ShieldDestroyed.Invoke();
            }
            else
            {

            }
        }

        // NOTE
        // This is not yet called by our view object to which it is tied.
        // We could attach to another view which is instantiated without the createController attribute
        ///
        public void OnDestroy()
        {
            view.ShieldCollision -= CollisionListener;
            model.HealthUpdate -= HealthUpdateListener;
        }
    }
}