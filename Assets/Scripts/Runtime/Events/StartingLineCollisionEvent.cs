using System.Numerics;
using TNNL.Level;
using UnityEngine;

namespace TNNL.Events
{
    public class StartingLineCollisionEvent
    {
        public static void Dispatch()
        {
            EventAggregator.Publish(Instance);
        }
        private static readonly StartingLineCollisionEvent Instance = new();
    }
}