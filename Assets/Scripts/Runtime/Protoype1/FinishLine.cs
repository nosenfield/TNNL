using UnityEngine;

namespace TNNL.Prototype_1
{
    public class FinishLine : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Ship ship = other.GetComponent<Ship>();

            if (ship != null)
            {
                Main.FinishLineContact.Invoke();
            }
        }
    }
}