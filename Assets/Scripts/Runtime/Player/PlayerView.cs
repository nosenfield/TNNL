using nosenfield;
using TNNL.Collidables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerView : MonoBehaviour, IWarpable
    {
        public InputActionAsset Actions;
        PlayerMVC mvc;
        [SerializeField][ReadOnly] PlayerModel model; // expose the model in the inspector
        public bool DoUpdate;
        // [SerializeField] private GameObject boostAnim;

        public void SetMVC(PlayerMVC playerMVC)
        {
            mvc = playerMVC;
            model = playerMVC.Model;
        }

        public void FixedUpdate()
        {
            mvc.Controller?.FixedUpdate();

            if (mvc.Model != null && DoUpdate)
            {
                transform.position = new Vector3(mvc.Model.XPosition, mvc.Model.YPosition);
            }
        }

        public void Update()
        {
            mvc.Controller?.Update();

            if (mvc.Model != null && DoUpdate)
            {
                // boostAnim.SetActive(model.IsBoosting);
            }
        }

        void OnEnable()
        {
            Actions?.FindActionMap("Gameplay").Enable();
        }
        void OnDisable()
        {
            Actions?.FindActionMap("Gameplay").Disable();
        }

        public void Warp(WormHole warpDestination)
        {
            mvc.Controller.WarpTo(warpDestination);
        }
    }
}