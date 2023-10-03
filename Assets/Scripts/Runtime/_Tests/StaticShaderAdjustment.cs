using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class increases the static by shifting the range of the available the gradient toward the white end (0)
/// </summary>
public class StaticShaderAdjustment : MonoBehaviour
{
    [SerializeField] Material mat;

    [SerializeField] float startingMin = 0.5f;
    [SerializeField] float targetMin = 0.0f;
    [SerializeField] float startingMax = 1f;
    [SerializeField] float targetMax = .5f;

    [SerializeField] float duration = 3f;
    [SerializeField] float time = 0f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        float min = Mathf.Lerp(startingMin, targetMin, time / duration);
        float max = Mathf.Lerp(startingMax, targetMax, time / duration);

        mat.SetFloat("_Min", min);
        mat.SetFloat("_Max", max);

        if (time >= duration)
        {
            time = 0f;
        }
    }

    void OnDisable()
    {
        mat.SetFloat("_Min", startingMin);
        mat.SetFloat("_Max", startingMax);
    }
}
