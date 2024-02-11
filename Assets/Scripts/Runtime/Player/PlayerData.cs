using System;
using TNNL.Collidables;
using TNNL.Player;

[Serializable]
public class PlayerData
{
    public int TotalPoints;
    public int StartingLives = 3;
    public int CurrentLives;

    int TerrainCollisionPoints = 1;
    int MineCollisionPoints = -1000;
    int ShieldCollisionPoints = 1000;

    public PlayerData()
    {
        ResetPlayerData();
        ShieldController.ShieldCollision += ShieldCollisionListener;
    }

    public void ResetPlayerData()
    {
        CurrentLives = StartingLives;
        TotalPoints = 0;
    }

    void ShieldCollisionListener(IShieldCollidable collidable)
    {
        switch (collidable.Type)
        {
            case ShieldCollisionType.Terrain:
                TotalPoints += TerrainCollisionPoints;
                break;

            case ShieldCollisionType.Mine:
                TotalPoints += MineCollisionPoints;
                if (TotalPoints < 0)
                {
                    TotalPoints = 0;
                }
                break;

            case ShieldCollisionType.ShieldBoost:
                TotalPoints += ShieldCollisionPoints;
                break;
        }
    }
}