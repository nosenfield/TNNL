using System;
using TNNL.Collidables;
using TNNL.Player;

[Serializable]
public class PlayerData
{
    public int TotalPoints = 0;
    public int StartingLives = 3;
    public int CurrentLives = -1;

    public int TerrainCollisionPoints = 1;
    public int MineCollisionPoints = -500;
    public int ShieldCollisionPoints = 1000;

    public PlayerData()
    {
        ShieldController.ShieldCollision += ShieldCollisionListener;
    }

    public void ResetPlayerData()
    {
        CurrentLives = StartingLives;
        TotalPoints = 0;
    }

    void ShieldCollisionListener(IShieldCollidable collidable, float shieldHealth)
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