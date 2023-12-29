using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Color fadeColor;
    MeshRenderer _meshRenderer;
    public bool done { get; private set; } = true;
    float alpha;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Fade(float alphaA, float alphaB, float duration)
    {
        StartCoroutine(FadeRoutine(alphaA, alphaB, duration));
    }

    public void Fade(float alphaB, float duration)
    {
        Fade(alpha, alphaB, duration);
    }

    IEnumerator FadeRoutine(float alphaA, float alphaB, float duration)
    {
        done = false;
        float timer = 0;
        while(timer < duration)
        {
            Color lerpedColor = fadeColor;
            alpha = lerpedColor.a = Mathf.Lerp(alphaA, alphaB, timer / duration);

            List<Material> newMaterials = new List<Material>();
            _meshRenderer.GetMaterials(newMaterials);
            for(int i = 0; i < _meshRenderer.materials.Length; i++) 
            {
                newMaterials[i].color = lerpedColor;
            }
            _meshRenderer.SetMaterials(newMaterials);

            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaB;
        List<Material> finalMaterials = new List<Material>();
        _meshRenderer.GetMaterials(finalMaterials);
        for(int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            finalMaterials[i].color = finalColor;
        }
        _meshRenderer.SetMaterials(finalMaterials);
        done = true;
    }

    public void SetFade(float value)
    {
        Color color = fadeColor;
        color.a = value;
        List<Material> fadeMaterials = new List<Material>();
        _meshRenderer.GetMaterials(fadeMaterials);
        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            fadeMaterials[i].color = color;
        }
        _meshRenderer.SetMaterials(fadeMaterials);
    }
}
