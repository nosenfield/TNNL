using System;
using TNNL.Collidables;
using TNNL.Player;

namespace TNNL.RuntimeData
{
    [Serializable]
    public class PlayerRuntimeData
    {
        public int TotalPoints = 0;
        public int ShipsRemaining = 0;
        public int StartingShips = 3;

        public PlayerRuntimeData()
        {
            ShieldController.ShieldCollision += ShieldCollisionListener;
            ShipController.ShipCollision += ShieldCollisionListener;
        }

        public void ResetPlayerData()
        {
            ShipsRemaining = StartingShips;
            TotalPoints = 0;
        }

        // Point value increases should be handled through a PointsEarned event dispatched through an event hub
        // This creates a layer of abstraction between the UserData and the many future gameplay/meta mechanics that will affect TotalPoints, Lives, etc.
        void ShieldCollisionListener(AbstractCollidable collidable)
        {
            // NOTE
            // Points earned in collisions feel more closely related to our meta system than our physics system.
            // Are the Collidable classes the right place to store these?
            // Consider:
            // - a separate config pairing collidable types to point values
            ///

            TotalPoints += collidable.CollisionPoints;
            if (TotalPoints < 0)
            {
                TotalPoints = 0;
            }
        }
    }
}