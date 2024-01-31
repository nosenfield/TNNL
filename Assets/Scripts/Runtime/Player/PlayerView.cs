using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerView : MonoBehaviour
    {
        public InputActionAsset actions;
        [SerializeField] private bool GenerateController;
        PlayerController controller;
        [SerializeField] PlayerModel model;
        public bool DoUpdate;
        // [SerializeField] private GameObject boostAnim;

        void Awake()
        {
            if (GenerateController)
            {
                controller = new PlayerController(this);
            }
        }

        public void SetModel(PlayerModel model)
        {
            this.model = model;
        }

        public void FixedUpdate()
        {
            controller.FixedUpdate();

            if (model != null && DoUpdate)
            {
                transform.position = new Vector3(model.XPosition, model.YPosition);
            }
        }

        public void Update()
        {
            controller.Update();

            if (model != null && DoUpdate)
            {
                // boostAnim.SetActive(model.IsBoosting);
            }
        }

        void OnEnable()
        {
            actions.FindActionMap("Gameplay").Enable();
        }
        void OnDisable()
        {
            actions.FindActionMap("Gameplay").Disable();
        }
    }
}