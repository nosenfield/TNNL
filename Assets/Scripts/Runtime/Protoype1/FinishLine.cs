using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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