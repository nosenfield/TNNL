using System;
using System.Collections;
using TNNL.Collidables;
using TNNL.Events;
using TNNL.Level;
using UnityEngine;
using UnityEngine.InputSystem;
using static nosenfield.Animation.EasingFunction;

namespace TNNL.Player
{
    public class PlayerController
    {
        public static Action PlayerShipDestroyed;
        Player.PlayerMVC mvc;
        nosenfield.Logging.Logger logger = new();

        public void SetMVC(PlayerMVC playerMVC)
        {
            // NOTE
            // - Listenening for static events on LevelParser could instead be a direct call from LevelParser to our PlayerController Singleton
            // - LevelParser.LevelCreated does not need to be static, it could be a member field accessible through the static Instance.LevelCreated
            ///

            mvc = playerMVC;
        }

        public void StartShipMovement()
        {
            // set velocities
            float deltaX = LevelParser.Instance.GetCurrentSection().Width - mvc.View.GetComponentInChildren<ShieldView>().MaxScale;
            float secondsToTravelLevelWidth = 3f;
            mvc.Model.SetXVelocity(deltaX, secondsToTravelLevelWidth);
            mvc.Model.SetYVelocity(deltaX / secondsToTravelLevelWidth);

            // start ship moving forward
            mvc.Model.UpdateX = false;
            mvc.Model.UpdateY = true;
        }

        public void ActivatePlayerControls()
        {
            logger.Trace();

            // enable user input
            mvc.View.Actions?.FindActionMap("Gameplay").Enable();
            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Press").performed += OnPress;
            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Release").performed += OnRelease;

            // add event listeners
            EventAggregator.Subscribe<ShieldHealthUpdateEvent>(ShieldHealthUpdateEventListener);
            EventAggregator.Subscribe<StartingLineCollisionEvent>(StartingLineCollisionEventListener);
        }

        private void StartingLineCollisionEventListener(object e)
        {
            // Start horizontal motion
            mvc.Model.UpdateX = true;
        }

        public void DeactivatePlayerControls()
        {
            logger.Trace();

            mvc.View.Actions?.FindActionMap("Gameplay").Disable();
            mvc.Model.BoostRequested = false;
            RemoveEventListeners();
        }

        private void RemoveEventListeners()
        {
            logger.Trace();

            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Press").performed -= OnPress;
            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Release").performed -= OnRelease;
            EventAggregator.Unsubscribe<ShieldHealthUpdateEvent>(ShieldHealthUpdateEventListener);
            EventAggregator.Unsubscribe<StartingLineCollisionEvent>(StartingLineCollisionEventListener);
        }

        public void StopShipMovement()
        {
            logger.Trace();

            mvc.Model.UpdateX = false;
            mvc.Model.UpdateY = false;
        }

        public void EnterLevelCompleteTransitionState()
        {
            logger.Trace();

            mvc.Model.UpdateX = false;
            mvc.Model.SetXVelocity(0f, 1f);

            float startingX = mvc.Model.XPosition;
            float endingX = 0f;
            float duration = 1.5f;
            Function easingFunc = nosenfield.Animation.EasingFunction.EaseOutSine;

            mvc.View.StartCoroutine(Routine());
            IEnumerator Routine()
            {
                float t = 0f;
                while (t < duration)
                {
                    mvc.Model.XPosition = easingFunc(startingX, endingX, t / duration);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                mvc.Model.XPosition = easingFunc(startingX, endingX, 1f);
            }
        }

        public void FixedUpdate()
        {
            mvc.Model.FixedUpdate();
        }

        public void Update()
        {
            mvc.Model.Update();
        }

        public void WarpTo(WormHole warpDestination)
        {
            mvc.Model.WarpTo(warpDestination.gameObject.transform.position.y);
        }

        // Boost the ship forward
        void OnPress(InputAction.CallbackContext context)
        {
            mvc.Model.BoostRequested = true;
        }

        void OnRelease(InputAction.CallbackContext context)
        {
            mvc.Model.BoostRequested = false;
        }

        void ShieldHealthUpdateEventListener(object e)
        {
            ShieldHealthUpdateEvent shieldHealthUpdateEvent = (ShieldHealthUpdateEvent)e;
            if (shieldHealthUpdateEvent.PercentHealth <= 0)
            {
                PlayerShipDestroyed?.Invoke();

                mvc.View.GetComponentInChildren<ShipView>().gameObject.SetActive(false);
                mvc.Model.ClearFlags();
            }
        }

        public void Destroy()
        {
            logger.Trace();

            RemoveEventListeners();
        }
    }
}