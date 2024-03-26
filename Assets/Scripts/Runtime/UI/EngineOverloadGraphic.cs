using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class EngineOverloadGraphic : MonoBehaviour
    {
        [SerializeField] GameObject overloadGraphic;
        PlayerModel playerModel;

        void Awake()
        {
            PlayerMVC.SetCurrentPlayer += AssignPlayer;
            overloadGraphic.SetActive(false);
        }

        void AssignPlayer(PlayerMVC playerMVC)
        {
            playerModel = playerMVC.Model;
        }

        void Update()
        {
            if (playerModel != null)
            {
                overloadGraphic.SetActive(playerModel.IsOverloaded);
                // NOTE
                // This does not need to be set every frame, but is likely cheaper/easier than implementing an overload event
                ///
            }
        }
    }
}
