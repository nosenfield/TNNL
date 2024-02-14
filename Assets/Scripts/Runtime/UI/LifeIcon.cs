using TNNL.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TNNL.UI
{
    public class LifeIcon : MonoBehaviour
    {
        [SerializeField] GameObject deathIcon;
        public void SetDeathIconActive(bool active)
        {
            deathIcon.SetActive(active);
        }
    }
}
