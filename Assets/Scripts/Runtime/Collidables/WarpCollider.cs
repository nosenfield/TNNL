using Sirenix.OdinInspector;
using UnityEngine;

namespace TNNL.Collidables
{
    public class WarpCollider : SerializedMonoBehaviour
    {
        [SerializeField] private IWarpable Warpable;

        private void OnTriggerEnter(Collider other)
        {
            WormHole collidable = other.GetComponentInParent<WormHole>();
            collidable?.WarpRequest(Warpable);
        }
    }

    public interface IWarpable
    {
        void Warp(WormHole warpDestination);
    }
}