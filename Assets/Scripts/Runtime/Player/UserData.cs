using System;
using TNNL.Collidables;
using TNNL.Player;

[Serializable]
public class UserData
{
    public int TotalPoints = 0;
    public int StartingRuns = 3;
    public int CurrentRun = -1;

    public UserData()
    {
        ShieldController.ShieldCollision += ShieldCollisionListener;
        ShipController.ShipCollision += ShieldCollisionListener;
    }

    public void ResetPlayerData()
    {
        CurrentRun = StartingRuns;
        TotalPoints = 0;
    }

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