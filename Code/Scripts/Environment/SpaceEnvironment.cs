using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpaceEnvironment : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] ScreenFade screenFade;

    [SerializeField, Header("Wake Up Fade")] float fadeTime;

    [SerializeField] AudioMixer masterMixer;
    [SerializeField] float[] dryLevelFade;

    [SerializeField] Volume defaultVolume;
    [SerializeField] float[] dofDistanceFade;
    DepthOfField depthOfField;

    bool awake;
    float timer;
    [SerializeField] float suffocateTime = 120;

    void Start()
    {
        defaultVolume.profile.TryGet(out depthOfField);
        StartCoroutine(WakeUpFade());
    }

    void Update()
    {
        if (awake)
        {
            timer += Time.deltaTime;

            depthOfField.focusDistance.value = Mathf.Lerp(dofDistanceFade[1], dofDistanceFade[0], timer / suffocateTime);
            screenFade.SetFade(timer / suffocateTime);
            if(timer >= suffocateTime)
                sceneLoader.LoadScene(0, false, false);
        }
    }

    IEnumerator WakeUpFade()
    {
        float fadeTimer = 0;
        depthOfField.active = true;
        while (fadeTimer < fadeTime)
        {
            depthOfField.focusDistance.value = Mathf.Lerp(dofDistanceFade[0], dofDistanceFade[1], fadeTimer / fadeTime);

            masterMixer.SetFloat("reverbDryLevel", Mathf.Lerp(dryLevelFade[0], dryLevelFade[1], fadeTimer / fadeTime));

            fadeTimer += Time.deltaTime;
            yield return null;
        }
        depthOfField.focusDistance.value = dofDistanceFade[1];
        masterMixer.SetFloat("reverbDryLevel", dryLevelFade[1]);
        awake = true;
    }
}
