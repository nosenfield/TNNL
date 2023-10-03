using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    /// <summary>
    /// Pass a Level Section scriptable object to this function and it will generate randomized level data based on the config and write it to the model
    /// </summary>
    /// <param name="section"></param>
    public static void GenerateLevelSection(LevelSection section)
    {
        List<LevelBlockNotation> notations = new List<LevelBlockNotation>();

        int TotalShields = 0;
        int TotalMines = 0;

        for (int i = 0; i < section.Height; i++)
        {
            bool generateShield = Random.Range(0f, 1f) < section.ChanceForShieldInLine;
            bool generateMine = Random.Range(0f, 1f) < section.ChanceForMineInLine;

            if (generateShield)
            {
                notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.ShieldBoost, true));
                TotalShields++;
            }
            else if (generateMine)
            {
                notations.Add(new LevelBlockNotation(section.Width * i + Random.Range(0, section.Width), LevelBlockType.Mine, true));
                TotalMines++;
            }
        }

        // if the very last block in our section is not already defined, define a default cube.
        // this allows our level recreation method to only iterate over the defined indexes and fill all others with default terrain blocks
        int finalIndex = section.Width * section.Height - 1;
        if (notations.Count == 0 || notations[notations.Count - 1].Index != finalIndex)
        {
            notations.Add(new LevelBlockNotation(finalIndex, LevelBlockType.DefaultTerrain, true));
        }

        section.Notations = notations.ToArray();
        section.TotalShields = TotalShields;
        section.TotalMines = TotalMines;
    }
}