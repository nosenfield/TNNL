using System;
using System.Collections.Generic;
using TNNL.Collidables;
using TNNL.Player;
using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Level
{
    public class LevelParser : MonoBehaviour
    {
        public static LevelParser Instance;
        public static event Action<float> LevelCreated;

        [SerializeField] private LevelSection[] levelSections;

        [SerializeField] private GameObject levelContainer;
        [SerializeField] private GameObject levelBacking;
        [SerializeField] private GameObject defaultTerrainPrefab;
        [SerializeField] private GameObject minePrefab;
        [SerializeField] private GameObject shieldPrefab;
        [SerializeField] private GameObject finishLinePrefab;
        private int FINISH_LINE_ROWS = 2;

        [SerializeField] private LevelSection section;
        private List<AbstractCollidable> cubes;

        int levelIndex = -1;

        void Awake()
        {
            Instance = this;
        }

        public void LoadPrevLevel()
        {
            levelIndex = levelIndex == 0 ? levelSections.Length - 1 : levelIndex - 1;
            ParseLevel(levelSections[levelIndex]);
        }

        public void LoadNextLevel()
        {
            levelIndex = levelIndex == levelSections.Length - 1 ? 0 : levelIndex + 1;
            ParseLevel(levelSections[levelIndex]);
        }

        private void ParseLevel(LevelSection section)
        {
            cubes = new List<AbstractCollidable>();
            GameObject cube = null;

            int curRow = 0;
            int curCol = 0;

            for (int i = 0; i < section.Notations.Length; i++)
            {
                int notatedRow = section.Notations[i].Index / section.Width;
                int notatedCol = section.Notations[i].Index % section.Width;

                while (curRow != notatedRow || curCol != notatedCol)
                {
                    // make default cube
                    cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                    cubes.Add(cube.GetComponentInChildren<AbstractCollidable>());

                    curCol++;
                    if (curCol == section.Width)
                    {
                        curCol = 0;
                        curRow++;
                    }
                }

                // we've hit the notated index
                switch (section.Notations[i].Type)
                {
                    case LevelBlockType.Mine:

                        // create a default terrain block behind the mine
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cubes.Add(cube.GetComponentInChildren<AbstractCollidable>());

                        cube = GameObject.Instantiate(minePrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;
                    case LevelBlockType.ShieldBoost:
                        // create a default terrain block behind the shield
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cubes.Add(cube.GetComponentInChildren<AbstractCollidable>());

                        cube = GameObject.Instantiate(shieldPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;
                    case LevelBlockType.DefaultTerrain:
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;
                    case LevelBlockType.FinishLine:
                        cube = GameObject.Instantiate(finishLinePrefab, new Vector3(curCol + section.Width * .5f, (section.Height + FINISH_LINE_ROWS * .5f) * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cube.transform.localScale = new Vector3(section.Width, FINISH_LINE_ROWS, cube.transform.localScale.z);
                        break;
                }

                cubes.Add(cube.GetComponentInChildren<AbstractCollidable>());
                cube.SetActive(section.Notations[i].IsActive);

                curCol++;
                if (curCol == section.Width)
                {
                    curCol = 0;
                    curRow++;
                }
            }

            // Debug.Log($"curRow: {curRow}, curCol: {curCol}");

            levelContainer.transform.position = new Vector3(-section.Width * .5f, levelContainer.transform.position.y, levelContainer.transform.position.z);
            levelBacking.transform.position = new Vector3(levelContainer.transform.position.x + section.Width * .5f, (levelContainer.transform.position.y + section.Height * .5f) * PlayerModel.Direction, levelBacking.transform.position.z);
            levelBacking.transform.localScale = new Vector3(section.Width, section.Height, 1);

            LevelCreated?.Invoke(section.Width);

            // while (cubeY < endY)
            // {
            //     cubeX = startX;

            //     bool generateShield = Random.Range(0f, 1f) < ChanceForShieldInLine;
            //     bool generateMine = Random.Range(0f, 1f) < ChanceForMineInLine;
            //     float shieldPos = generateShield ? Random.Range(startX, endX) : -1f;
            //     float minePos = generateMine ? Random.Range(startX, endX) : -1f;

            //     while (cubeX < endX)
            //     {

            //         if (generateShield && cubeX >= shieldPos)
            //         {
            //             prefab = shieldCubePrefab;
            //             TotalShields++;
            //             generateShield = false;
            //         }
            //         else if (generateMine && cubeX >= minePos)
            //         {
            //             prefab = mineCubePrefab;
            //             TotalMines++;
            //             generateMine = false;
            //         }
            //         else
            //         {
            //             prefab = levelCubePrefab;
            //         }

            //         cube = GameObject.Instantiate(prefab, new Vector3(cubeX, cubeY, transform.position.z), Quaternion.identity, transform.parent);
            //         cube.transform.localScale = Vector3.one;
            //         cubes.Add(cube);

            //         cubeX++;
            //     }
            //     cubeY++;
            // }

            // //Add 1 giant finishline cube
            // float finishLineHeight = 2f;
            // cube = GameObject.Instantiate(finishLineBlockPrefab, new Vector3(transform.position.x, cubeY + finishLineHeight * .5f, transform.position.z), Quaternion.identity, transform.parent);
            // cube.transform.localScale = new Vector3(transform.lossyScale.x, finishLineHeight, cube.transform.localScale.z);

            // Add 2 rows of Finish Line Cubes
            // prefab = finishLineBlockPrefab;
            // for (int i = 0; i < 2; i++)
            // {
            //     for (cubeX = startX; cubeX < endX; cubeX++)
            //     {
            //         cube = GameObject.Instantiate(prefab, new Vector3(cubeX, cubeY + i, transform.position.z), Quaternion.identity, transform.parent);
            //         cube.transform.localScale = Vector3.one;
            //     }
            // }
        }

        public void ResetLevel()
        {
            Debug.Log("LevelParse.ResetLevel");

            if (cubes == null)
            {
                LoadNextLevel();
            }
            else
            {
                Debug.Log($"cubes.Count: {cubes.Count}");

                foreach (AbstractCollidable cube in cubes)
                {
                    cube.Activate();
                }
            }
        }
    }
}