using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct TextLoop
{
    public string text;
    public float characterSpeed;
    public int startIndex;
    public int endIndex;
    public int maxLoops;
}

public class TextAnimation : MonoBehaviour
{
    TMP_Text textBox;

    [SerializeField] bool playOnStart = false;
    [SerializeField] TextLoop[] textLoops;

    Coroutine animationRoutine;

    void Start()
    {
        textBox = GetComponent<TMP_Text>();
        if (playOnStart)
            animationRoutine = StartCoroutine(AnimationRoutine());
    }

    public void Play()
    {
        if(textBox == null)
            textBox = GetComponent<TMP_Text>();

        if(animationRoutine != null)
            StopCoroutine(animationRoutine);
        animationRoutine = StartCoroutine(AnimationRoutine());
    }

    public void Stop()
    {
        if(animationRoutine != null)
            StopCoroutine(animationRoutine);
    }

    IEnumerator AnimationRoutine()
    {
        foreach(TextLoop textLoop in textLoops)
        {
            int loops = 0;
            if (textLoop.startIndex != -1 && textLoop.endIndex != -1)
            {
                while (textLoop.maxLoops == -1 || loops < textLoop.maxLoops)
                {
                    textBox.text = textLoop.text[..textLoop.startIndex];
                    yield return new WaitForSeconds(textLoop.characterSpeed);
                    for (int i = textLoop.startIndex; i <= textLoop.endIndex; i++)
                    {
                        textBox.text += textLoop.text[i];
                        yield return new WaitForSeconds(textLoop.characterSpeed);
                    }
                    loops++;
                }
            }
            else
            {
                while(textLoop.maxLoops == -1 || loops < textLoop.maxLoops)
                {
                    textBox.text = "";
                    yield return new WaitForSeconds(textLoop.characterSpeed);
                    foreach (char character in textLoop.text.ToCharArray())
                    {
                        textBox.text += character;
                        yield return new WaitForSeconds(textLoop.characterSpeed);
                    }
                    loops++;
                }
            }
        }
        animationRoutine = null;
    }
}
