
using System;
using nosenfield;
using UnityEngine;

namespace TNNL.Level
{
    [CreateAssetMenu(fileName = "LevelSection", menuName = "ScriptableObjects/LevelSection", order = 1)]
    public class LevelSection : ScriptableObject
    {
        public string DisplayName;
        public int HighScore;
        public int Width;
        public int Height;
        public float ChanceForMineInLine = 0.1f;
        public float ChanceForShieldInLine = 0.01f;
        public float ChanceForInvincibilityInLine = 0.005f;
        public float MinDistanceBetweenInvincibility = 200;
        public bool GenerateWarp = false;
        /// <summary>
        /// the minimum percentage of map before a warp can appear
        /// </summary>
        public float MinWarpLocationByPercent = .25f;
        /// <summary>
        /// the maximum percentage of map before which a warp should appear
        /// </summary>
        public float MaxWarpLocationByPercent = .75f;
        /// <summary>
        /// the minimum percentage of map between warp points
        /// </summary>
        public float MinWarpDistance = .1f;
        /// <summary>
        /// the maximum percentage of map between warp points
        /// </summary>
        public float MaxWarpDistance = .5f;
        public bool GenerateElectricGates; // this currently generates full-width electric gates
        public float MinElectricGatesLocationByPercent = .1f;
        public float MaxElectricGatesLocationByPercent = .95f;
        public int MinElectricGates = 3;
        public int MaxElectricGates = 5;
        [ReadOnly] public int MineCount;
        [ReadOnly] public int ShieldCount;
        [ReadOnly] public int InvincibilityCount;
        [ReadOnly] public int WarpCount;
        [ReadOnly] public int ElectricGateCount;
        public LevelBlockNotation[] Notations; // this is an array notating all non-default cubes & inactive default cubes
    }

    /// <summary>
    /// This is the model detailing properties of the block generated at each section
    /// </summary>
    /// 

    [Serializable]
    public class LevelBlockNotation : IComparable<LevelBlockNotation>
    {
        public LevelBlockNotation(int index, LevelBlockType type, bool isActive)
        {
            Index = index;
            Type = type;
            IsActive = isActive;
        }
        public int CompareTo(LevelBlockNotation other)
        {
            return Index.CompareTo(other.Index);
        }
        public int Index;
        public bool IsActive;
        public LevelBlockType Type;
    }

    public enum LevelBlockType
    {
        DefaultTerrain,
        Mine,
        ShieldBoost,
        FinishLine,
        WormHole,
        ElectricGate,
        Invincibility
    }

}