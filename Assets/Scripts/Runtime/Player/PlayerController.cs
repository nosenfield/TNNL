using System.Timers;
using TNNL.Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private PlayerModel model;

        public PlayerController(PlayerView view)
        {
            this.view = view;
            model = new PlayerModel();
            view.SetModel(model);

            LevelParser.LevelCreated += UpdatePlayerForLevel;
            ShieldController.ShieldDestroyed += PlayerDestroyed;

            view.actions.FindActionMap("Gameplay").FindAction("Press").performed += OnPress;
            view.actions.FindActionMap("Gameplay").FindAction("Release").performed += OnRelease;
        }

        public void UpdatePlayerForLevel(float levelWidth)
        {
            model.SetXVelocity(levelWidth - view.GetComponentInChildren<ShieldView>().MaxScale, 3f);
            model.SetYVelocity((levelWidth - view.GetComponentInChildren<ShieldView>().MaxScale) / 3f);

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

        // Boost the ship forward
        void OnPress(InputAction.CallbackContext context)
        {
            model.BoostRequested = true;
        }

        void OnRelease(InputAction.CallbackContext context)
        {
            model.BoostRequested = false;
        }

        void PlayerDestroyed()
        {
            model.DoUpdate = false;
        }
    }
}