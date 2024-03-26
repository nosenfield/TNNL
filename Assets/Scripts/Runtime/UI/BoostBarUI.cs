using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class BoostBarUI : MonoBehaviour
    {
        [SerializeField] GameObject overloadGraphic;
        [SerializeField] Slider slider;
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
                slider.value = playerModel.BoostTime / playerModel.BoostMaxTime;

                // NOTE
                // This does not need to be set every frame, but is likely cheaper/easier than implementing an overload event
                ///
                overloadGraphic.SetActive(playerModel.IsOverloaded);
            }
        }
    }
}
