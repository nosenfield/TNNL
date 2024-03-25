using System;
using System.Collections.Generic;
using TNNL.Collidables;
using TNNL.Player;
using UnityEngine;

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
        [SerializeField] private GameObject electricGatePrefab;
        [SerializeField] private GameObject invincibilityPrefab;
        private int FINISH_LINE_ROWS = 2;
        private List<GameObject> cubes;

        int levelIndex = -1;
        public string CurrentLevelName
        {
            get
            {
                return levelSections[levelIndex].DisplayName;
            }
        }

        public string CurrentLevelId
        {
            get
            {
                return levelSections[levelIndex].Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A copy of the current level section</returns>
        public LevelSection GetCurrentSection()
        {
            return ScriptableObject.Instantiate(levelSections[levelIndex]);
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

        public void ClearLevel()
        {
            if (cubes != null)
            {
                foreach (GameObject cube in cubes)
                {
                    GameObject.DestroyImmediate(cube);
                }

                cubes = null;
            }
        }

        private void ParseLevel(LevelSection section)
        {
            cubes = new List<GameObject>();
            GameObject cube = null;
            WormHole firstWarpReached = null;

            levelContainer.transform.position = Vector3.zero;

            for (int y = 0; y < section.Height; y++)
            {
                for (int x = 0; x < section.Width; x++)
                {
                    // make default cube
                    cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(x, y * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                    cubes.Add(cube);
                    cube.name = $"{cube.name}-{y}_{x}";
                }
            }


            int notatedRow;
            int notatedCol;

            for (int i = 0; i < section.Notations.Length; i++)
            {
                notatedRow = section.Notations[i].Index / section.Width;
                notatedCol = section.Notations[i].Index % section.Width;

                switch (section.Notations[i].Type)
                {
                    case LevelBlockType.DefaultTerrain:
                        cube = GameObject.Instantiate(defaultTerrainPrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;

                    case LevelBlockType.Mine:
                        cube = GameObject.Instantiate(minePrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;

                    case LevelBlockType.ShieldBoost:
                        cube = GameObject.Instantiate(shieldPrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;

                    case LevelBlockType.FinishLine:
                        cube = GameObject.Instantiate(finishLinePrefab, new Vector3(section.Width * .5f, (notatedRow - 1 + FINISH_LINE_ROWS * .5f) * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cube.transform.localScale = new Vector3(section.Width, FINISH_LINE_ROWS, cube.transform.localScale.z);
                        break;

                    case LevelBlockType.WormHole:
                        cube = GameObject.Instantiate(wormHolePrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);

                        // this will link warps in a first-come first-serve manner (eg. AABB)
                        // will need revisiting if we want other linkage arrangements (eg. ABBA)

                        if (firstWarpReached == null)
                        {
                            firstWarpReached = cube.GetComponentInChildren<WormHole>();
                        }
                        else
                        {
                            WormHole.PairWarps(firstWarpReached, cube.GetComponentInChildren<WormHole>());
                            firstWarpReached = null;
                        }
                        break;

                    case LevelBlockType.ElectricGate:
                        cube = GameObject.Instantiate(electricGatePrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        cube.GetComponentInChildren<ElectricGate>().SetGatePositionAndWidth(section.Width * .5f, section.Width);
                        break;

                    case LevelBlockType.Invincibility:
                        cube = GameObject.Instantiate(invincibilityPrefab, new Vector3(notatedCol, notatedRow * PlayerModel.Direction, 0f), Quaternion.identity, levelContainer.transform);
                        break;
                }

                cubes.Add(cube);
                cube.name = $"{cube.name}-{notatedRow}_{notatedCol}";
                cube.SetActive(section.Notations[i].IsActive);
            }

            levelContainer.transform.position = new Vector3(-section.Width * .5f, levelContainer.transform.position.y, levelContainer.transform.position.z);
            levelBacking.transform.position = new Vector3(levelContainer.transform.position.x + section.Width * .5f, (levelContainer.transform.position.y + section.Height * .5f) * PlayerModel.Direction, levelBacking.transform.position.z);
            levelBacking.transform.localScale = new Vector3(section.Width, section.Height, 1);

            LevelCreated?.Invoke(section.Width);
        }

        public void ResetLevel()
        {
            if (cubes == null)
            {
                LoadNextLevel();
            }
            else
            {
                foreach (GameObject cube in cubes)
                {
                    cube.GetComponentInChildren<AbstractCollidable>().Activate();
                }
            }
        }
    }
}