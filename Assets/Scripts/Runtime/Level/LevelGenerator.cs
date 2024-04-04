using System.Collections.Generic;
using System.Linq;
using nosenfield.Logging;
using TNNL.Collidables;
using UnityEngine;

namespace TNNL.Level
{
    public class LevelGenerator
    {
        /// <summary>
        /// Pass a Level Section scriptable object to this function and it will generate randomized level data based on the config and write it to the model
        /// </summary>
        /// <param name="section"></param>
        public static void GenerateLevelSection(LevelSection section)
        {
            List<LevelBlockNotation> notations = new List<LevelBlockNotation>();

            section.ShieldCount = 0;
            section.MineCount = 0;
            section.InvincibilityCount = 0;
            section.WarpCount = 0;
            section.ElectricGateCount = 0;

            float distanceSinceInvincibility = 0;

            // Generates:
            // - shields
            // - mines
            // - invincibility

            for (int i = 0; i < section.Height; i++)
            {
                bool generateShield = Random.Range(0f, 1f) < section.ChanceForShieldInLine;
                bool generateMine = Random.Range(0f, 1f) < section.ChanceForMineInLine;

                // for every 10 rows since generation of the previous invisibility, increase the chance by 1x
                bool generateInvincibility = distanceSinceInvincibility >= section.MinDistanceBetweenInvincibility && Random.Range(0f, 1f) * (section.MinDistanceBetweenInvincibility / distanceSinceInvincibility) < section.ChanceForInvincibilityInLine;

                if (generateShield)
                {
                    notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.ShieldBoost, true));
                    section.ShieldCount++;
                }

                if (generateMine)
                {
                    notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.Mine, true));
                    section.MineCount++;
                }

                if (generateInvincibility)
                {
                    notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.Invincibility, true));
                    section.InvincibilityCount++;
                    distanceSinceInvincibility = 0;
                }

                distanceSinceInvincibility++;
            }

            // NOTE
            // We now add a FinishLine block annotation the end of the section so we no longer need to add a terrain section block
            //
            // [Deprecated]
            // int finalIndex = section.Width * section.Height - 1;
            // if (notations.Count == 0 || notations[notations.Count - 1].Index != finalIndex)
            // {
            //     notations.Add(new LevelBlockNotation(finalIndex, LevelBlockType.DefaultTerrain, true));
            // }
            ///

            // NOTE
            // We now add a premade "finish line" section at the end of every level at parse time
            //
            // [Deprecated]
            // notations.Add(new LevelBlockNotation(section.Width * section.Height, LevelBlockType.FinishLine, true));
            ///

            // generate Warp

            if (section.GenerateWarp)
            {

                float availableDistance = section.MaxWarpLocationByPercent - section.MinWarpLocationByPercent;
                float minWarpDistance = Mathf.Min(section.MinWarpDistance, availableDistance);
                float maxWarpDistance = Mathf.Min(section.MaxWarpDistance, availableDistance);
                float warpDistance = Random.Range(minWarpDistance, maxWarpDistance);

                if (section.MinWarpDistance > availableDistance)
                {
                    DefaultLogger.Instance.Log(LogLevel.WARN, $"MinWarpDistance ({section.MinWarpDistance}) is greater than availableDistance ({availableDistance}). Using maximum available distance.");
                }

                if (section.MaxWarpDistance > availableDistance)
                {
                    DefaultLogger.Instance.Log(LogLevel.WARN, $"MaxWarpDistance ({section.MaxWarpDistance}) is greater than availableDistance ({availableDistance}). Using maximum available distance.");
                }

                int warpRowA = Mathf.RoundToInt(section.Height * Random.Range(section.MinWarpLocationByPercent, section.MaxWarpLocationByPercent - warpDistance));
                float buffer = .2f;
                int warpCol = Random.Range(Mathf.CeilToInt(section.Width * buffer), Mathf.FloorToInt(section.Width * (1 - buffer)));


                int warpRowB = warpRowA + Mathf.RoundToInt(section.Height * warpDistance);

                int indexA = section.Width * warpRowA + warpCol;
                int indexB = section.Width * warpRowB + warpCol;

                InsertNotation(new LevelBlockNotation(indexA, LevelBlockType.WormHole, true), notations, true);
                InsertNotation(new LevelBlockNotation(indexB, LevelBlockType.WormHole, true), notations, true);

                section.WarpCount++;
            }

            if (section.GenerateElectricGates)
            {
                int maxPossibleRow = Mathf.FloorToInt(section.MaxElectricGatesLocationByPercent * section.Height);
                int maxRowForCurrentGate;
                int minRowForCurrentGate = Mathf.FloorToInt(section.Height * section.MinElectricGatesLocationByPercent);
                for (int i = (int)Random.Range((float)section.MinElectricGates, (float)section.MaxElectricGates) - 1; i >= 0; i--)
                {
                    maxRowForCurrentGate = maxPossibleRow - i * ElectricGate.MinRowsBetweenGates;
                    int rowForGate = Random.Range(minRowForCurrentGate, maxRowForCurrentGate);

                    // create notation for gate at specified row and insert into notations
                    int index = rowForGate * section.Width;
                    InsertNotation(new LevelBlockNotation(index, LevelBlockType.ElectricGate, true), notations, true);

                    minRowForCurrentGate = rowForGate + ElectricGate.MinRowsBetweenGates;

                    section.ElectricGateCount++;
                }
            }

            section.Notations = notations.ToArray();
        }

        private static void InsertNotation(LevelBlockNotation notation, List<LevelBlockNotation> notations, bool overwrite)
        {
            // NOTE
            // LevelParser is no longer dependent upon our notations being in a specific order,
            // so inserting the warps and gates is unneeded.
            // We could just add these notations to our list of all notations,
            // with the knowledge that doing so could allow a Warp or ElectricGate to share an index and overlap with a mine/shield/invincibility
            /// 

            int index = System.Array.BinarySearch(notations.ToArray(), notation);
            index = index >= 0 ? index : ~index;
            if (index >= notations.Count)
            {
                notations.Insert(index, notation);
            }
            else if (notations[index].Index == notation.Index)
            {
                if (overwrite)
                {
                    notations[index] = notation;
                }
            }
            else
            {
                notations.Insert(index, notation);
            }
        }
    }
}