using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class BoostUI : MonoBehaviour
    {
        [SerializeField] Slider slider;
        PlayerModel playerModel;

        void Awake()
        {
            PlayerController.PlayerModelCreated += AssignModel;
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
            }
        }
    }
}
