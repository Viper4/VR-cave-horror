using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFluctuation : MonoBehaviour
{
    TMP_Text tmpText;
    [SerializeField] string prefix;
    [SerializeField] string separator;
    [SerializeField] string suffix;

    float[] values;
    [SerializeField] float[] targetValues;
    [SerializeField] float fluctuationScale = 0.1f;
    [SerializeField] float fluctuationSpeed = 1;
    [SerializeField] int decimals;
    float[] perlinX;
    float multiplier;

    void Start()
    {
        values = new float[targetValues.Length];
        tmpText = GetComponent<TMP_Text>();
        perlinX = new float[targetValues.Length];
        for(int i = 0; i < perlinX.Length; i++)
        {
            perlinX[i] = Random.Range(-10000f, 10000f);
        }
        multiplier = Mathf.Pow(10, decimals);
    }

    void Update()
    {
        string text = prefix;
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = Mathf.Round((targetValues[i] + ((Mathf.PerlinNoise1D(perlinX[i]) - 0.5f) * fluctuationScale * 2)) * multiplier) / multiplier;
            text += i > 0 ? separator + values[i] : values[i];
            perlinX[i] += Time.deltaTime * fluctuationSpeed;
        }
        tmpText.text = text + suffix;
    }
}
