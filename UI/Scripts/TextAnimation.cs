using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextAnimation : MonoBehaviour
{
    TMP_Text textBox;
    
    [SerializeField] bool playOnStart = false;
    [SerializeField] bool loop = true;
    [SerializeField] bool specificRange = false;
    [ConditionalHide("specificRange")] public int characterStart = 0;
    [ConditionalHide("specificRange")] public int characterEnd = 0;
    [SerializeField] float characterSpeed = 0.25f;
    Coroutine animationRoutine;

    void Start()
    {
        textBox = GetComponent<TMP_Text>();
        if (playOnStart)
            animationRoutine = StartCoroutine(AnimationRoutine());
    }

    public void Play()
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);
        animationRoutine = StartCoroutine(AnimationRoutine());
    }

    public void Stop()
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);
    }

    void HideCharacters(TMP_TextInfo textInfo, int startIndex, int endIndex)
    {
        for (int i = startIndex; i <= endIndex; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (charInfo.isVisible)
            {
                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
                Color32 characterColor = new Color32(0, 0, 0, 0);
                destinationColors[vertexIndex + 0] = characterColor;
                destinationColors[vertexIndex + 1] = characterColor;
                destinationColors[vertexIndex + 2] = characterColor;
                destinationColors[vertexIndex + 3] = characterColor;
            }
        }
        textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    void AnimationEnd()
    {
        if (loop)
            animationRoutine = StartCoroutine(AnimationRoutine());
        else
            animationRoutine = null;
    }

    IEnumerator AnimationRoutine()
    {
        textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = textBox.textInfo;

        int characterCount = textInfo.characterCount;
        if(characterCount > 0)
        {
            Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
            for (int i = 0; i < originalColors.Length; i++)
            {
                Color32[] colors = textInfo.meshInfo[i].colors32;
                originalColors[i] = new Color32[colors.Length];
                Array.Copy(colors, originalColors[i], colors.Length);
            }

            int startIndex = 0;
            int endIndex = characterCount - 1;
            if (specificRange)
            {
                startIndex = Mathf.Min(characterStart, characterCount - 1);
                endIndex = Mathf.Min(characterStart, characterCount - 1);
            }

            HideCharacters(textInfo, startIndex, endIndex);

            for (int i = startIndex; i <= endIndex; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                while (!charInfo.isVisible)
                {
                    i++;
                    if (i > characterCount)
                    {
                        AnimationEnd();
                        yield break;
                    }
                }

                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
                vertexColors[vertexIndex + 0] = originalColors[materialIndex][vertexIndex];
                vertexColors[vertexIndex + 1] = originalColors[materialIndex][vertexIndex];
                vertexColors[vertexIndex + 2] = originalColors[materialIndex][vertexIndex];
                vertexColors[vertexIndex + 3] = originalColors[materialIndex][vertexIndex];

                textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(characterSpeed);
            }
            AnimationEnd();
        }
        else
        {
            animationRoutine = null;
        }
    }
}
