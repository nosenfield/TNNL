using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCubeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject finishLineBlockPrefab;
    [SerializeField] private GameObject levelCubePrefab;
    [SerializeField] private GameObject shieldCubePrefab;
    [SerializeField] private GameObject mineCubePrefab;
    [SerializeField] private StatusText statusText;

    public float ChanceForShieldInLine = 0.01f;
    public float ChanceForMineInLine = 0.1f;

    public int TotalMines;
    public int TotalShields;

    private List<GameObject> cubes;

    void Awake()
    {
        cubes = new List<GameObject>();
        // convert this cube to many cubes
        float startX = transform.position.x - transform.lossyScale.x * .5f;
        float startY = transform.position.y - transform.lossyScale.y * .5f;
        float endX = transform.position.x + transform.lossyScale.x * .5f;
        float endY = transform.position.y + transform.lossyScale.y * .5f;

        GameObject cube;
        float cubeX = startX;
        float cubeY = startY;
        GameObject prefab;

        while (cubeY < endY)
        {
            cubeX = startX;

            bool generateShield = Random.Range(0f, 1f) < ChanceForShieldInLine;
            bool generateMine = Random.Range(0f, 1f) < ChanceForMineInLine;
            float shieldPos = generateShield ? Random.Range(startX, endX) : -1f;
            float minePos = generateMine ? Random.Range(startX, endX) : -1f;

            while (cubeX < endX)
            {

                if (generateShield && cubeX >= shieldPos)
                {
                    prefab = shieldCubePrefab;
                    TotalShields++;
                    generateShield = false;
                }
                else if (generateMine && cubeX >= minePos)
                {
                    prefab = mineCubePrefab;
                    TotalMines++;
                    generateMine = false;
                }
                else
                {
                    prefab = levelCubePrefab;
                }

                cube = GameObject.Instantiate(prefab, new Vector3(cubeX, cubeY, transform.position.z), Quaternion.identity, transform.parent);
                cube.transform.localScale = Vector3.one;
                cubes.Add(cube);

                cubeX++;
            }
            cubeY++;
        }

        //Add 1 giant finishline cube
        float finishLineHeight = 2f;
        cube = GameObject.Instantiate(finishLineBlockPrefab, new Vector3(transform.position.x, cubeY + finishLineHeight * .5f, transform.position.z), Quaternion.identity, transform.parent);
        cube.transform.localScale = new Vector3(transform.lossyScale.x, finishLineHeight, cube.transform.localScale.z);

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


        statusText.TotalMines = TotalMines;
        statusText.TotalShields = TotalShields;

        gameObject.SetActive(false);
    }

    public void ResetLevel()
    {
        foreach (GameObject cube in cubes)
        {
            cube.SetActive(true);
        }
    }
}