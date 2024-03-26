using UnityEngine;

namespace TNNL.Prototype_1
{
    public class LevelCube : MonoBehaviour
    {
        public enum CubeType
        {
            DEFAULT,
            MINE,
            SHIELD
        }
        public CubeType Type;
        public float Damage;
        public GameObject Container;
    }
}