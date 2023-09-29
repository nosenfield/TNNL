using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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