using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class BoostUI : MonoBehaviour
    {
        [SerializeField] GameObject overloadGraphic;
        [SerializeField] Slider slider;
        PlayerModel playerModel;

        void Awake()
        {
            PlayerController.PlayerModelCreated += AssignModel;
            overloadGraphic.SetActive(false);
        }

        void AssignModel(PlayerModel model)
        {
            playerModel = model;
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
