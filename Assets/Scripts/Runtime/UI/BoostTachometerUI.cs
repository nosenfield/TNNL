using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class BoostTachometerUI : MonoBehaviour
    {
        [SerializeField] GameObject overloadGraphic;
        [SerializeField] GameObject meterFill;
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
                meterFill.transform.rotation = Quaternion.AngleAxis(playerModel.BoostTime / playerModel.BoostMaxTime * 180f, Vector3.back) * Quaternion.AngleAxis(180f, Vector3.left);

                // NOTE
                // This does not need to be set every frame, but is likely cheaper/easier than implementing an overload event
                ///
                overloadGraphic.SetActive(playerModel.IsOverloaded);
            }
        }
    }
}
