using UnityEngine;

namespace TNNL.UI
{
    public class AnimationDestroyer : MonoBehaviour
    {
        public void DestroyAnimation()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}