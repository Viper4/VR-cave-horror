using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpaceEnvironment : MonoBehaviour
{
    public static SpaceEnvironment instance;

    [SerializeField] ScreenFade screenFade;

    [SerializeField, Header("Wake Up Fade")] float fadeTime;

    [SerializeField] AudioReverbFilter reverbFilter;
    [SerializeField] float[] dryLevelFade;

    public bool timing { get; set; }
    public float timer;
    [SerializeField] float suffocateTime = 120;

    void Start()
    {
        instance = this;
        StartCoroutine(WakeUpFade());
    }

    void Update()
    {
        if(timing)
        {
            screenFade.SetFade(timer / suffocateTime);
            if(timer >= suffocateTime)
            {
                SaveSystem.DeleteLatest();
                SceneLoader.instance.LoadScene(0, false);
            }
            timer += Time.deltaTime;
        }
    }

    IEnumerator WakeUpFade()
    {
        float fadeTimer = 0;
        while(fadeTimer < fadeTime)
        {
            reverbFilter.dryLevel = Mathf.Lerp(dryLevelFade[0], dryLevelFade[1], fadeTimer / fadeTime);

            fadeTimer += Time.deltaTime;
            yield return null;
        }
        reverbFilter.dryLevel = dryLevelFade[1];
        timing = true;
    }
}
