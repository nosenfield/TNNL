using System;
using System.Collections.Generic;
using TNNL.Events;

namespace TNNL.RuntimeData
{
    [Serializable]
    public class AttemptData
    {
        public string CheckpointReached = ""; // what is the last known location of our ship in the level progression
        public bool IsAlive = true; // is this ship still active
    }

    [Serializable]
    public class PlayerRuntimeData
    {
        private int numAttempts = 3; // Unless this is going to be modifited at Runtime, this is more of a "config" kind of value
        public int TotalPoints = 0;
        public List<AttemptData> Attempts;

        public PlayerRuntimeData(List<AttemptData> attempts)
        {
            Attempts = attempts;
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

        public void ResetAttemptsAndScore()
        {
            TotalPoints = 0;
            Attempts = new List<AttemptData>();
            for (int i = 0; i < numAttempts; i++)
            {
                Attempts.Add(new AttemptData());
            }
        }
    }
}