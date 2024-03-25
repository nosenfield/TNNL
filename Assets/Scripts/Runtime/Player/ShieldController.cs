using System;
using nosenfield.Logging;
using TNNL.Collidables;
using TNNL.Events;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL.Player
{
    public class ShieldController
    {
        private nosenfield.Logging.Logger logger = new();
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
                    logger.Log(LogLevel.DEBUG, "Hit a mine!");
                    model.DamageShield(((Mine)collidable).Damage);
                    break;
                case CollisionType.ShieldBoost:
                    logger.Log(LogLevel.DEBUG, "Grabbed a shield!");
                    model.HealShield(((ShieldBoost)collidable).Amount);
                    break;
                case CollisionType.ElectricGate:
                    logger.Log(LogLevel.DEBUG, "Hit electric gate!");
                    ((ElectricGate)collidable).SetModelToDamage(model);
                    break;
                case CollisionType.Invincibility:
                    logger.Log(LogLevel.DEBUG, "Grabbed invincibility!");
                    model.SecondsInvincibleRemaining = ((Invincibility)collidable).DurationInSeconds;
                    break;
                default:
                    logger.Log(LogLevel.DEBUG, "Unspecified shield collision!");
                    return;
            }
        }

        private void HealthUpdateListener(float percentHealth)
        {
            if (percentHealth <= 0)
            {
                logger.Log(LogLevel.DEBUG, "Shield is destroyed!");
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