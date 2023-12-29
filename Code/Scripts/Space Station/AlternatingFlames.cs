using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingFlames : MonoBehaviour
{
    [SerializeField] float interval = 4;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] ParticleSystemRenderer[] first;
    [SerializeField] ParticleSystemRenderer[] second;

    void Start()
    {
        StartCoroutine(AlternatingRoutine());
    }

    IEnumerator AlternatingRoutine()
    {
        float time = 0;
        while(time < fadeTime)
        {
            time += Time.deltaTime;
            float t = time / fadeTime;
            for (int i = 0; i < first.Length; i++)
            {
                Color color = first[i].material.color;
                color.a = Mathf.Lerp(1, 0, t);
                first[i].material.color = color;
            }
            for (int i = 0; i < second.Length; i++)
            {
                Color color = second[i].material.color;
                color.a = Mathf.Lerp(0, 1, t);
                second[i].material.color = color;
            }
            yield return null;
        }

        yield return new WaitForSeconds(interval);
        time = 0;
        while(time < fadeTime)
        {
            time += Time.deltaTime;
            float t = time / fadeTime;
            for (int i = 0; i < first.Length; i++)
            {
                Color color = first[i].material.color;
                color.a = Mathf.Lerp(0, 1, t);
                first[i].material.color = color;
            }
            for (int i = 0; i < second.Length; i++)
            {
                Color color = second[i].material.color;
                color.a = Mathf.Lerp(1, 0, t);
                second[i].material.color = color;
            }
            yield return null;
        }

        yield return new WaitForSeconds(interval);
        StartCoroutine(AlternatingRoutine());
    }
}
