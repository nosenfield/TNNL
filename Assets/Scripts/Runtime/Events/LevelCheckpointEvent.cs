using System.Numerics;
using TNNL.Level;
using UnityEngine;

namespace TNNL.Events
{
    public class LevelCheckpointEvent
    {
        public static void Dispatch(LevelSection levelSection)
        {
            Instance.LevelSection = levelSection;
            EventAggregator.Publish(Instance);
        }
        private static readonly LevelCheckpointEvent Instance = new() { LevelSection = null };
        public LevelSection LevelSection;
    }
}