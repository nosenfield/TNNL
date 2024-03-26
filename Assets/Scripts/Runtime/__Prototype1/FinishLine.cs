using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Prototype_1
{
    public class FinishLine : MonoBehaviour
    {
        public static UnityAction FinishLineContact;
        private void OnTriggerEnter(Collider other)
        {
            Ship ship = other.GetComponent<Ship>();

            if (ship != null)
            {
                FinishLineContact?.Invoke();
            }
        }
    }
}