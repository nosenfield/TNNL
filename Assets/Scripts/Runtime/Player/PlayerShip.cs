using UnityEngine;

namespace TNNL.Player
{
    public class PlayerShip : MonoBehaviour
    {
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            instance = this;
        }
    }
}