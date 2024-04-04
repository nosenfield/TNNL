using nosenfield;
using Sirenix.OdinInspector;
using TNNL.Collidables;
using TNNL.RuntimeData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TNNL.Player
{
    public class PlayerView : MonoBehaviour, IWarpable
    {
        public InputActionAsset Actions;
        PlayerMVC mvc;
        [SerializeField][ReadOnly] PlayerModel model; // expose the model in the inspector
        [SerializeField][ReadOnly] AttemptData attemptData; // expose the model in the inspector
        // [SerializeField] private GameObject boostAnim;

        public void SetMVC(PlayerMVC playerMVC)
        {
            mvc = playerMVC;
            model = playerMVC.Model;
            attemptData = playerMVC.AttemptData;
        }

        public void FixedUpdate()
        {
            mvc.Controller?.FixedUpdate();

            if (mvc.Model != null)
            {
                transform.position = new Vector3(mvc.Model.XPosition, mvc.Model.YPosition);
            }
        }

        public void Update()
        {
            mvc.Controller?.Update();

            if (mvc.Model != null)
            {
                // boostAnim.SetActive(model.IsBoosting);
            }
        }

        public void Warp(WormHole warpDestination)
        {
            mvc.Controller.WarpTo(warpDestination);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}