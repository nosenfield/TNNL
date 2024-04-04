using System.Numerics;
using TNNL.Level;
using UnityEngine;

namespace TNNL.Events
{
    public class LevelCheckpointEvent
    {
        public LevelSection LevelSection;
        private static readonly LevelCheckpointEvent Instance = new();
        private LevelCheckpointEvent() { }
        public static void Dispatch(LevelSection levelSection)
        {
            Instance.LevelSection = levelSection;
            EventAggregator.Publish(Instance);
        }
    }
}