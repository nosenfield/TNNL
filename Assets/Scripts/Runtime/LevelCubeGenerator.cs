using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCubeGenerator : MonoBehaviour
{
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