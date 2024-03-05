using System;
using TNNL.Collidables;
using TNNL.Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerController
    {
        public static PlayerController Instance;
        public static Action<PlayerModel> PlayerModelCreated;
        public static Action PlayerShipDestroyed;
        private PlayerView view;
        private PlayerModel model;

        public PlayerController(PlayerView view)
        {
            this.view = view;
            model = new PlayerModel();
            view.SetModel(model);

            // NOTE
            // We've opened up the PlayerController and LevelParser as a Singletons. That means:
            // - Listenening for static events on LevelParser could instead be a direct call from LevelParser to our PlayerController Singleton
            // - LevelParser.LevelCreated does not need to be static, it could be a member field accessible through the static Instance.LevelCreated
            ///

            LevelParser.LevelCreated += UpdatePlayerForLevel;
            ShieldController.ShieldDestroyed += ShieldDestroyed;

            view.actions.FindActionMap("Gameplay").FindAction("Press").performed += OnPress;
            view.actions.FindActionMap("Gameplay").FindAction("Release").performed += OnRelease;

            PlayerModelCreated?.Invoke(model);

            Instance = this;
        }

        public void ResetPlayer()
        {
            model.SetDefaults();
            ActivatePlayer();
        }
        
        private void UpdatePlayerForLevel(float levelWidth)
        {
            float secondsToTravelLevelWidth = 3f;
            model.SetXVelocity(levelWidth - view.GetComponentInChildren<ShieldView>().MaxScale, secondsToTravelLevelWidth);
            model.SetYVelocity((levelWidth - view.GetComponentInChildren<ShieldView>().MaxScale) / secondsToTravelLevelWidth);
        }

        public void ActivatePlayer()
        {
            view.DoUpdate = true;
            model.DoUpdate = true;
        }

        public void FixedUpdate()
        {
            model.FixedUpdate();
        }

        public void Update()
        {
            model.Update();
        }

        public void PausePlayer()
        {
            view.DoUpdate = false;
            model.DoUpdate = false;
        }

        public void WarpTo(WormHole warpDestination)
        {
            Debug.Log("PlayerController.Warp()");
            model.WarpTo(warpDestination.gameObject.transform.position.y);
        }

        // Boost the ship forward
        void OnPress(InputAction.CallbackContext context)
        {
            Debug.Log("OnPress");
            model.BoostRequested = true;
        }

        void OnRelease(InputAction.CallbackContext context)
        {
            Debug.Log("OnRelease");
            model.BoostRequested = false;
        }

        void ShieldDestroyed()
        {
            model.DoUpdate = false;
            PlayerShipDestroyed?.Invoke();
        }
    }
}