using System;
using TNNL.Collidables;
using TNNL.Events;
using TNNL.Player;
using UnityEngine;

namespace TNNL.RuntimeData
{
    public class PlayerRuntimeData
    {
        public int TotalPoints = 0;
        public int ShipsRemaining = 0;
        public int StartingShips = 3;

        public PlayerRuntimeData()
        {
            // add listener for any ICollision event
            EventAggregator.Subscribe<PointCollectionEvent>(PointCollectionListener);
        }

        public void PointCollectionListener(object e)
        {
            PointCollectionEvent pointCollectionEvent = (PointCollectionEvent)e;
            TotalPoints += pointCollectionEvent.Points;
            if (TotalPoints < 0)
            {
                TotalPoints = 0;
            }
        }

        public void ResetPlayerData()
        {
            ShipsRemaining = StartingShips;
            TotalPoints = 0;
        }
    }
}