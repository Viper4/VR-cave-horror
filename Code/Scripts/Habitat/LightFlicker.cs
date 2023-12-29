using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    Light _light;

    [SerializeField] float mainIntensity = 1;
    [SerializeField] float secondIntensity = 0;
    [SerializeField] float flickerTime = 0.05f;
    float timer;
    [SerializeField] Vector2 resetTimeRange;
    float resetTime;

    void Start()
    {
        _light = GetComponent<Light>();
        resetTime = Random.Range(resetTimeRange.x, resetTimeRange.y);
    }

    void Update()
    {
        _light.intensity = timer > flickerTime ? mainIntensity : secondIntensity;
        if(timer > resetTime)
        {
            resetTime = Random.Range(resetTimeRange.x, resetTimeRange.y);
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
