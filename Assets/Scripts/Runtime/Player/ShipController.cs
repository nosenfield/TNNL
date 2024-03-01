using System;
using TNNL.Collidables;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL.Player
{
    public class ShipController
    {
        public static Action<AbstractCollidable> ShipCollision;
        private ShipView view;

        public ShipController(ShipView view)
        {
            this.view = view;
            view.ShipCollision += CollisionListener;
        }

        private void CollisionListener(AbstractCollidable collidable)
        {
            switch (collidable.Type)
            {
                case CollisionType.FinishLine:
                    Debug.Log("Crossed the finish line!");
                    break;
            }

            ShipCollision?.Invoke(collidable);
        }

        // NOTE
        // This is not yet called by our view object to which it is tied.
        // We could attach to another view which is instantiated without the createController attribute
        ///
        public void OnDestroy()
        {
            view.ShipCollision -= CollisionListener;
        }
    }
}