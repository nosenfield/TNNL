using System;
using TNNL.Collidables;
using TNNL.Player;

[Serializable]
public class UserData
{
    public int TotalPoints = 0;
    public int StartingRuns = 3;
    public int CurrentRun = -1;

    public int TerrainCollisionPoints = 1;
    public int MineCollisionPoints = -500;
    public int ShieldCollisionPoints = 1000;
    public int LevelCompletionPoints = 10000;

    public UserData()
    {
        ShieldController.ShieldCollision += ShieldCollisionListener;
        FinishLine.FinishLineCollision += FinishLineCollisionListener;
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
        // Hence they are not currently stored with the collidables.
        //
        // However they also feel out of place in the PlayerData class.
        //
        // As the system develops consider:
        // - CollidableConfig class reference held by the collidable 
        // - MVC wrapper for each collidable
        ///

        switch (collidable.Type)
        {
            case CollisionType.Terrain:
                TotalPoints += TerrainCollisionPoints;
                break;

            case CollisionType.Mine:
                TotalPoints += MineCollisionPoints;
                if (TotalPoints < 0)
                {
                    TotalPoints = 0;
                }
                break;

            case CollisionType.ShieldBoost:
                TotalPoints += ShieldCollisionPoints;
                break;
        }
    }

    public void FinishLineCollisionListener()
    {
        TotalPoints += LevelCompletionPoints;
    }
}