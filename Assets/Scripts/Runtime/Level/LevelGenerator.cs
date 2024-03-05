using System.Collections.Generic;
using nosenfield.Logging;
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

            int ShieldCount = 0;
            int MineCount = 0;

            for (int i = 0; i < section.Height; i++)
            {
                bool generateShield = Random.Range(0f, 1f) < section.ChanceForShieldInLine;
                bool generateMine = Random.Range(0f, 1f) < section.ChanceForMineInLine;

                if (generateShield)
                {
                    notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.ShieldBoost, true));
                    ShieldCount++;
                }
                else if (generateMine)
                {
                    notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.Mine, true));
                    MineCount++;
                }
            }

            // if the very last block in our section is not already defined, define a default cube.
            // this allows our level recreation method to only iterate over the defined indexes and fill all others with default terrain blocks
            int finalIndex = section.Width * section.Height - 1;
            if (notations.Count == 0 || notations[notations.Count - 1].Index != finalIndex)
            {
                notations.Add(new LevelBlockNotation(finalIndex, LevelBlockType.DefaultTerrain, true));
            }

            notations.Add(new LevelBlockNotation(finalIndex + 1, LevelBlockType.FinishLine, true));

            if (section.GenerateWarp)
            {
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"Generating warp...");

                float availableDistance = section.MaxWarpLocationByPercent - section.MinWarpLocationByPercent;
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"availableDistance: {availableDistance}");
                float minWarpDistance = Mathf.Min(section.MinWarpDistance, availableDistance);
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"minWarpDistance: {minWarpDistance}");
                float maxWarpDistance = Mathf.Min(section.MaxWarpDistance, availableDistance);
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"maxWarpDistance: {maxWarpDistance}");

                float warpDistance = Random.Range(minWarpDistance, maxWarpDistance);

                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"warpDistance: {warpDistance}");

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

                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"warpRowA: {warpRowA}");

                int warpRowB = warpRowA + Mathf.RoundToInt(section.Height * warpDistance);
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"warpRowB: {warpRowB}");

                int indexA = section.Width * warpRowA + warpCol;
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"indexA: {indexA}");
                int indexB = section.Width * warpRowB + warpCol;
                DefaultLogger.Instance.Log(LogLevel.DEBUG, $"indexB: {indexB}");

                int j;
                for (j = 0; j < notations.Count; j++)
                {
                    if (indexA > notations[j].Index)
                    {
                        continue;
                    }
                    else if (indexA < notations[j].Index)
                    {
                        notations.Insert(j, new LevelBlockNotation(indexA, LevelBlockType.WormHole, true));
                        break;
                    }
                    else // if (indexA == notations[j].Index)
                    {
                        notations[j] = new LevelBlockNotation(indexA, LevelBlockType.WormHole, true);
                        break;
                    }
                }

                for (j = j + 1; j < notations.Count; j++)
                {
                    if (indexB > notations[j].Index)
                    {
                        continue;
                    }
                    else if (indexB < notations[j].Index)
                    {
                        notations.Insert(j, new LevelBlockNotation(indexB, LevelBlockType.WormHole, true));
                        break;
                    }
                    else // if (indexB == notations[j].Index)
                    {
                        notations[j] = new LevelBlockNotation(indexB, LevelBlockType.WormHole, true);
                        break;
                    }
                }
            }

            section.Notations = notations.ToArray();
            section.ShieldCount = ShieldCount;
            section.MineCount = MineCount;
        }
    }
}