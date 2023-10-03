
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "ScriptableObjects/LevelSection", order = 1)]
public class LevelSection : ScriptableObject
{
    public int Width;
    public int Height;
    public float ChanceForShieldInLine = 0.01f;
    public float ChanceForMineInLine = 0.1f;
    public int TotalMines;
    public int TotalShields;
    public LevelBlockNotation[] Notations; // this is an array notating all non-default cubes & inactive default cubes
}

/// <summary>
/// This is the model detailing properties of the block generated at each section
/// </summary>
/// 

[Serializable]
public class LevelBlockNotation
{
    public LevelBlockNotation(int index, LevelBlockType type, bool isActive)
    {
        this.Index = index;
        this.Type = type;
        this.IsActive = isActive;
    }
    public int Index;
    public bool IsActive;
    public LevelBlockType Type;
}

public enum LevelBlockType
{
    DefaultTerrain,
    Mine,
    ShieldBoost
}
