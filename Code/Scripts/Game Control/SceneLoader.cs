using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [SerializeField] ScreenFade screenFade;
    [SerializeField] bool fadeOnLoad = true;
    [SerializeField] float fadeTime = 2;
    [SerializeField] float screenDistance = 1;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextAnimation loadingAnimation;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText;
    bool loadingScene = false;

    private void Start()
    {
        instance = this;
        if (fadeOnLoad)
            screenFade.Fade(1, 0, fadeTime);
    }

    public void LoadScene(int index, bool fade = true)
    {            
        StartCoroutine(LoadSceneRoutine(index, fade));
    }

    public void LoadSave(bool fade = true)
    {
        GameManager.instance.LoadLatestPlayerData();
        if(GameManager.instance.currentPlayerData != null)
            StartCoroutine(LoadSceneRoutine(GameManager.instance.currentPlayerData.sceneIndex, fade));
        else
            StartCoroutine(LoadSceneRoutine(1, fade));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex, bool fade)
    {
        if(!loadingScene)
        {
            loadingScene = true;

            loadingScreen.SetActive(true);
            loadingScreen.transform.SetPositionAndRotation(Camera.main.transform.position + (Camera.main.transform.forward * screenDistance), Quaternion.LookRotation(Camera.main.transform.forward, Player.instance.transform.up)); 
            loadingAnimation.Play();

            if(fade)
                screenFade.Fade(1, 2);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            asyncLoad.allowSceneActivation = false;

            float progress = 0;
            while(progress < 1)
            {
                progress = Mathf.Clamp01(asyncLoad.progress / .9f);

                loadingSlider.value = progress;
                loadingText.text = (Mathf.Round(progress * 1000) / 10) + "%";
                yield return null;
            }
            yield return new WaitUntil(() => screenFade.done);
            asyncLoad.allowSceneActivation = true;
            loadingScene = false;
        }
    }
}
