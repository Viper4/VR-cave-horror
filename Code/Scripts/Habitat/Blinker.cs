using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField] bool playOnStart = true;
    [SerializeField] float startDelayMax = 0;
    [SerializeField] float loopTime = 2;
    [SerializeField] float onTime = 0.25f;

    Material materialInstance;

    [ColorUsage(true, true)] public Color offColor;
    [ColorUsage(true, true)] public Color onColor;
    [SerializeField] bool changeLightColor;
    [SerializeField] Light _light;

    int lightIndex = 0;
    [SerializeField] bool[] lightPattern;

    IEnumerator Start()
    {
        if (onTime > loopTime)
            onTime = loopTime;
        if (TryGetComponent(out MeshRenderer renderer))
            materialInstance = renderer.material;
        if (!_light)
            TryGetComponent(out _light);
        if (playOnStart)
        {
            yield return new WaitForSeconds(Random.Range(0, startDelayMax));
            StartCoroutine(BlinkLoop());
        }
    }

    IEnumerator BlinkLoop()
    {
        if(lightPattern.Length == 0 || lightPattern[lightIndex])
        {
            if (materialInstance)
                materialInstance.SetColor("_EmissionColor", onColor);
            if (changeLightColor)
                _light.color = onColor;
            else
                _light.enabled = true;
        }

        yield return new WaitForSeconds(onTime);

        if (materialInstance)
            materialInstance.SetColor("_EmissionColor", offColor);
        if (changeLightColor)
            _light.color = offColor;
        else
            _light.enabled = false;

        yield return new WaitForSeconds(loopTime - onTime);
        lightIndex++;
        if (lightIndex > lightPattern.Length - 1)
            lightIndex = 0;
        StartCoroutine(BlinkLoop());
    }
}
