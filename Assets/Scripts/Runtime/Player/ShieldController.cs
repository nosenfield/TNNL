using System;
using System.Collections;
using nosenfield.Logging;
using TNNL.Collidables;
using UnityEngine;

namespace TNNL.Player
{
    public class ShieldController
    {
        private nosenfield.Logging.Logger logger = new();
        private ShieldMVC mvc;

        public void SetMVC(ShieldMVC shieldMVC)
        {
            mvc = shieldMVC;
        }

        public void FixedUpdate()
        {
            if (mvc.Model.SecondsInvincibleRemaining > 0f)
            {
                mvc.Model.SecondsInvincibleRemaining = Mathf.Max(0f, mvc.Model.SecondsInvincibleRemaining - Time.fixedDeltaTime);
            }
        }

        public void ResetShield()
        {
            mvc.View.StartCoroutine(Routine());
            IEnumerator Routine()
            {
                while (mvc.Model.PercentHealth < 1)
                {
                    mvc.Model.HealShield(1);
                    yield return new WaitForEndOfFrame();
                }

                mvc.Model.ResetShield();
            }
        }

        public void HandleCollision(AbstractCollidable collidable)
        {
            if (collidable.ExternalDirty)
            {
                logger.Log(LogLevel.DEBUG, "Collidable already processed. Skipping");
                return;
            }

            switch (collidable.Type)
            {
                case CollisionType.DefaultTerrain:
                    mvc.Model.DamageShield(((Collidables.DefaultTerrain)collidable).Damage);
                    collidable.ExternalDirty = true;
                    break;
                case CollisionType.Mine:
                    logger.Log(LogLevel.DEBUG, "Hit a mine!");
                    mvc.Model.DamageShield(((Mine)collidable).Damage);
                    collidable.ExternalDirty = true;
                    break;
                case CollisionType.ShieldBoost:
                    logger.Log(LogLevel.DEBUG, "Grabbed a shield!");
                    mvc.Model.HealShield(((ShieldBoost)collidable).Amount);
                    collidable.ExternalDirty = true;
                    break;
                case CollisionType.ElectricGate:
                    logger.Log(LogLevel.DEBUG, "Hit electric gate!");
                    ((ElectricGate)collidable).SetModelToDamage(mvc.Model);
                    collidable.ExternalDirty = true;
                    break;
                case CollisionType.Invincibility:
                    logger.Log(LogLevel.DEBUG, "Grabbed invincibility!");
                    mvc.Model.SecondsInvincibleRemaining = ((Invincibility)collidable).DurationInSeconds;
                    collidable.ExternalDirty = true;
                    break;
                default:
                    logger.Log(LogLevel.DEBUG, "Unspecified shield collision!");
                    return;
            }
        }
    }
}