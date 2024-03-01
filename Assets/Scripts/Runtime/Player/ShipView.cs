using System;
using TNNL.Collidables;
using UnityEngine;

namespace TNNL.Player
{

    /// <summary>
    /// Placed on the Ship game object to separate it from the Shield game object to differentiate collision types
    /// </summary>
    public class ShipView : MonoBehaviour
    {
        public Action<AbstractCollidable> ShipCollision;

        // mvc 
        [SerializeField] private bool GenerateController;
        ShipController controller;

        void Awake()
        {
            if (GenerateController)
            {
                controller = new ShipController(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            AbstractCollidable collidable = other.GetComponentInParent<AbstractCollidable>();

            switch (collidable)
            {
                case null:
                    break;
                default:
                    ShipCollision?.Invoke(collidable);
                    break;
            }

            // parse/report the collision by some kind of type identifier (what did we hit)
        }
    }
}