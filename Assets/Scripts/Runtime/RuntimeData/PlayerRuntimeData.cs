using System.Collections.Generic;
using TNNL.Events;

namespace TNNL.RuntimeData
{
    public class ShipData
    {
        public string CheckpointReached = ""; // what is the last known location of our ship in the level progression
        public bool IsAlive = true; // is this ship still active
    }

    public class PlayerRuntimeData
    {
        private int numStartingShips = 3; // Unless this is going to be modifited at Runtime, this is more of a "config" kind of value
        public int TotalPoints = 0;
        public List<ShipData> ShipData;

        public PlayerRuntimeData(List<ShipData> shipData)
        {
            ShipData = shipData;
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
            TotalPoints = 0;
            ShipData = new List<ShipData>();
            for (int i = 0; i < numStartingShips; i++)
            {
                ShipData.Add(new ShipData());
            }
        }
    }
}