using System;
using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Player
{
    [Serializable]
    public class PlayerModel
    {
        public static float Direction = -1f;
        PlayerMVC mvc;
        public bool UpdateX;
        public bool UpdateY;
        public bool BoostRequested;
        public float XPosition;
        public float YPosition;
        [SerializeField] private bool isBoosting;

        [SerializeField] private float timeInPeriod;
        [SerializeField] private float period; // the time it takes to complete one left/right cycle

        [SerializeField] private float xDisplacement; // the total width of our level
        [SerializeField] private float xVelocity; // the default X/Y unit change per second 
        [SerializeField] private float yVelocity; // the default X/Y unit change per second 

        [SerializeField] private float boostDistanceMultiplier; // the Y multiplier while boosting
        [SerializeField] private float boostTimeOnContact; // the time in seconds to keep boost active on first contact
        [SerializeField] private float boostTime; // the current time since the player started boosting
        public float BoostTime { get { return boostTime; } }

        [SerializeField] private float boostAllowThreshold; // the value the boost must return to after entering recovery before allowing another boost
        [SerializeField] private float boostMaxTime; // the maximum boost time before "overload"
        public float BoostMaxTime { get { return boostMaxTime; } }

        [SerializeField] private float recoveryRateMultiplier; // the time multiplier for boost recovery
        [SerializeField] private float overloadRecoveryThreshold; // the minimum value needed to re-enable boost after overload

        [SerializeField] private bool isOverloaded;
        public bool IsOverloaded
        {
            get
            {
                return isOverloaded;
            }
        }
        [SerializeField] private float overloadPenalty; // the penalty time added to the standard recovery time for going into overload

        public PlayerModel()
        {
            SetDefaults();
        }

        /// <summary>
        /// Currently the model does not need MVC access. This is just here for consistency across the MVC
        /// </summary>
        /// <param name="playerMVC"></param>
        public void SetMVC(PlayerMVC playerMVC)
        {
            mvc = playerMVC;
        }

        public void SetDefaults()
        {
            isBoosting = false;
            BoostRequested = false;

            timeInPeriod = 0f;

            boostDistanceMultiplier = 2f; // we double our distance while boosting

            boostTime = 0f;
            boostAllowThreshold = 0f;
            boostTimeOnContact = .5f;
            boostMaxTime = 1f;
            recoveryRateMultiplier = 2f; //  we recover from a boost in half the time it took to boost

            overloadPenalty = 1f;
            overloadRecoveryThreshold = 0f;

            XPosition = 0f;
            YPosition = 0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displacement">The x movement of the ship. Should be based on the width of the level (minus some buffer amount)</param>
        /// <param name="seconds">The number of seconds to move this level width (half a period)</param>
        public void SetXVelocity(float displacement, float seconds)
        {
            xDisplacement = displacement;
            xVelocity = displacement / seconds;
            period = seconds * 2f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displacement">The default y movement of the ship per second</param>
        public void SetYVelocity(float displacementPerSecond)
        {
            yVelocity = displacementPerSecond;
        }

        public void Update()
        {
            boostTime = CalculateBoostTime();
        }

        public void FixedUpdate()
        {
            if (!isOverloaded && boostTime > boostMaxTime)
            {
                Debug.Log($"Boost overloaded! {Time.time}");
                isOverloaded = true;
                isBoosting = false;
                AddOverloadPenalty();
            }
            else if (isOverloaded && boostTime <= overloadRecoveryThreshold)
            {
                Debug.Log($"Overload recovery complete {Time.time}");
                isOverloaded = false;
            }

            if (!isOverloaded)
            {
                if (BoostRequested && boostTime <= boostAllowThreshold)
                {
                    isBoosting = true;
                }
                else if (!BoostRequested && boostTime >= boostTimeOnContact)
                {
                    isBoosting = false;
                }
            }

            if (UpdateX && !isBoosting)
            {
                timeInPeriod += Time.fixedDeltaTime;
                if (timeInPeriod > period)
                {
                    timeInPeriod -= period;
                }

                float percentComplete = timeInPeriod / period;
                XPosition = CalculateXPosition(percentComplete);
            }

            if (UpdateY)
            {
                YPosition = isBoosting ? CalculateBoostedYPosition() : CalculateYPosition();
            }
        }

        private float CalculateXPosition(float t)
        {
            // t == the percetange of time into the current period
            float percentDisplaced = Mathf.Sin(Mathf.PI * 2 * t);
            return percentDisplaced * xDisplacement / 2;
        }

        private float CalculateYPosition()
        {
            return YPosition += yVelocity * Time.fixedDeltaTime * Direction;
        }

        private float CalculateBoostedYPosition()
        {
            return YPosition += yVelocity * boostDistanceMultiplier * Time.fixedDeltaTime * Direction;
        }

        private float CalculateBoostTime()
        {
            if (isBoosting)
            {
                return boostTime + Time.deltaTime;
            }
            else
            {
                return Mathf.Max(0f, boostTime - Time.deltaTime * recoveryRateMultiplier);
            }
        }

        private void AddOverloadPenalty()
        {
            boostTime = boostMaxTime + overloadPenalty;
        }

        public void WarpTo(float _yPosition)
        {
            YPosition = _yPosition;
        }

        public void AdjustY(float amount)
        {
            YPosition += amount;
        }

        public void ClearFlags()
        {
            isBoosting = false;
            isOverloaded = false;
        }
    }
}