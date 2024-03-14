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
            ShieldModel.HealthUpdate += HealthUpdateListener;

            OverlayUI.StartRunClicked += ResetShield;
        }

        public void FixedUpdate()
        {
            if (model.SecondsInvincibleRemaining > 0f)
            {
                model.SecondsInvincibleRemaining = Mathf.Max(0f, model.SecondsInvincibleRemaining - Time.fixedDeltaTime);
            }
        }

        private void ResetShield()
        {
            model.ResetShield();
        }

        private void CollisionListener(AbstractCollidable collidable)
        {
            switch (collidable.Type)
            {
                case CollisionType.DefaultTerrain:
                    model.DamageShield(((Collidables.DefaultTerrain)collidable).Damage);
                    break;
                case CollisionType.Mine:
                    Debug.Log("Hit a mine!");
                    model.DamageShield(((Mine)collidable).Damage);
                    break;
                case CollisionType.ShieldBoost:
                    Debug.Log("Grabbed a shield!");
                    model.HealShield(((ShieldBoost)collidable).Amount);
                    break;
                case CollisionType.ElectricGate:
                    Debug.Log("Hit electric gate!");
                    ((ElectricGate)collidable).SetModelToDamage(model);
                    break;
                case CollisionType.Invincibility:
                    Debug.Log("Grabbed invincibility!");
                    model.SecondsInvincibleRemaining = ((Invincibility)collidable).DurationInSeconds;
                    break;
                default:
                    Debug.Log("Unspecified shield collision!");
                    return;
            }

            ShieldCollision?.Invoke(collidable);
        }

        private void HealthUpdateListener(float percentHealth)
        {
            if (percentHealth <= 0)
            {
                Debug.Log("Shield is destroyed!");
                ShieldDestroyed?.Invoke();
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
            ShieldModel.HealthUpdate -= HealthUpdateListener;
        }
    }
}