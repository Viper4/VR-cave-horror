using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public float fadeDuration = 2;
    [SerializeField] Color fadeColor;
    MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Fade(float alphaA, float alphaB)
    {
        StartCoroutine(FadeRoutine(alphaA, alphaB));
    }

    IEnumerator FadeRoutine(float alphaA, float alphaB)
    {
        float timer = 0;
        while(timer < fadeDuration)
        {
            Color lerpedColor = fadeColor;
            lerpedColor.a = Mathf.Lerp(alphaA, alphaB, timer / fadeDuration);

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
        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            finalMaterials[i].color = finalColor;
        }
        _meshRenderer.SetMaterials(finalMaterials);
    }
}
