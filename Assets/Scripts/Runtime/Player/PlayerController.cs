using System;
using TNNL.Collidables;
using TNNL.Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerController
    {
        public static Action PlayerShipDestroyed;
        Player.PlayerMVC mvc;

        public void SetMVC(PlayerMVC playerMVC)
        {
            // NOTE
            // We've opened up the PlayerController and LevelParser as a Singletons. That means:
            // - Listenening for static events on LevelParser could instead be a direct call from LevelParser to our PlayerController Singleton
            // - LevelParser.LevelCreated does not need to be static, it could be a member field accessible through the static Instance.LevelCreated
            ///

            mvc = playerMVC;
        }

        private void UpdatePlayerForLevel(float levelWidth)
        {
            float secondsToTravelLevelWidth = 3f;
            mvc.Model.SetXVelocity(levelWidth - mvc.View.GetComponentInChildren<ShieldView>().MaxScale, secondsToTravelLevelWidth);
            mvc.Model.SetYVelocity((levelWidth - mvc.View.GetComponentInChildren<ShieldView>().MaxScale) / secondsToTravelLevelWidth);
        }

        public void EnterTransitionState()
        {
            // this method should stop the player's oscillation and bring them to neutral (center)
            // we can have separate stopping locations for ship 1,2,3 via colliders

            mvc.Model.SetXVelocity(0f, 1f);
        }

        public void Activate()
        {
            UpdatePlayerForLevel(LevelParser.Instance.GetCurrentSection().Width);

            mvc.View.DoUpdate = true;
            mvc.Model.DoUpdate = true;

            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Press").performed += OnPress;
            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Release").performed += OnRelease;

            ShieldController.ShieldDestroyed += ShieldDestroyed;
        }

        public void FixedUpdate()
        {
            mvc.Model.FixedUpdate();
        }

        public void Update()
        {
            mvc.Model.Update();
        }

        public void Deactivate()
        {
            mvc.View.DoUpdate = false;
            mvc.Model.DoUpdate = false;

            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Press").performed -= OnPress;
            mvc.View.Actions.FindActionMap("Gameplay").FindAction("Release").performed -= OnRelease;

            ShieldController.ShieldDestroyed -= ShieldDestroyed;
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

        void ShieldDestroyed()
        {
            Deactivate();
            PlayerShipDestroyed?.Invoke();
        }
    }
}