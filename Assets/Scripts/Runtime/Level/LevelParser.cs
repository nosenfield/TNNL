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
        [SerializeField] private GameObject wormHolePrefab;
        private int FINISH_LINE_ROWS = 2;

        [SerializeField] private LevelSection section;
        private List<AbstractCollidable> cubes;

        int levelIndex = -1;
        public string CurrentLevelName
        {
            get
            {

                return levelSections[levelIndex].DisplayName;
            }
        }

        public int HighScore
        {
            get
            {
                return levelSections[levelIndex].HighScore;
            }
            set
            {
                levelSections[levelIndex].HighScore = value;
            }
        }

        void Awake()
        {
            Instance = this;
        }

        public void LoadPrevLevel()
        {
            ClearLevel();
            levelIndex = levelIndex == 0 ? levelSections.Length - 1 : levelIndex - 1;
            ParseLevel(levelSections[levelIndex]);
        }

        public void LoadNextLevel()
        {
            ClearLevel();
            levelIndex = levelIndex == levelSections.Length - 1 ? 0 : levelIndex + 1;
            ParseLevel(levelSections[levelIndex]);
        }

        private void ClearLevel()
        {
            if (cubes != null)
            {
                foreach (AbstractCollidable cube in cubes)
                {
                    GameObject.Destroy(cube.gameObject);
                }
            }
        }

        private void ParseLevel(LevelSection section)
        {
            cubes = new List<AbstractCollidable>();
            GameObject cube = null;

            int curRow = 0;
            int curCol = 0;

            levelContainer.transform.position = Vector3.zero;

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
                    case LevelBlockType.DefaultTerrain:
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;

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

                    case LevelBlockType.FinishLine:
                        cube = GameObject.Instantiate(finishLinePrefab, new Vector3(curCol + section.Width * .5f, (curRow - 1 + FINISH_LINE_ROWS * .5f) * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cube.transform.localScale = new Vector3(section.Width, FINISH_LINE_ROWS, cube.transform.localScale.z);
                        break;

                    case LevelBlockType.WormHole:
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cubes.Add(cube.GetComponentInChildren<AbstractCollidable>());

                        cube = GameObject.Instantiate(wormHolePrefab, new Vector3(curCol, curRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
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