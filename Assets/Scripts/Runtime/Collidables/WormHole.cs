// the Mine behaviour is placed on the prefab. adjustable damage allows for tweaking and differentiation (ie. Supermine w/ 2x damage)
using System.Collections.Generic;
using UnityEngine;

namespace TNNL.Collidables
{
    /// <summary>
    /// Whatever enters the WormHole should be sent to the other wormhole
    /// However, each item that goes through may have its own set of methods for handling the interaction
    /// The ship needs to change its location, which is primarily done through the PlayerController
    /// So the player needs a "Warp" method.
    /// The player can be accessed by it listening for a wormhole collision, but it seems better that the worm hole tell the player to warp
    /// 
    /// </summary>
    public class WormHole : AbstractCollidable
    {
        public override CollisionType Type
        {
            get
            {
                return CollisionType.WormHole;
            }
        }

        [SerializeField] private WormHole other;
        private List<IWarpable> incoming;

        void Awake()
        {
            incoming = new List<IWarpable>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            IWarpable warpable = other.GetComponentInParent<IWarpable>();
            if (warpable != null)
            {
                Debug.Log($"IWarpable collision detected by {gameObject.name}");
            }
        }

        public static void PairWarps(WormHole warpA, WormHole warpB)
        {
            warpA.other = warpB;
            warpB.other = warpA;
        }

        // requests and accepts could be run through 2 private methods to compartmentalize the logic to each individual instance instead of object A manipulating object B's lists
        public void WarpRequest(IWarpable warpable)
        {
            Debug.LogWarning("Incoming warp request.");

            if (incoming.Contains(warpable))
            {
                incoming.Remove(warpable);
            }
            else if (other.incoming.Contains(warpable))
            {
                Debug.LogWarning("This object has alredy requested warp.");
            }
            else
            {
                other.incoming.Add(warpable);
                warpable.Warp(other);
            }
        }
    }
}